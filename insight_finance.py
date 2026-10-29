"""Read-only finance report data views traced from the installed SuiteG6 DLLs.

Amounts are calculated from parcel history, never from sales documents. Native
printer layout equivalence is separate from these paginated data views.
"""
from __future__ import annotations

from calendar import monthrange
from collections import defaultdict
from datetime import date, datetime, time, timedelta, timezone
from decimal import Decimal, InvalidOperation, ROUND_HALF_UP, localcontext
import math
import re
from time import monotonic
from typing import Literal

from bson import ObjectId
from fastapi import APIRouter, HTTPException
from pydantic import BaseModel, ConfigDict, Field, field_validator, model_validator
from pymongo.errors import PyMongoError

from insight_version import VERSION

router = APIRouter(prefix='/api/finance', tags=['Finance reports'])
MAX_PARCELS = 50000
MAX_SECONDS = 20
ZERO = Decimal(0)
MONEY_FIELDS = ('value', 'corrected', 'settled', 'interest', 'penalty', 'discount', 'fee')
SETTLED_HISTORY_TYPES = {'HistoricoQuitado', 'HistoricoQuitadoComExtratoBancario', 'HistoricoQuitadoComRetornoDePagar',
                         'HistoricoQuitadoGeracaoParcelasCartaoECarteiraDigital', 'HistoricoQuitadoParcial',
                         'HistoricoQuitadoParcialExtratoBancario', 'HistoricoQuitadoParcialGeracaoParcelasCartaoECarteiraDigital'}


def _spec(module, name, *, mode='select', group='company', person=False, general=False):
    return {'id': module + '.' + name, 'collection': 'Pagamentos' if module == 'Pagamento' else 'Recebimentos',
            'mode': mode, 'group': group, 'person_filter': person, 'general': general,
            'detail_filters': person, 'segment_filter': False, 'seller_filter': False,
            'card_filter': False, 'fee_filter': False, 'status_filter': True,
            'account_filter': module == 'Recebimento' or general,
            'dates': ['inclusion', 'due', 'settled', 'issue'] if module == 'Recebimento' or general else ['issue', 'due', 'settled'],
            'global_scope': mode == 'competence'}


SPECS = {}
for _module, _prefix in [('Pagamento', 'Pagamentos'), ('Recebimento', 'Recebimentos')]:
    for _name, _opts in [
        ('PagamentosRecebimentos', {'general': True}),
        ('PagamentosRecebimentosSintetico', {'general': True, 'group': 'company'}),
        (_prefix + 'EmAberto', {'mode': 'open'}),
        (_prefix + 'Quitados', {'mode': 'settled'}),
        (_prefix + 'QuitadosPorDataCompetencia', {'mode': 'competence'}),
        (_prefix + 'PorPessoa', {'person': True, 'group': 'person'}),
    ]:
        _item = _spec(_module, _name, **_opts)
        SPECS[_item['id']] = _item
for _name, _opts in [('PagamentosPorDiaVencimento', {'group': 'due'})]:
    _item = _spec('Pagamento', _name, **_opts)
    SPECS[_item['id']] = _item
for _name, _opts in [('RecebimentosEmAbertoSintetico', {'mode': 'open', 'group': 'person'}),
                     ('RecebimentosPorPessoa40Colunas', {'person': True, 'group': 'person'})]:
    _item = _spec('Recebimento', _name, **_opts)
    SPECS[_item['id']] = _item
for _module, _name in [('Pagamento','PagamentosPorSegmentacao'), ('Recebimento','RecebimentosPorSegmentacao'),
                       ('Recebimento','RecebimentosPorSegmentacao40Colunas')]:
    _item = _spec(_module, _name, group='person')
    _item.update(segment_filter=True, detail_filters=True)
    SPECS[_item['id']] = _item
_item = _spec('Recebimento', 'RecebimentosPorVendedor', group='person')
_item.update(seller_filter=True)
SPECS[_item['id']] = _item
for _name in ['RecebimentosCartao', 'RecebimentosTaxas']:
    _item = _spec('Recebimento', _name, person=True)
    _item.update(card_filter=True, fee_filter=_name == 'RecebimentosTaxas', status_filter=False,
                 detail_filters=False, account_filter=False, dates=['inclusion','due','issue'])
    SPECS[_item['id']] = _item


