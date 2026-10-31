"""Independent, read-only audits of explicit normalized document snapshots.

Repository selection and domain deserialization are supplied by the exporter;
this module never claims it has reproduced those operations from raw BSON.
"""
from __future__ import annotations

import hashlib
import json
from collections import Counter
from decimal import Decimal, ROUND_DOWN, localcontext
from typing import Annotated

from fastapi import APIRouter, HTTPException
from pydantic import BaseModel, ConfigDict, Field, field_validator, model_validator

from insight_upstream_issues import ANALYZED_SHA256

router = APIRouter(prefix='/api/audit', tags=['Independent calculation audits'])
MAX_DECIMAL = 2**96 - 1
Amount = Annotated[Decimal, Field(allow_inf_nan=False)]
UnitAmount = Annotated[Decimal, Field(allow_inf_nan=False, max_digits=18, decimal_places=8)]


def checked(value: Decimal) -> Decimal:
    """Require exact representability as a .NET Decimal; never round silently."""
    sign, digits, exponent = value.as_tuple()
    coefficient = int(''.join(map(str, digits)))
    if not coefficient:
        return Decimal(0)
    if exponent > 0:
        if len(digits) + exponent > 29:
            raise ValueError('Valor/soma fora da representação decimal exata suportada.')
        coefficient *= 10**exponent
        exponent = 0
    while exponent < 0 and coefficient % 10 == 0:
        coefficient //= 10
        exponent += 1
    if coefficient > MAX_DECIMAL or exponent < -28:
        raise ValueError('Valor/soma fora da representação decimal exata suportada.')
    return value


class StrictModel(BaseModel):
    model_config = ConfigDict(extra='forbid')


class Item(StrictModel):
    seller_id: str = Field(max_length=120, pattern=r'^[A-Za-z0-9._:-]*$')
    total_liquido: Amount
    cancelled: bool
    auxiliary_document: bool
    moves_stock: bool
    moves_finance: bool

    @field_validator('total_liquido')
    @classmethod
    def valid_decimal(cls, value):
        return checked(value)


class Document(StrictModel):
    document_id: str = Field(min_length=1, max_length=120)
    month: int = Field(ge=1, le=12, strict=True)
    items: list[Item] = Field(max_length=500)


class NativeRow(StrictModel):
    seller_id: str = Field(max_length=120, pattern=r'^[A-Za-z0-9._:-]*$')
    months: list[Amount] = Field(min_length=12, max_length=12)

    @field_validator('months')
    @classmethod
    def validate_months(cls, values):
        return [checked(value) for value in values]


class NativeRun(StrictModel):
    label: str = Field(min_length=1, max_length=100)
    rows: list[NativeRow] = Field(max_length=2000)

    @model_validator(mode='after')
    def unique_sellers(self):
        keys = [row.seller_id.casefold() for row in self.rows]
        if len(keys) != len(set(keys)):
            raise ValueError('Vendedor duplicado no resultado nativo informado.')
        return self


class MonthlyAudit(StrictModel):
    schema_version: str = Field(default='normalized-monthly-v1', pattern='^normalized-monthly-v1$')
    # These fields describe the exporter-selected repository scope, not a filter
    # interpreted locally or proof of the remote server's identity.
    year: int = Field(ge=1900, le=9998, strict=True)
    company_reference: str = Field(min_length=1, max_length=120)
    repository_scope_description: str = Field(min_length=1, max_length=2000)
    declared_complete: bool = False
    seller_id: str | None = Field(default=None, max_length=120, pattern=r'^[A-Za-z0-9._:-]*$')
    documents: list[Document] = Field(max_length=5000)
    native_runs: list[NativeRun] = Field(default_factory=list, max_length=10)
    explanation_limit: int = Field(default=30, ge=0, le=200)
    difference_limit: int = Field(default=100, ge=0, le=2000)

    @model_validator(mode='after')
    def bounded_unique_input(self):
        ids = [doc.document_id for doc in self.documents]
        if len(ids) != len(set(ids)):
            raise ValueError('Documento repetido: a auditoria não pode contar a mesma referência duas vezes.')
        if sum(len(doc.items) for doc in self.documents) > 50000:
            raise ValueError('Divida o snapshot: o limite é de 50.000 itens por auditoria.')
        if len({run.label for run in self.native_runs}) != len(self.native_runs):
            raise ValueError('Identificadores de execuções nativas devem ser únicos.')
        return self


