"""Map report entries to literal WCF calls in retained native report classes.

The installed .NET helper then verifies these calls against runtime contracts.
No service operation is inferred from a title or arbitrary user input.
"""
import hashlib
import json
from pathlib import Path
import sys

ROOT=Path(__file__).resolve().parents[1]
sys.path.insert(0,str(ROOT/'engenharia/ferramentas'))
from trace_relatorios import canais_de


def build():
    catalog=json.loads((ROOT/'static/report-catalog.json').read_text(encoding='utf-8'))
    reports={}
    for item in catalog['reports']:
        calls=[]
        sources=[]
        for source in item['sources']:
            path=(ROOT/source['path']).resolve()
            if not path.is_relative_to(ROOT):
                raise ValueError('Source escaped workspace')
            raw=path.read_bytes()
            if hashlib.sha256(raw).hexdigest()!=source['sha256']:
                raise ValueError('Retained report source changed: '+item['id'])
            sources.append(source)
            for contract,method in canais_de(raw.decode('utf-8-sig')):
                call={'contract':contract,'method':method}
                if call not in calls:
                    calls.append(call)
        if item['module'] in ['Pagamento','Recebimento']:
            # The same report class serves both modules; use the correct side.
            other='ObterListaRecebimentos' if item['module']=='Pagamento' else 'ObterListaPagamentos'
            calls=[c for c in calls if c['method']!=other]
        reports[item['id']]={'title':item['title'],'module':item['module_label'],
                             'class_name':item['class'],
                             'calls':calls,'sources':sources,
                             'status':'contract_verification_pending' if calls else 'not_in_retained_assemblies',
                             'presentation_parity':'unverified'}
    result={'schema_version':1,'target_release':'1.0.3-rc.1','reports':reports,
            'limitations':['Service DTO access does not execute local report-class post-processing or reproduce print layouts.',
                           'Retained source identities and installed service contract identities have separate verification.']}
    path=ROOT/'.release-build/native-host/map.seed.json'
    path.parent.mkdir(parents=True,exist_ok=True)
    path.write_text(json.dumps(result,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
    print(json.dumps({'entries':len(reports),'with_source_calls':sum(bool(r['calls']) for r in reports.values()),
                      'unique_calls':len({(c['contract'],c['method']) for r in reports.values() for c in r['calls']})}))


if __name__=='__main__':
    build()