class FinanceRequest(BaseModel):
    model_config = ConfigDict(extra='forbid')
    report_id: str = Field(max_length=120)
    start: date
    end: date
    company_id: str | None = None
    consolidated: bool = False
    date_basis: Literal['inclusion', 'due', 'settled', 'issue'] = 'due'
    status: Literal['all', 'pending', 'upcoming', 'settled', 'overdue', 'reversed'] = 'all'
    person_id: str | None = None
    account_id: str | None = None
    segment_id: str | None = None
    seller_id: str | None = None
    operation: Literal['all','credit','debit'] = 'all'
    include_zero_fees: bool = True
    order_by: Literal['default','issue','due','network','fee_asc','fee_desc'] = 'default'
    plan_code: str | None = Field(default=None, max_length=80)
    cost_center_code: str | None = Field(default=None, max_length=80)
    species: int = Field(default=99, ge=0, le=99)
    offset: int = Field(default=0, ge=0, le=MAX_PARCELS)
    page_size: int = Field(default=50, ge=1, le=200)

    @field_validator('company_id', 'person_id', 'account_id', 'segment_id', 'seller_id')
    @classmethod
    def object_id(cls, value):
        if value is not None and not ObjectId.is_valid(value):
            raise ValueError('Referência inválida; selecione um cadastro da base conectada.')
        return str(ObjectId(value)) if value is not None else None

    @model_validator(mode='after')
    def valid_report(self):
        spec = SPECS.get(self.report_id)
        if not spec:
            raise ValueError('Relatório financeiro ainda não implementado.')
        if self.end < self.start or (self.end - self.start).days > 3660 or self.end == date.max:
            raise ValueError('Informe um período válido de até dez anos.')
        if spec['global_scope']:
            if self.company_id or self.account_id or self.person_id or self.consolidated:
                raise ValueError('O relatório nativo por competência consulta todas as empresas, sem filtro de conta ou pessoa.')
        elif not self.consolidated and not self.company_id:
            raise ValueError('Selecione uma empresa ou marque consolidado.')
        if self.consolidated and self.company_id:
            raise ValueError('Consolidado consulta todas as empresas; remova o filtro de empresa.')
        if self.date_basis not in spec['dates']:
            raise ValueError('Tipo de data indisponível neste relatório nativo.')
        if self.date_basis == 'inclusion' and not (1971 <= self.start.year <= self.end.year <= 2105):
            raise ValueError('A data de inclusão pelo ObjectId deve estar entre 1971 e 2105.')
        if not spec['person_filter'] and self.person_id:
            raise ValueError('Filtro de pessoa indisponível neste relatório.')
        if not spec['detail_filters'] and (self.plan_code or self.cost_center_code):
            raise ValueError('Filtros de plano e centro de custo indisponíveis neste relatório.')
        if not spec['segment_filter'] and self.segment_id:
            raise ValueError('Filtro de segmentação indisponível neste relatório.')
        if spec['seller_filter'] and (not self.seller_id or self.consolidated):
            raise ValueError('Selecione um vendedor e uma empresa. O nativo não consolida este relatório.')
        if not spec['seller_filter'] and self.seller_id:
            raise ValueError('Filtro de vendedor indisponível neste relatório.')
        if not spec['card_filter'] and (self.operation != 'all' or self.order_by != 'default'):
            raise ValueError('Operação e ordenação de cartão indisponíveis neste relatório.')
        if not spec['status_filter'] and self.status != 'all':
            raise ValueError('O relatório de cartão nativo não filtra situação.')
        if not spec['fee_filter'] and (not self.include_zero_fees or self.order_by in {'fee_asc','fee_desc'}):
            raise ValueError('Filtro e ordenação de taxa indisponíveis neste relatório.')
        if spec['fee_filter'] and self.order_by == 'network':
            raise ValueError('O relatório de taxas não possui ordenação por rede.')
        if not spec['account_filter'] and self.account_id:
            raise ValueError('O relatório nativo não aceita filtro de conta.')
        if spec['mode'] != 'open' and self.species != 99:
            raise ValueError('Espécie está disponível apenas nos relatórios em aberto.')
        if spec['mode'] != 'select' and (self.status != 'all' or self.date_basis != 'due'):
            raise ValueError('Este relatório possui situação e tipo de data fixos; use as opções padrão.')
        return self