def input_identity(req: MonthlyAudit) -> str:
    # Reorderings of documents/items do not change the identity of the input
    # multiset. Comparison runs and display limits are not calculation input.
    payload = req.model_dump(mode='json', exclude={'native_runs', 'explanation_limit', 'difference_limit', 'declared_complete'})
    payload['documents'].sort(key=lambda d: d['document_id'])
    for document in payload['documents']:
        document['items'].sort(key=lambda item: json.dumps(item, sort_keys=True))
    return hashlib.sha256(json.dumps(payload, sort_keys=True, separators=(',', ':'), ensure_ascii=False).encode()).hexdigest()


def monthly_audit(req: MonthlyAudit) -> dict:
    totals: dict[str, list[Decimal]] = {}
    labels: dict[str, str] = {}
    counts = Counter()
    explanations = []
    with localcontext() as context:
        context.prec = 60
        for doc in req.documents:
            contributions: dict[str, Decimal] = {}
            excluded = Counter()
            for item in doc.items:
                counts['items_seen'] += 1
                reason = None
                if item.cancelled:
                    reason = 'cancelled'
                elif item.auxiliary_document and not item.moves_stock:
                    reason = 'auxiliary_without_stock'
                elif item.auxiliary_document and not item.moves_finance:
                    reason = 'auxiliary_without_finance'
                # The recovered per-item seller predicate is case-sensitive;
                # the original dictionary groups ObjectId strings ignoring case.
                elif req.seller_id and item.seller_id != req.seller_id:
                    reason = 'other_seller'
                if reason:
                    counts[reason] += 1
                    excluded[reason] += 1
                    continue
                key = item.seller_id.casefold()
                labels.setdefault(key, item.seller_id)
                cells = totals.setdefault(key, [Decimal(0) for _ in range(12)])
                try:
                    cells[doc.month - 1] = checked(cells[doc.month - 1] + item.total_liquido)
                except ValueError as exc:
                    raise HTTPException(422, str(exc)) from None
                contributions[key] = contributions.get(key, Decimal(0)) + item.total_liquido
                counts['items_included'] += 1
            if len(explanations) < req.explanation_limit:
                explanations.append({'document_id': doc.document_id, 'month': doc.month,
                                     'contributions': {labels[k]: str(v) for k, v in sorted(contributions.items())},
                                     'excluded_items': dict(excluded)})
        if len(totals) > 2000:
            raise HTTPException(422, 'Divida o snapshot: mais de 2.000 vendedores no resultado.')
        rows = [{'seller_id': labels[key], 'months': [str(v) for v in values],
                 'year_total': str(sum(values, Decimal(0)))} for key, values in sorted(totals.items())]
        comparisons = []
        native_values = []
        for run in req.native_runs:
            supplied = {row.seller_id.casefold(): row.months for row in run.rows}
            native_values.append(supplied)
            differences = []
            difference_count = 0

            def record_difference(difference):
                nonlocal difference_count
                difference_count += 1
                if len(differences) < req.difference_limit:
                    differences.append(difference)

            for seller in sorted(set(totals) | set(supplied)):
                expected = totals.get(seller)
                actual = supplied.get(seller)
                if expected is None or actual is None:
                    record_difference({'seller_id': labels.get(seller, seller), 'kind': 'missing_in_reference' if expected is None else 'missing_in_native',
                                        'reference': [str(x) for x in expected] if expected is not None else None,
                                        'native': [str(x) for x in actual] if actual is not None else None})
                    continue
                for month, (reference, observed) in enumerate(zip(expected, actual), 1):
                    if reference != observed:
                        record_difference({'seller_id': labels[seller], 'month': month, 'kind': 'value_difference',
                                            'reference': str(reference), 'native': str(observed),
                                            'native_minus_reference': str(observed - reference)})
            comparisons.append({'label': run.label, 'matches_snapshot_reference': difference_count == 0,
                                'difference_count': difference_count, 'differences': differences,
                                'differences_truncated': difference_count > len(differences)})
    varies = any(values != native_values[0] for values in native_values[1:]) if native_values else False
    findings = []
    if any(not result['matches_snapshot_reference'] for result in comparisons):
        findings.append('Há diferenças entre os resultados informados e o cálculo independente deste snapshot. Confira primeiro a igualdade de escopo e entrada.')
    if varies:
        findings.append('As execuções nativas informadas diferem entre si. Variação com entrada realmente congelada é compatível com o defeito de concorrência, mas não comprova sua causa.')
    if comparisons and not findings:
        findings.append('Os resultados informados coincidem com o cálculo independente deste snapshot.')
    return {'rows': rows, 'documents': len(req.documents), 'counts': dict(counts), 'comparisons': comparisons,
            'native_runs_vary': varies, 'findings': findings, 'explanations': explanations,
            'explanations_truncated': len(explanations) < len(req.documents),
            'provenance': {'method': 'independent-serial-decimal', 'snapshot_sha256': input_identity(req),
                           'analyzed_application_sha256': ANALYZED_SHA256,
                           'year': req.year, 'company_reference': req.company_reference,
                           'repository_scope_description': req.repository_scope_description,
                           'read_only': True, 'production_assembly_executed': False},
            'coverage': {'declared_complete': req.declared_complete, 'independently_verified_complete': False,
                         'raw_bson_mapping_verified': False, 'remote_build_verified': False},
            'rules': ['Entrada normalizada representa a seleção prévia do repositório; o Insight não reinterpreta origem/situação/finalidade neste endpoint.',
                      'Exclui item cancelado; item auxiliar exige movimentação de estoque e financeiro.',
                      'Agrupa pelo vendedor do item e mês de emissão; total usa TotalLiquido normalizado, sem float nem soma paralela.',
                      'Ausência de vendedor usa referência vazia; ausência de linha é distinta de uma linha com zeros.',
                      'Nenhum relatório nativo foi executado e nenhum resultado informado foi alterado.']}


