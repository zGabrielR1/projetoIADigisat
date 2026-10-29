"""BI read models and bounded analytics built on the existing reporting engine.

Mappings are stored only in the local Insight SQLite database. There is no ETL,
arbitrary expression execution, cross-collection join, or write to SuiteG6 here.
"""
from __future__ import annotations

from concurrent.futures import ThreadPoolExecutor
from datetime import date, timedelta
from decimal import Decimal, InvalidOperation, ROUND_HALF_UP
import hashlib
import json
from typing import Literal

from fastapi import APIRouter, HTTPException
from pydantic import BaseModel, Field, field_validator, model_validator
import insight_store as store

router = APIRouter(prefix='/api/bi', tags=['Business intelligence'])
FIELD_PATTERN = r'^[\w-]+(?:\[\])?(?:\.[\w-]+(?:\[\])?)*$'


class DataMapping(BaseModel):
    collection: str = Field(min_length=1, max_length=120)
    date_field: str = Field(min_length=1, max_length=120, pattern=FIELD_PATTERN)
    value_field: str = Field(min_length=1, max_length=120)
    product_field: str | None = Field(default=None, max_length=120, pattern=FIELD_PATTERN)
    company_field: str | None = Field(default=None, max_length=120, pattern=FIELD_PATTERN)

    @field_validator('value_field')
    @classmethod
    def valid_value(cls, value):
        import re
        if value != '@item_total' and not re.fullmatch(FIELD_PATTERN, value):
            raise ValueError('Selecione um campo de valor ou o total calculado dos itens.')
        return value

    @model_validator(mode='after')
    def compatible_grain(self):
        if '[]' in self.date_field or (self.company_field and '[]' in self.company_field):
            raise ValueError('Data e empresa devem estar no nível do documento.')
        arrays = set()
        for field in (self.value_field, self.product_field):
            if field == '@item_total':
                arrays.add('ItensBase')
            elif field and '[]' in field:
                if field.count('[]') > 1:
                    raise ValueError('Use no máximo um nível de itens embutidos.')
                arrays.add(field.split('[]')[0])
        if len(arrays) > 1:
            raise ValueError('Produto e valor devem usar a mesma lista de itens para evitar duplicação.')
        if self.product_field and '[]' in self.product_field and self.value_field != '@item_total' and '[]' not in self.value_field:
            raise ValueError('Um valor de documento não pode ser repetido para cada produto. Selecione um valor no nível dos itens.')
        return self


class ModelSave(BaseModel):
    name: str = Field(min_length=1, max_length=80)
    mapping: DataMapping

    @field_validator('name')
    @classmethod
    def clean_name(cls, value):
        if not value.strip():
            raise ValueError('Informe um nome para o modelo.')
        return value.strip()


class BIQuery(BaseModel):
    mapping: DataMapping | None = None
    model_id: str | None = Field(default=None, min_length=10, max_length=100)
    start: date
    end: date
    company: str | None = Field(default=None, max_length=120)
    movement_group: Literal['all_sales', 'fiscal_sales', 'nfce', 'nfe', 'manual', 'registro_venda', 'purchases', 'all'] = 'all_sales'
    only_valid: bool = True
    compare: bool = True

    @model_validator(mode='after')
    def bounded_period(self):
        if not self.mapping and not self.model_id:
            raise ValueError('Escolha ou configure um modelo de dados.')
        if self.mapping and self.model_id:
            raise ValueError('Envie o mapeamento ou o modelo salvo, não ambos.')
        if self.end < self.start or (self.end - self.start).days >= 366:
            raise ValueError('Escolha um período de 1 a 366 dias.')
        days = (self.end - self.start).days + 1
        if self.end == date.max or self.start.toordinal() <= days:
            raise ValueError('Período fora do intervalo de datas suportado.')
        return self


class ExploreQuery(BIQuery):
    dimension: Literal['daily', 'companies', 'top_products', 'documents'] = 'daily'
    limit: int = Field(default=100, ge=1, le=500)


