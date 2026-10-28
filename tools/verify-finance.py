"""Read-only finance parity checks with independent in-memory predicates.

Exports scenario counts/pass flags only, not parcel documents or personal data.
Does not claim comparison with the native application's printed output.
"""
from __future__ import annotations
from datetime import date, datetime, timedelta
from decimal import Decimal, ROUND_HALF_UP
import json
from pathlib import Path
import sys

ROOT=Path(__file__).resolve().parents[1]
sys.path.insert(0,str(ROOT))
import app
import insight_finance as finance


def discriminators(record):
    value=record.get('_t',[])
    return [value] if isinstance(value,str) else value


def number(value):
    return Decimal(str(value.to_decimal() if hasattr(value,'to_decimal') else value))


def cash(value):
    return format(value.quantize(Decimal('.01'),rounding=ROUND_HALF_UP),'.2f')


def amounts(record):
    history=record['Historico'];code=record['Situacao']['Codigo']
    paid=next((h for h in reversed(history) if any(t.startswith('HistoricoQuitado') for t in discriminators(h))),{})
    pending=next((h for h in reversed(history) if 'HistoricoPendente' in discriminators(h)),{})
    principal=number((pending if code==3 else history[-1])['Valor'])
    settled=number(paid.get('Valor',0)) if code==3 else Decimal(0)
    discount=number(paid.get('Desconto',0)) if code==3 else Decimal(0)
    fee=Decimal(0)
    is_fee=any(t in discriminators(record) for t in ['ParcelaRecebimentoCartao','ParcelaRecebimentoCarteiraDigital'])
    if is_fee and record.get('TaxaRede'):
        rate=record['TaxaRede'];value=number(rate['PercentualTaxa'])
        fee=number(cash(principal*value/100)) if rate['TipoTaxa']==0 else number(cash(value)) if principal>value else Decimal(0)
    partial='HistoricoQuitadoParcial' in discriminators(history[-1])
    return {'value':settled if partial else principal,'settled':settled,'discount':discount,'fee':fee}


def selected(record, req, people):
    rid=req.report_id
    if record.get('Ativo') is not True:
        return False
    if 'PorDataCompetencia' not in rid and not req.consolidated and str(record.get('EmpresaReferencia'))!=req.company_id:
        return False
    types=discriminators(record)
    if any(tag in rid for tag in ['RecebimentosCartao','RecebimentosTaxas']) and 'ParcelaRecebimentoCartao' not in types:
        return False
    person=people.get(record.get('PessoaReferencia'),{})
    if req.seller_id and (not person.get('Cliente') or str(person['Cliente'].get('VendedorReferencia'))!=req.seller_id or 'Consumidor' in discriminators(person)):
        return False
    if req.segment_id and str(((person.get('Segmentacao') or {}).get('Tipo') or {}).get('_id'))!=req.segment_id:
        return False
    status=record['Situacao']['Codigo']
    if 'EmAberto' in rid:
        if status!=1:return False
        timestamps=[record['Vencimento']]
    elif 'QuitadosPorDataCompetencia' in rid:
        if status!=3:return False
        timestamps=[h.get('Data') for h in record['Historico']]
    elif 'Quitados' in rid:
        if status!=3:return False
        timestamps=[record.get('DataQuitacao')]
    else:
        timestamps=[record['Vencimento']]
    return any(isinstance(t,datetime) and req.start<=t.date()<=req.end for t in timestamps)


def main():
    with app.mongo_client() as client:
        db=client[app.MONGO_DATABASE]
        data={name:list(db[name].find({}).limit(50001).max_time_ms(20000)) for name in ['Pagamentos','Recebimentos']}
        if any(len(rows)>50000 for rows in data.values()):
            raise RuntimeError('Verification limited to 50,000 parcels per collection')
        references={r.get('EmpresaReferencia') for rows in data.values() for r in rows if r.get('EmpresaReferencia')}
        persons={r.get('PessoaReferencia') for rows in data.values() for r in rows if r.get('PessoaReferencia')}
        people={p['_id']:p for p in db.Pessoas.find({'_id':{'$in':list(persons)}}, {'Cliente':1,'Segmentacao':1,'_t':1}).max_time_ms(20000)}
    if not references:
        raise RuntimeError('No company with financial parcels in the connected database')
    company=sorted(references,key=str)[0]
    dates=[r['Vencimento'].date() for rows in data.values() for r in rows if isinstance(r.get('Vencimento'),datetime) and r['Vencimento'].year>1970]
    end=max(dates+[date.today()]);start=max(min(dates),end-timedelta(days=3659))
    seller=next((p['Cliente']['VendedorReferencia'] for p in people.values() if p.get('Cliente') and p['Cliente'].get('VendedorReferencia') and 'Consumidor' not in discriminators(p)),None)
    segment=next((((p.get('Segmentacao') or {}).get('Tipo') or {}).get('_id') for p in people.values() if ((p.get('Segmentacao') or {}).get('Tipo') or {}).get('_id')),None)
    results=[]
    for rid,spec in finance.SPECS.items():
        if spec['seller_filter'] and seller is None:
            results.append({'report_id':rid,'status':'unavailable','reason':'No linked customer/seller in this database; isolated fixture covers the query.'})
            continue
        req=finance.FinanceRequest(report_id=rid,start=start,end=end,
            company_id=None if spec['global_scope'] else str(company),
            seller_id=str(seller) if spec['seller_filter'] else None,
            segment_id=str(segment) if spec['segment_filter'] and segment else None,page_size=200)
        expected=[r for r in data[spec['collection']] if selected(r,req,people)]
        sums={key:Decimal(0) for key in ['value','settled','discount','fee']}
        for record in expected:
            for key,value in amounts(record).items():sums[key]+=value
        actual=finance.execute(req)
        if actual['record_count']!=len(expected):
            raise RuntimeError('Independent count mismatch: '+rid)
        if any(actual['totals'][key]!=cash(value) for key,value in sums.items()):
            raise RuntimeError('Independent monetary mismatch: '+rid)
        if len(actual['rows'])==len(expected) and {r['id'] for r in actual['rows']}!={str(r['_id']) for r in expected}:
            raise RuntimeError('Independent selected-ID mismatch: '+rid)
        results.append({'report_id':rid,'status':'passed','record_count':len(expected),
                        'checks':['independent_predicate_count','principal','settled','discount','fee','selected_ids_when_single_page'],
                        'segment_join_exercised':bool(req.segment_id)})
    result={'target_release':'1.0.3-rc.1','source':'live_read_only','period':{'start':str(start),'end':str(end)},
            'limitations':['No native application execution or printed-report comparison.',
                           'Local database snapshot may change concurrently.',
                           'Interest/penalty arithmetic is checked separately with 432 reconstructed C# vectors.',
                           'Absent segmentation fixtures in the live base are covered only by isolated integration tests.'],
            'scenarios':results}
    (ROOT/'docs/finance-verification-1.0.3-rc.1.json').write_text(json.dumps(result,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
    print(json.dumps({'passed':sum(r['status']=='passed' for r in results),'unavailable':sum(r['status']=='unavailable' for r in results)}))


if __name__=='__main__':
    main()