def types(value):
    value = value.get('_t', []) if isinstance(value, dict) else []
    if isinstance(value, str):
        return [value]
    if not isinstance(value, list) or not all(isinstance(t, str) for t in value):
        raise HTTPException(422, 'Discriminador financeiro inválido; nenhum total parcial foi entregue.')
    return value


def number(value):
    try:
        if hasattr(value, 'to_decimal'):
            value = value.to_decimal()
        result = Decimal(str(value))
        if not result.is_finite() or abs(result) > Decimal('1e20'):
            raise InvalidOperation
        return result
    except (InvalidOperation, ValueError, TypeError):
        raise HTTPException(422, 'Valor financeiro inválido; nenhum total parcial foi entregue.') from None


def rounded(value, places=2):
    return value.quantize(Decimal(1).scaleb(-places), rounding=ROUND_HALF_UP)


def pro_rata_period(start: date, end: date):
    """CalculoProRataDieService: month fractions and its 365/30/28 conversion.

    Both dates are shifted by one day in the native method. CIL argument slots
    resolve the duplicate/obfuscated parameter names in the decompiler output.
    """
    start, end = start - timedelta(days=1), end - timedelta(days=1)
    start_days, end_days = monthrange(start.year, start.month)[1], monthrange(end.year, end.month)[1]
    if (start.year, start.month) == (end.year, end.month):
        months = -1
    else:
        days = (end - start).days - (start_days - start.day + end.day)
        months = int(Decimal(days) / 365) * 12
        remainder = days - int(Decimal(days) / 365) * 365
        if remainder >= 28:
            months += int(Decimal(remainder) / 30)
        if remainder % 30 >= 28:
            months += 1
    value = (start_days - start.day) / start_days + months + end.day / end_days
    # Native Math.Round(double, 4, AwayFromZero), then conversion to decimal.
    scaled = value * 10000
    result = math.copysign(math.floor(abs(scaled) + .5), scaled) / 10000
    return Decimal(format(result, '.15g'))


def charges(record, value, reference_date):
    if (record.get('Situacao') or {}).get('Codigo') != 1:
        return ZERO, ZERO
    due = record.get('Vencimento')
    if not isinstance(due, datetime) or due.year == 1:
        raise HTTPException(422, 'Parcela sem vencimento válido.')
    interest = penalty = ZERO
    juro, multa = record.get('Juro'), record.get('Multa')
    with localcontext() as context:
        context.prec = 29
        for config, is_interest in [(juro, True), (multa, False)]:
            if not config:
                continue
            percent = number(config.get('Percentual', 0))
            grace = config.get('DiasCarencia', 0)
            if not isinstance(grace, int) or not 0 <= grace <= 36500:
                raise HTTPException(422, 'Carência financeira inválida.')
            if due.date() >= reference_date - timedelta(days=grace):
                continue
            if is_interest:
                period = pro_rata_period(due.date(), reference_date)
                kind = types(config)
                if 'JuroSimples' in kind:
                    interest = rounded(value * (percent / 100) * period)
                elif 'JuroComposto' in kind:
                    try:
                        growth = math.pow(float(1 + percent / 100), float(period)) - 1
                        interest = rounded(value * number(format(growth, '.15g')))
                    except (ValueError, OverflowError):
                        raise HTTPException(422, 'Juro composto fora da faixa de cálculo.') from None
                else:
                    raise HTTPException(422, 'Tipo de juro não reconhecido; nenhum total parcial foi entregue.')
            else:
                penalty = rounded(value * percent / 100)
    return interest, penalty