class ViewSave(BaseModel):
    name: str = Field(min_length=1, max_length=80)
    query: BIQuery
    period_preset: Literal['fixed', 'month', '7', '30', 'previous'] = 'fixed'
    tab: Literal['overview', 'explore'] = 'overview'
    metric: Literal['total', 'documents'] = 'total'
    dimension: Literal['daily', 'companies', 'top_products', 'documents'] = 'daily'
    limit: int = Field(default=100, ge=1, le=500)
    search: str = Field(default='', max_length=200)
    sort: Literal['_id', 'total', 'documents', 'items', 'Data', 'Numero', 'Serie', 'Empresa', 'Produtos', 'Produto', 'Valor', 'Situacao'] | None = None
    sort_direction: Literal[-1, 1] = 1
    drill_company: str | None = Field(default=None, max_length=120)
    drill_day: date | None = None

    @field_validator('name')
    @classmethod
    def clean_name(cls, value):
        if not value.strip():
            raise ValueError('Informe um nome para a visão.')
        return value.strip()

    @model_validator(mode='after')
    def validate_drill(self):
        if self.drill_day:
            if self.period_preset != 'fixed':
                raise ValueError('Um detalhamento de dia deve usar datas fixas.')
            if not self.query.start <= self.drill_day <= self.query.end:
                raise ValueError('O dia detalhado deve pertencer ao período salvo.')
        return self


def core():
    import app
    return app


def source_identity():
    app = core()
    fingerprint = hashlib.sha256(app.MONGO_URI.encode('utf-8')).hexdigest()[:20]
    return {'database': app.MONGO_DATABASE, 'mode': app.MODE, 'connection_id': fingerprint}


def list_models():
    return [json.loads(row['body']) for row in store.read('SELECT body FROM bi_models ORDER BY updated DESC')]


@router.get('/models')
def models():
    identity = source_identity()
    return {'models': [{**model, 'available': model['source'] == identity} for model in list_models()],
            'source': identity}


@router.post('/models', status_code=201)
def save_model(req: ModelSave):
    app = core()
    schema = app.schema(req.mapping.collection)
    types = {field['name']: set(field.get('types', [])) for field in schema['fields']}
    fields = set(types)
    fields.update(field['name'] for field in schema.get('computed', []))
    for field in (req.mapping.date_field, req.mapping.value_field, req.mapping.company_field, req.mapping.product_field):
        if field and field not in fields:
            raise HTTPException(422, f'O campo {field} não aparece na amostra atual. Revise o modelo.')
    if types.get(req.mapping.date_field) and not types[req.mapping.date_field] & {'datetime', 'date'}:
        raise HTTPException(422, 'O campo de data não foi observado como data na amostra.')
    if types.get(req.mapping.value_field) and not types[req.mapping.value_field] & {'int', 'float', 'Decimal128', 'Decimal', 'Int64'}:
        raise HTTPException(422, 'O campo de valor não foi observado como número na amostra.')
    key = store.identifier()
    model = {'id': key, 'name': req.name, 'mapping': req.mapping.model_dump(),
             'source': source_identity(), 'updated': store.now(), 'sampled': schema['sampled']}
    store.write('INSERT INTO bi_models VALUES(?,?,?)', (key, store.dump(model), model['updated']))
    return model


@router.delete('/models/{key}')
def delete_model(key: str):
    rows = store.read('SELECT id FROM bi_models WHERE id=?', (key,))
    if not rows:
        raise HTTPException(404, 'Modelo não encontrado.')
    store.write('DELETE FROM bi_models WHERE id=?', (key,))
    return {'removed': True}


def resolve_mapping(req: BIQuery) -> DataMapping:
    if req.model_id:
        rows = store.read('SELECT body FROM bi_models WHERE id=?', (req.model_id,))
        if not rows:
            raise HTTPException(404, 'Modelo não encontrado.')
        model = json.loads(rows[0]['body'])
        if model['source'] != source_identity():
            raise HTTPException(409, 'Este modelo pertence a outra base ou modo de conexão. Selecione um modelo da conexão atual.')
        return DataMapping(**model['mapping'])
    return req.mapping


def view_period(preset: str, start: date, end: date, today: date):
    """Resolve a relative period when the view is opened, never when it was saved."""
    if preset == 'month':
        return today.replace(day=1), today
    if preset == 'previous':
        last = today.replace(day=1) - timedelta(days=1)
        return last.replace(day=1), last
    if preset in {'7', '30'}:
        return today - timedelta(days=int(preset) - 1), today
    return start, end


