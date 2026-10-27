"""Read-only lot expiry, independent of sales plans and monetary totals."""
from __future__ import annotations

from collections import defaultdict
from datetime import date, datetime, timedelta
from decimal import Decimal, InvalidOperation
import math
from time import monotonic

from bson import ObjectId
from fastapi import HTTPException
from pydantic import BaseModel, ConfigDict, Field, field_validator
from pymongo.errors import PyMongoError

import insight_store as store
from insight_version import VERSION

MAX_LOTS = 20000


class ExpiryRequest(BaseModel):
    model_config = ConfigDict(extra='forbid')
    company_id: str
    reference_date: date = Field(default_factory=date.today)
    horizon_days: int = Field(default=30, ge=0, le=366)
    include_expired: bool = False
    positive_only: bool = True
    search: str = Field(default='', max_length=100)
    page_size: int = Field(default=50, ge=1, le=200)
    offset: int = Field(default=0, ge=0, le=MAX_LOTS)

    @field_validator('company_id')
    @classmethod
    def valid_company(cls, value):
        if not ObjectId.is_valid(value):
            raise ValueError('Selecione uma empresa válida.')
        return str(ObjectId(value))


def number(value):
    try:
        if hasattr(value, 'to_decimal'):
            value = value.to_decimal()
        result = Decimal(str(value))
        if not result.is_finite() or not math.isfinite(float(result)):
            raise InvalidOperation
        return result
    except (InvalidOperation, TypeError, ValueError, OverflowError):
        raise HTTPException(422, 'Quantidade de lote inválida. Confira o cadastro e os movimentos de estoque.') from None


def stock_rows(records, req: ExpiryRequest, *, deadline=None):
    """Reproduce ItemEstoque.Processar/Quantidade from signed stock movements."""
    result = []
    examined = 0
    movement_count = 0
    upper = req.reference_date + timedelta(days=req.horizon_days)
    search = req.search.strip().casefold()
    for record in records:
        balances = defaultdict(Decimal)
        stock = record.get('stock') or {}
        for movement in stock.get('Quantidades') or []:
            movement_count += 1
            if movement_count % 512 == 0 and deadline is not None and monotonic() > deadline:
                raise HTTPException(422, 'A consulta de lotes excedeu 20 segundos. Tente novamente em um horário de menor carga.')
            balances[str(movement.get('EstoqueReferencia'))] += number(movement.get('Quantidade'))
        product = record.get('product') or {}
        for lot in stock.get('Itens') or []:
            examined += 1
            if examined > MAX_LOTS:
                raise HTTPException(422, 'A consulta excede 20.000 lotes. Nenhum resultado parcial foi entregue.')
            if deadline is not None and monotonic() > deadline:
                raise HTTPException(422, 'A consulta de lotes excedeu 20 segundos. Tente novamente em um horário de menor carga.')
            types = lot.get('_t') or []
            if isinstance(types, str):
                types = [types]
            if any(t in types for t in ('ItemEstoqueLoteGrade', 'ItemEstoqueLoteSerial')):
                raise HTTPException(422, 'Lotes com grade ou serial ainda não são suportados. Nenhum saldo parcial foi entregue.')
            expiry = lot.get('DataValidade')
            if not isinstance(expiry, datetime):
                raise HTTPException(422, 'Lote sem data de validade reconhecida. Confira o cadastro.')
            expires = expiry.date()
            if expires > upper or (not req.include_expired and expires <= req.reference_date):
                continue
            quantity = balances[str(lot.get('_id'))]
            if req.positive_only and quantity <= 0:
                continue
            description = str(product.get('Descricao') or '')
            code = str(product.get('CodigoInterno') or '')
            lot_number = str(lot.get('Numero') or '')
            if search and search not in (' '.join([description, code, lot_number])).casefold():
                continue
            days = (expires - req.reference_date).days
            result.append({
                'Codigo': code, 'Produto': description, 'Lote': lot_number,
                'Validade': expires.isoformat(), 'Quantidade': float(quantity),
                'Unidade': (product.get('UnidadeMedida') or {}).get('Sigla') or '',
                'SituacaoLote': 'Vencido' if days < 0 else 'Vence hoje' if days == 0 else 'A vencer',
                'DiasParaVencer': days, '_order': (expires, code, lot_number, str(record.get('link_id')), str(lot.get('_id'))),
            })
    result.sort(key=lambda row: row['_order'])
    for row in result:
        row.pop('_order')
    return result