def projected_amounts(record, reference_date):
    history = record.get('Historico')
    if not isinstance(history, list) or not history or not all(isinstance(h, dict) for h in history):
        raise HTTPException(422, 'Parcela sem histórico válido; nenhum total parcial foi entregue.')
    last = history[-1]
    settled = (record.get('Situacao') or {}).get('Codigo') == 3
    pending = next((h for h in reversed(history) if 'HistoricoPendente' in types(h)), None)
    payment = next((h for h in reversed(history) if SETTLED_HISTORY_TYPES.intersection(types(h))), None)
    if settled and pending is None:
        raise HTTPException(422, 'Parcela quitada sem histórico pendente para recuperar o valor original.')
    principal = number((pending if settled else last).get('Valor'))
    paid = number(payment.get('Valor')) if settled and payment else ZERO
    discount = number(payment.get('Desconto', 0)) if settled and payment else ZERO
    interest, penalty = charges(record, principal, reference_date)
    corrected = principal + interest + penalty
    fee = ZERO
    if any(t in types(record) for t in ('ParcelaRecebimentoCartao', 'ParcelaRecebimentoCarteiraDigital')):
        config = record.get('TaxaRede')
        if config:
            amount = number(config.get('PercentualTaxa', 0))
            kind = config.get('TipoTaxa', 0)
            if kind == 0:
                fee = rounded(principal * amount / 100)
            elif kind == 1:
                fee = rounded(amount) if principal > amount else ZERO
            else:
                raise HTTPException(422, 'Tipo de taxa não reconhecido; nenhum total parcial foi entregue.')
        # These subclasses override ValorTotal as Valor - ValorTaxa.
        corrected = principal - fee
    if 'HistoricoQuitadoParcial' in types(last):
        value = corrected_value = paid
    else:
        value, corrected_value = principal, corrected
    return {'value': value, 'corrected': corrected_value, 'settled': paid,
            'interest': interest, 'penalty': penalty, 'discount': discount, 'fee': fee}


def query_filter(req, reference_date):
    spec = SPECS[req.report_id]
    clauses = [{'Ativo': True}]
    if spec['card_filter']:
        clauses.append({'_t': 'ParcelaRecebimentoCartao'})
    if not req.consolidated and not spec['global_scope']:
        clauses.append({'EmpresaReferencia': ObjectId(req.company_id)})
    lower = datetime.combine(req.start, time.min)
    upper = datetime.combine(req.end + timedelta(days=1), time.min)
    basis = {'open': 'due', 'settled': 'settled', 'competence': 'competence'}.get(spec['mode'], req.date_basis)
    if basis == 'inclusion':
        # ObjectId.GenerateNewId(DateTime) uses an actual instant, unlike the
        # custom SuiteG6 serializer used for stored DateTime properties.
        lower_utc, upper_utc = lower.astimezone(timezone.utc), upper.astimezone(timezone.utc)
        clauses.append({'_id': {'$gte': ObjectId.from_datetime(lower_utc), '$lt': ObjectId.from_datetime(upper_utc)}})
    elif basis == 'competence':
        # The native spec matches ANY history's Data, not just settlement history.
        clauses.append({'Historico': {'$elemMatch': {'Data': {'$gte': lower, '$lt': upper}}}})
    else:
        field = {'due': 'Vencimento', 'settled': 'DataQuitacao', 'issue': 'DataEmissao'}[basis]
        clauses.append({field: {'$gte': lower, '$lt': upper}})
    status = {'open': 'pending', 'settled': 'settled', 'competence': 'settled'}.get(spec['mode'], req.status)
    if basis == 'settled':
        clauses.append({'Situacao.Codigo': 3})
    if status != 'all':
        clauses.append({'Situacao.Codigo': {'pending': 1, 'upcoming': 1, 'overdue': 1, 'settled': 3, 'reversed': 4}[status]})
    if status in {'upcoming', 'overdue'}:
        clauses.append({'Vencimento': {('$gte' if status == 'upcoming' else '$lt'): datetime.combine(reference_date, time.min)}})
    for key, value in [('PessoaReferencia', ObjectId(req.person_id) if req.person_id else None),
                       ('ContaReferencia', ObjectId(req.account_id) if req.account_id else None),
                       ('Historico.PlanoContaCodigoUnico', req.plan_code),
                       ('Historico.CentroCustoCodigoUnico', req.cost_center_code)]:
        if value is not None:
            clauses.append({key: value})
    return {'$and': clauses}