@router.get('/views')
def views():
    identity = source_identity()
    items = [json.loads(row['body']) for row in store.read('SELECT body FROM bi_views ORDER BY updated DESC')]
    return {'views': [{'id': item['id'], 'name': item['name'], 'updated': item['updated'],
                       'period_preset': item['config']['period_preset'], 'tab': item['config']['tab'],
                       'available': item['source'] == identity} for item in items]}


@router.post('/views', status_code=201)
def save_view(req: ViewSave):
    mapping = resolve_mapping(req.query)
    if (req.drill_company or req.query.company) and not mapping.company_field:
        raise HTTPException(422, 'O modelo não possui uma dimensão de empresa para este filtro.')
    if req.tab == 'explore':
        report_request(req.query, mapping, req.dimension, limit=req.limit)
    config = req.model_dump(mode='json', exclude={'name'})
    # Save the mapping itself: removing a named model must not break a saved view.
    config['query']['model_id'] = None
    config['query']['mapping'] = mapping.model_dump()
    key = store.identifier()
    record = {'id': key, 'name': req.name, 'config': config, 'source': source_identity(), 'updated': store.now()}
    store.write('INSERT INTO bi_views VALUES(?,?,?)', (key, store.dump(record), record['updated']))
    return record


@router.get('/views/{key}')
def get_view(key: str):
    rows = store.read('SELECT body FROM bi_views WHERE id=?', (key,))
    if not rows:
        raise HTTPException(404, 'Visão não encontrada. Ela pode ter sido excluída.')
    record = json.loads(rows[0]['body'])
    if record['source'] != source_identity():
        raise HTTPException(409, 'Esta visão pertence a outra base ou conexão. Abra uma visão da origem atual.')
    config = record['config']
    query = config['query']
    start, end = view_period(config['period_preset'], date.fromisoformat(query['start']), date.fromisoformat(query['end']), date.today())
    query.update(start=str(start), end=str(end))
    return record


@router.delete('/views/{key}')
def delete_view(key: str):
    if not store.read('SELECT id FROM bi_views WHERE id=?', (key,)):
        raise HTTPException(404, 'Visão não encontrada.')
    store.write('DELETE FROM bi_views WHERE id=?', (key,))
    return {'removed': True}


def report_request(req, mapping, kind, *, start=None, end=None, limit=None):
    app = core()
    if req.company and not mapping.company_field:
        raise HTTPException(422, 'Mapeie o campo de empresa para filtrar por empresa.')
    if kind == 'companies' and not mapping.company_field:
        raise HTTPException(422, 'O modelo não possui uma dimensão de empresa.')
    if kind == 'top_products' and not mapping.product_field:
        raise HTTPException(422, 'O modelo não possui uma dimensão de produto.')
    return app.ReportRequest(**mapping.model_dump(), kind=kind, start=start or req.start, end=end or req.end,
                             company_value=req.company, only_valid=req.only_valid, movement_group=req.movement_group,
                             limit=min(limit or 366, app.MAX_ROWS))


def money(value):
    amount = Decimal(str(value or 0))
    if not amount.is_finite():
        raise HTTPException(422, 'A consulta retornou um valor não finito. Revise o modelo.')
    return float(amount.quantize(Decimal('.01'), rounding=ROUND_HALF_UP))


def totals(report):
    rows = report.get('rows', [])
    total = money(rows[0].get('total')) if rows else 0.0
    docs = int(report.get('documents') or 0)
    return {'revenue': total, 'documents': docs,
            'ticket': money(Decimal(str(total)) / docs) if docs else 0.0,
            'payments': money(report['payments_total']) if report.get('payments_total') is not None else None}


def deltas(current, previous):
    result = {}
    for key, value in current.items():
        baseline = previous.get(key)
        if value is None or baseline is None:
            result[key] = None
        else:
            change = money(Decimal(str(value)) - Decimal(str(baseline)))
            result[key] = {'absolute': change, 'percent': round(change / abs(baseline) * 100, 2) if baseline else None}
    return result