def pipeline(req):
    # MongoDB 3.4-compatible joins; all stage/collection/field names are fixed.
    return [
        {'$match': {'_t': 'EstoqueLote'}},
        {'$project': {'Itens': 1, 'Quantidades.EstoqueReferencia': 1, 'Quantidades.Quantidade': 1}},
        {'$lookup': {'from': 'ProdutosServicosEmpresa', 'localField': '_id', 'foreignField': 'EstoqueReferencia', 'as': 'link'}},
        {'$unwind': '$link'},
        {'$match': {'link.EmpresaReferencia': ObjectId(req.company_id), 'link._t': 'ProdutoEmpresa'}},
        {'$lookup': {'from': 'ProdutosServicos', 'localField': 'link.ProdutoServicoReferencia', 'foreignField': '_id', 'as': 'product'}},
        {'$unwind': '$product'},
        {'$match': {'product.Ativo': True, 'product._t': 'Produto'}},
        {'$project': {'_id': 0, 'link_id': '$link._id',
                      'stock': {'Itens': '$Itens', 'Quantidades': '$Quantidades'},
                      'product': {'Descricao': '$product.Descricao', 'CodigoInterno': '$product.CodigoInterno',
                                  'UnidadeMedida': '$product.UnidadeMedida'}}},
    ]


def companies():
    import app
    try:
        with app.mongo_client() as client:
            db = client[app.MONGO_DATABASE]
            refs = db.ProdutosServicosEmpresa.distinct('EmpresaReferencia', maxTimeMS=20000)
            if len(refs) > 200:
                raise HTTPException(422, 'Mais de 200 empresas encontradas. Configure uma base com escopo adequado.')
            names = {row['_id']: row.get('Nome') for row in db.Pessoas.find({'_id': {'$in': refs}}, {'Nome': 1}).max_time_ms(20000)}
            return {'companies': [{'id': str(ref), 'name': names.get(ref) or str(ref)} for ref in sorted(refs, key=str)]}
    except PyMongoError:
        raise HTTPException(503, 'Não foi possível carregar as empresas. Confira a conexão.') from None


def query_expiry(req: ExpiryRequest):
    import app
    started = monotonic()
    try:
        with app.mongo_client() as client:
            db = client[app.MONGO_DATABASE]
            required = {'Estoques', 'ProdutosServicosEmpresa', 'ProdutosServicos'}
            if not required.issubset(db.list_collection_names()):
                raise HTTPException(422, 'Esta base não possui as coleções necessárias para consultar lotes.')
            cursor = db.Estoques.aggregate(pipeline(req), maxTimeMS=20000, allowDiskUse=False, batchSize=25)
            try:
                rows = stock_rows(cursor, req, deadline=started + 20)
            finally:
                cursor.close()
    except PyMongoError:
        raise HTTPException(503, 'Não foi possível consultar os lotes no prazo. Confira a conexão ou tente novamente.') from None
    total = len(rows)
    page = rows[req.offset:req.offset + req.page_size]
    summary = [f'Empresa: {req.company_id}', f'Referência: {req.reference_date}; horizonte: {req.horizon_days} dias.',
               'Inclui vencidos e vencimentos de hoje.' if req.include_expired else 'Somente datas posteriores à referência; vencimentos de hoje ficam fora.',
               'Somente quantidades positivas.' if req.positive_only else 'Inclui quantidades zero e negativas.',
               f'Busca: {req.search or "sem filtro"}. Página com {len(page)} de {total} lotes.']
    result = {'module': 'stock_expiry', 'rows': page, 'total_lots': total,
              'pagination': {'offset': req.offset, 'page_size': req.page_size, 'has_more': req.offset + len(page) < total},
              'interpretation': {'kind_label': 'Validade de lotes', 'summary': summary},
              'warnings': ['Quantidade física calculada pela soma dos movimentos assinados de cada lote; reservas não são descontadas.',
                           'Quantidades de produtos com unidades diferentes não são somadas.',
                           'Exportações preservam somente a página exibida.'],
              'provenance': {'version': VERSION, 'database': app.MONGO_DATABASE, 'source': 'live',
                             'queried_at': store.now(), 'reference_date': str(req.reference_date)},
              'duration_ms': round((monotonic() - started) * 1000, 1)}
    run_id = store.save_run({'module': 'stock_expiry', **req.model_dump(mode='json')}, result)
    return {'run_id': run_id, 'result': result}