def iso(value):
    return value.isoformat() if isinstance(value, (datetime, date)) and value.year > 1 else None


def project(record, people, accounts, reference_date):
    amounts = projected_amounts(record, reference_date)
    last = record['Historico'][-1]
    person = people.get(record.get('PessoaReferencia'), {})
    company = people.get(last.get('EmpresaReferencia'), {})
    species = last.get('EspeciePagamento') or {}
    situation = (record.get('Situacao') or {}).get('Codigo')
    situation_label = {0: 'Aguardando', 1: 'Pendente', 3: 'Quitada', 4: 'Estornada'}.get(situation, str(situation))
    if 'HistoricoQuitadoParcial' in types(last):
        situation_label = 'Quitada parcial'
    created = record['_id'].generation_time.astimezone().replace(tzinfo=None)
    operation = ''
    if 'ParcelaRecebimentoCartao' in types(record):
        config = record.get('TaxaRede')
        operation = ('Débito' if any(t in types(config) for t in ('Debito','DebitoParcelado')) else 'Crédito') if config else {1:'Crédito',2:'Débito'}.get(species.get('TipoCartao'),'Indefinida')
    return {'id': str(record['_id']), 'company_id': str(last.get('EmpresaReferencia') or ''),
            'company': str(company.get('Nome') or ''), 'person_id': str(record.get('PessoaReferencia') or ''),
            'person': str(person.get('Nome') or ''), 'document': str(record.get('Documento') or ''),
            'order': record.get('Ordem'), 'description': str(record.get('Descricao') or ''),
            'included_at': iso(created), 'issued_at': iso(record.get('DataEmissao')),
            'due_at': iso(record.get('Vencimento')), 'settled_at': iso(record.get('DataQuitacao')),
            'last_history_at': iso(last.get('Data')), 'species': str(species.get('Descricao') or ''),
            'species_code': species.get('Codigo'), 'status': situation_label,
            'operation': operation, 'network': str(record.get('DescricaoRede') or ''),
            'account': str(accounts.get(last.get('ContaReferencia'), {}).get('Descricao') or ''),
            'observation': str(record.get('Observacao') or ''), **amounts}


def summarize(rows, group):
    totals = {key: ZERO for key in MONEY_FIELDS}
    groups = {}
    for row in rows:
        group_key = (row['company_id'], row['due_at'][:10] if group == 'due' and row['due_at'] else
                     row['person_id'] if group == 'person' else '')
        if group_key not in groups:
            groups[group_key] = {'company_id': row['company_id'], 'company': row['company'],
                                 'label': group_key[1] if group == 'due' else row['person'] if group == 'person' else row['company'],
                                 'count': 0, **{key: ZERO for key in MONEY_FIELDS}}
        groups[group_key]['count'] += 1
        for key in MONEY_FIELDS:
            totals[key] += row[key]
            groups[group_key][key] += row[key]
    return totals, sorted(groups.values(), key=lambda g: (g['company'], g['label'], g['company_id']))


def json_money(value):
    if isinstance(value, Decimal):
        return format(rounded(value), '.2f')
    if isinstance(value, dict):
        return {k: json_money(v) for k,v in value.items()}
    if isinstance(value, list):
        return [json_money(v) for v in value]
    return value


@router.get('/reports')
def metadata():
    return {'reports': list(SPECS.values()), 'max_parcels': MAX_PARCELS,
            'date_policy': 'DateTimeSerializerEx: datas gravadas sem deslocamento de fuso; inclusão usa o instante do ObjectId no fuso local do servidor.',
            'validation': 'Consultas implementadas a partir dos assemblies instalados; homologação visual com o relatório nativo pendente.'}