@router.post('/monthly')
def audit_monthly(req: MonthlyAudit):
    return monthly_audit(req)


class UnitPriceRow(StrictModel):
    reference: str = Field(min_length=1, max_length=120)
    quantity: UnitAmount
    net_total: UnitAmount
    reported_unit_price: UnitAmount | None = None


class UnitPriceAudit(StrictModel):
    rows: list[UnitPriceRow] = Field(max_length=2000)


@router.post('/unit-prices')
def audit_unit_prices(req: UnitPriceAudit):
    rows = []
    with localcontext() as context:
        context.prec = 60
        for row in req.rows:
            if row.quantity == 0:
                rows.append({'reference': row.reference, 'status': 'undefined_zero_quantity', 'reference_unit_price': None,
                             'reported_unit_price': str(row.reported_unit_price) if row.reported_unit_price is not None else None})
                continue
            expected = (row.net_total / row.quantity).quantize(Decimal('.01'), rounding=ROUND_DOWN)
            recovered = (row.net_total / max(Decimal(1), row.quantity)).quantize(Decimal('.01'), rounding=ROUND_DOWN)
            reported = row.reported_unit_price
            rows.append({'reference': row.reference, 'quantity': str(row.quantity), 'net_total': str(row.net_total),
                         'reference_unit_price': str(expected), 'recovered_formula_unit_price': str(recovered),
                         'reported_unit_price': str(reported) if reported is not None else None,
                         'status': 'not_compared' if reported is None else 'matches' if reported == expected else 'different',
                         'reported_minus_reference': str(reported - expected) if reported is not None else None,
                         'matches_recovered_defective_formula': reported == recovered if reported is not None else None})
    return {'rows': rows, 'read_only': True, 'results_modified': False,
            'method': 'net-total / quantity; truncate toward zero to two decimal places',
            'rules': ['Quantidade zero não define preço unitário; nenhuma substituição silenciosa por 1.',
                      'Correspondência com a expressão defeituosa é um indício aritmético, não prova da versão ou causa no servidor.']}
