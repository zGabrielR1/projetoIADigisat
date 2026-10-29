"""Read-only financial BI over the same complete parcel set as finance reports."""
from collections import defaultdict
from datetime import date
from decimal import Decimal

from fastapi import APIRouter, HTTPException
from pydantic import BaseModel, ConfigDict, Field, ValidationError, field_validator
from bson import ObjectId

from insight_finance import FinanceRequest, execute, rounded

router = APIRouter(prefix='/api/bi/finance', tags=['Financial BI'])
BANDS = ('A vencer', 'Vence hoje', '1–7 dias', '8–30 dias', '31–60 dias', '61–90 dias', 'Mais de 90 dias')


def analyze_rows(rows, reference_date):
    """Sum displayed parcel cents, before pagination; stable IDs preserve homonyms."""
    buckets = {label: {'label': label, 'count': 0, 'amount': Decimal(0)} for label in BANDS}
    people, daily = {}, defaultdict(lambda: {'count': 0, 'amount': Decimal(0)})
    total = overdue = Decimal(0)
    overdue_count = 0
    for row in rows:
        if not row.get('due_at'):
            raise HTTPException(422, 'Parcela sem vencimento válido; nenhum total parcial foi entregue.')
        due = date.fromisoformat(row['due_at'][:10])
        days = (reference_date - due).days
        band = BANDS[0 if days < 0 else 1 if days == 0 else 2 if days <= 7 else 3 if days <= 30 else 4 if days <= 60 else 5 if days <= 90 else 6]
        amount = rounded(row['corrected'])
        total += amount
        buckets[band]['amount'] += amount
        buckets[band]['count'] += 1
        if days > 0:
            overdue += amount
            overdue_count += 1
        day = daily[due.isoformat()]
        day['amount'] += amount
        day['count'] += 1
        key = row['person_id']
        person = people.setdefault(key, {'id': key, 'label': row['person'] or 'Pessoa não identificada', 'count': 0, 'amount': Decimal(0), 'overdue': Decimal(0)})
        person['count'] += 1
        person['amount'] += amount
        if days > 0:
            person['overdue'] += amount
    return {'total': total, 'count': len(rows), 'overdue': overdue, 'overdue_count': overdue_count,
            'aging': list(buckets.values()), 'daily': [{'date': key, **value} for key, value in sorted(daily.items())],
            'people': sorted(people.values(), key=lambda p: (-p['amount'], p['id']))[:8]}


class FinancialScope(BaseModel):
    model_config = ConfigDict(extra='forbid')
    start: date
    end: date
    company_id: str | None = None
    account_id: str | None = None

    def request(self, side, **extra):
        if side not in ('receivable', 'payable'):
            raise HTTPException(422, 'Tipo de parcela inválido.')
        try:
            return FinanceRequest(report_id=('Recebimento' if side == 'receivable' else 'Pagamento') + '.PagamentosRecebimentos',
                                  start=self.start, end=self.end, company_id=self.company_id,
                                  consolidated=not bool(self.company_id), account_id=self.account_id,
                                  date_basis='due', status='pending', **extra)
        except ValidationError as exc:
            raise HTTPException(422, '; '.join(e['msg'] for e in exc.errors())) from None


class ParcelScope(FinancialScope):
    side: str
    person_id: str | None = None
    offset: int = Field(default=0, ge=0, le=50000)

    @field_validator('person_id')
    @classmethod
    def valid_person(cls, value):
        if value is not None and not ObjectId.is_valid(value):
            raise ValueError('Pessoa inválida.')
        return str(ObjectId(value)) if value is not None else None


@router.post('/overview')
def overview(req: FinancialScope):
    reference = date.today()
    # Validate both scopes before performing reads. A failed source stays unavailable.
    requests = {side: req.request(side) for side in ('receivable', 'payable')}
    result = {'reference_date': reference.isoformat(), 'start': req.start.isoformat(), 'end': req.end.isoformat(), 'sides': {}}
    for side, query in requests.items():
        try:
            data = execute(query, reference_date=reference, include_analysis=True)
            result['sides'][side] = {'analysis': data['analysis'], 'provenance': data['provenance']}
        except HTTPException as exc:
            result['sides'][side] = {'error': str(exc.detail)}
    return result


@router.post('/parcels')
def parcels(req: ParcelScope):
    return execute(req.request(req.side, offset=req.offset, page_size=50), analysis_person_id=req.person_id)