@router.post('/overview')
def overview(req: BIQuery):
    app, mapping = core(), resolve_mapping(req)
    current = app.run_report(report_request(req, mapping, 'revenue'))
    days = (req.end - req.start).days + 1
    previous_end = req.start - timedelta(days=1)
    previous_start = req.start - timedelta(days=days)
    jobs = {'daily': report_request(req, mapping, 'daily')}
    if mapping.company_field:
        jobs['companies'] = report_request(req, mapping, 'companies', limit=8)
    if mapping.product_field:
        jobs['products'] = report_request(req, mapping, 'top_products', limit=8)
    if req.compare:
        jobs['previous'] = report_request(req, mapping, 'revenue', start=previous_start, end=previous_end)
        jobs['previous_daily'] = report_request(req, mapping, 'daily', start=previous_start, end=previous_end)
    results, errors = {}, {}
    with ThreadPoolExecutor(max_workers=3) as pool:
        pending = {name: pool.submit(app.run_report, request) for name, request in jobs.items()}
        for name, future in pending.items():
            try:
                results[name] = future.result()
            except HTTPException as exc:
                errors[name] = str(exc.detail)
    current_totals = totals(current)
    previous = totals(results['previous']) if 'previous' in results else None
    warnings = list(current.get('warnings', []))
    for result in results.values():
        warnings.extend(result.get('warnings', []))
    daily = results.get('daily', {}).get('rows', [])
    daily_complete = 'daily' in results and (len(daily) < min(366, app.MAX_ROWS) or days <= min(366, app.MAX_ROWS))
    previous_daily = results.get('previous_daily', {}).get('rows', [])
    previous_daily_complete = 'previous_daily' in results and (len(previous_daily) < min(366, app.MAX_ROWS) or days <= min(366, app.MAX_ROWS))
    if 'daily' in results and not daily_complete:
        warnings.append('A série diária atingiu o limite de linhas; os dias restantes não estão no gráfico. Reduza o período.')
    if 'previous_daily' in results and not previous_daily_complete:
        warnings.append('A série do período anterior está limitada; os dias não carregados ficam sem valor na comparação.')
    if 'daily' in results and daily_complete:
        daily_total = money(sum((Decimal(str(row.get('total') or 0)) for row in daily), Decimal(0)))
        if abs(daily_total - current_totals['revenue']) > .01:
            warnings.append('A série diária diverge do faturamento principal. A base pode ter mudado entre as consultas ou o modelo precisa de revisão.')
    return {'metrics': current_totals, 'previous': previous,
            'deltas': deltas(current_totals, previous) if previous is not None else None,
            'period': {'start': str(req.start), 'end': str(req.end)},
            'previous_period': {'start': str(previous_start), 'end': str(previous_end)} if req.compare else None,
            'daily': daily, 'daily_complete': daily_complete,
            'previous_daily': previous_daily, 'previous_daily_complete': previous_daily_complete,
            'companies': results.get('companies', {}).get('rows', []),
            'products': results.get('products', {}).get('rows', []),
            'scope': current.get('scope'), 'coverage': current.get('coverage'),
            'value_note': current.get('value_note'), 'warnings': list(dict.fromkeys(warnings)),
            'errors': errors, 'mapping': mapping.model_dump(), 'source': source_identity(),
            'applied_filters': {'company': req.company, 'movement_group': req.movement_group, 'only_valid': req.only_valid},
            'generated_at': store.now(), 'read_only': True,
            'rankings_limit': 8}


@router.post('/explore')
def explore(req: ExploreQuery):
    mapping = resolve_mapping(req)
    result = core().run_report(report_request(req, mapping, req.dimension, limit=req.limit))
    rows = result['rows']
    if req.dimension == 'documents' and '[]' in mapping.value_field:
        # The generic engine projects embedded values as an array for documents.
        # BI displays one document total, matching the mapped line-value measure.
        normalized = []
        for row in rows:
            value = row.get('Valor')
            if isinstance(value, list):
                try:
                    amount = sum((Decimal(str(item)) for item in value if item is not None), Decimal(0))
                    row = {**row, 'Valor': money(amount)}
                except (InvalidOperation, TypeError, ValueError):
                    raise HTTPException(422, 'O documento contém um valor de item inválido. Revise o campo de valor do modelo.') from None
            normalized.append(row)
        rows = normalized
    return {**result, 'rows': rows, 'dimension': req.dimension, 'limit': min(req.limit, core().MAX_ROWS),
            'limit_reached': len(rows) >= min(req.limit, core().MAX_ROWS),
            'applied_filters': {'company': req.company, 'movement_group': req.movement_group, 'only_valid': req.only_valid},
            'source': source_identity(), 'generated_at': store.now()}