@router.get('/options')
def options():
    import app
    try:
        with app.mongo_client() as client:
            db = client[app.MONGO_DATABASE]
            references = set()
            for name in ['Pagamentos', 'Recebimentos']:
                references.update(db[name].distinct('EmpresaReferencia', maxTimeMS=MAX_SECONDS * 1000))
            if len(references) > 200:
                raise HTTPException(422, 'Mais de 200 empresas; configure uma base com escopo adequado.')
            companies = [{'id': str(r['_id']), 'label': str(r.get('Nome') or r['_id'])}
                         for r in db.Pessoas.find({'_id': {'$in': list(references)}}, {'Nome': 1}).max_time_ms(20000)]
            accounts = [{'id': str(r['_id']), 'label': str(r.get('Descricao') or r['_id'])}
                        for r in db.Contas.find({}, {'Descricao': 1}).limit(501).max_time_ms(20000)]
            if len(accounts) > 500:
                raise HTTPException(422, 'Mais de 500 contas; configure uma base com escopo adequado.')
            segments = [{'id': str(r['_id']), 'label': str(r.get('Descricao') or r['_id'])}
                        for r in db.TiposSegmentacao.find({}, {'Descricao':1}).limit(501).max_time_ms(20000)]
            if len(segments) > 500:
                raise HTTPException(422, 'Mais de 500 segmentações; configure uma base com escopo adequado.')
            return {'companies': sorted(companies, key=lambda r:r['label']),
                    'accounts': sorted(accounts, key=lambda r:r['label']),
                    'segments': sorted(segments, key=lambda r:r['label'])}
    except PyMongoError:
        raise HTTPException(503, 'Não foi possível carregar empresas e contas financeiras.') from None


@router.get('/people')
def people_search(q: str = ''):
    import app
    if not 2 <= len(q.strip()) <= 100:
        raise HTTPException(422, 'Informe de 2 a 100 caracteres do nome da pessoa.')
    try:
        with app.mongo_client() as client:
            cursor = client[app.MONGO_DATABASE].Pessoas.find(
                {'Nome': {'$regex': re.escape(q.strip()), '$options': 'i'}}, {'Nome': 1}).sort('Nome', 1).limit(51).max_time_ms(20000)
            rows = list(cursor)
            return {'people': [{'id': str(r['_id']), 'label': str(r.get('Nome') or '')} for r in rows[:50]],
                    'has_more': len(rows) > 50}
    except PyMongoError:
        raise HTTPException(503, 'Não foi possível pesquisar pessoas na base conectada.') from None


def execute(req, *, reference_date=None, include_analysis=False, analysis_person_id=None):
    import app
    started = monotonic()
    reference_date = reference_date or date.today()
    spec = SPECS[req.report_id]
    def remaining_ms():
        remaining = MAX_SECONDS - (monotonic() - started)
        if remaining <= 0:
            raise HTTPException(422, 'Consulta financeira excedeu 20 segundos; reduza o período.')
        return max(1, int(remaining * 1000))
    try:
        with app.mongo_client() as client:
            db = client[app.MONGO_DATABASE]
            if spec['collection'] not in db.list_collection_names(maxTimeMS=remaining_ms()):
                raise HTTPException(422, 'A base não possui a coleção financeira exigida.')
            # Project the supported state, not whole documents or embedded contacts.
            projection = {key: 1 for key in ['Historico', 'Juro', 'Multa', 'TaxaRede', 'DescricaoRede', 'Situacao', 'Vencimento', 'PessoaReferencia',
                          'DataEmissao', 'DataQuitacao', 'Documento', 'Descricao', 'Ordem', 'Observacao', '_t']}
            match = query_filter(req, reference_date)
            related = None
            if req.segment_id:
                related = {'Segmentacao.Tipo._id': ObjectId(req.segment_id)}
            elif req.seller_id:
                related = {'Cliente': {'$ne': None}, 'Cliente.VendedorReferencia': ObjectId(req.seller_id),
                           '_t': {'$ne': 'Consumidor'}}
            if related is not None:
                ids = [r['_id'] for r in db.Pessoas.find(related, {'_id':1}).limit(MAX_PARCELS+1).max_time_ms(remaining_ms())]
                if len(ids) > MAX_PARCELS:
                    raise HTTPException(422, 'Mais de 50.000 pessoas relacionadas; consulta não executada.')
                if req.seller_id and not ids:
                    raise HTTPException(422, 'Nenhum cliente vinculado ao vendedor informado.')
                match['$and'].append({'PessoaReferencia': {'$in': ids}})
            records = list(db[spec['collection']].find(match, projection)
                           .sort('_id', 1).limit(MAX_PARCELS + 1).max_time_ms(remaining_ms()))
            if len(records) > MAX_PARCELS:
                raise HTTPException(422, 'Mais de 50.000 parcelas; reduza o período. Nenhum total parcial foi entregue.')
            people_refs, account_refs = set(), set()
            for record in records:
                history = record.get('Historico')
                if not isinstance(history, list) or not history or not all(isinstance(h, dict) for h in history):
                    raise HTTPException(422, 'Parcela sem histórico válido; nenhum total parcial foi entregue.')
                people_refs.add(record.get('PessoaReferencia'))
                for h in history:
                    people_refs.add(h.get('EmpresaReferencia'))
                    account_refs.add(h.get('ContaReferencia'))
            people = {r['_id']: r for r in db.Pessoas.find({'_id': {'$in': [x for x in people_refs if x]}}, {'Nome': 1}).max_time_ms(remaining_ms())}
            accounts = {r['_id']: r for r in db.Contas.find({'_id': {'$in': [x for x in account_refs if x]}}, {'Descricao': 1}).max_time_ms(remaining_ms())}
            rows = []
            for record in records:
                if monotonic() - started > MAX_SECONDS:
                    raise HTTPException(422, 'Consulta financeira excedeu 20 segundos; reduza o período.')
                row = project(record, people, accounts, reference_date)
                if analysis_person_id is not None and row['person_id'] != analysis_person_id:
                    continue
                if req.species != 99 and row['species_code'] != req.species:
                    continue
                if req.operation != 'all' and row['operation'] != {'credit':'Crédito','debit':'Débito'}[req.operation]:
                    continue
                if not req.include_zero_fees and row['fee'] <= 0:
                    continue
                rows.append(row)
    except PyMongoError:
        raise HTTPException(503, 'Consulta financeira indisponível ou excedeu o prazo. Confira a conexão.') from None
    remaining_ms()
    if spec['card_filter']:
        order = req.order_by if req.order_by != 'default' else 'issue' if spec['fee_filter'] else 'network'
        key = {'issue':'issued_at','due':'due_at','network':'network','fee_asc':'fee','fee_desc':'fee'}[order]
        rows.sort(key=lambda r:(r[key] if r[key] is not None else '', r['id']), reverse=order=='fee_desc')
    elif spec['group'] == 'due':
        rows.sort(key=lambda r:(r['company_id'], r['due_at'] or '', r['id']))
    else:
        rows.sort(key=lambda r:(r['company_id'], r['person'], r['due_at'] or '', r['id']))
    totals, groups = summarize(rows, spec['group'])
    analysis = {}
    if include_analysis:
        from insight_bi_finance import analyze_rows
        analysis['analysis'] = analyze_rows(rows, reference_date)
        remaining_ms()
    page = rows[req.offset:req.offset + req.page_size]
    return json_money({**analysis, 'report_id': req.report_id, 'rows': page, 'totals': totals, 'groups': groups,
                       'record_count': len(rows), 'pagination': {'offset': req.offset, 'page_size': req.page_size,
                       'has_more': req.offset + len(page) < len(rows)},
                       'warnings': ['Homologação visual contra a saída nativa pendente; esta visão não reproduz o layout de impressão.',
                                    'Paginação consulta a base novamente; alterações concorrentes podem mudar os resultados.',
                                    'Totais e agrupamentos incluem todas as parcelas filtradas; CSV contém somente a página exibida.'],
                       'provenance': {'version': VERSION, 'database': app.MONGO_DATABASE, 'collection': spec['collection'],
                                      'reference_date': reference_date.isoformat(), 'read_only': True,
                                      'date_policy': 'SuiteG6 DateTimeSerializerEx', 'source': 'live',
                                      'duration_ms': round((monotonic() - started) * 1000, 1)}})


@router.post('/query')
def query(req: FinanceRequest):
    return execute(req)
