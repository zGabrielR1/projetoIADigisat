"""Read-only diagnostics for the DigiSat sales reports.

Reproduces the monthly report (`Item.TotalLiquido` per seller) and explains
document-vs-seller differences in the simplified movement report. It never
writes to MongoDB.

Usage:
  python engenharia/ferramentas/diagnostico_relatorios.py --year 2026
  python engenharia/ferramentas/diagnostico_relatorios.py --year 2026 --company "EMPRESA"
  python engenharia/ferramentas/diagnostico_relatorios.py --date 2026-10-05 --seller "VENDEDORA 2"
  python engenharia/ferramentas/diagnostico_relatorios.py --date 2026-10-05 --seller-ref 6496ee283a5e352378b9e8f1
  python engenharia/ferramentas/diagnostico_relatorios.py --explain
"""
from __future__ import annotations

import argparse
import math
import sys
from collections import defaultdict
from datetime import date, datetime, timedelta
from pathlib import Path

ROOT = Path(__file__).resolve().parents[2]
sys.path.insert(0, str(ROOT))

import app  # noqa: E402
from bson import ObjectId  # noqa: E402

MONTHS = ["Jan", "Fev", "Mar", "Abr", "Mai", "Jun", "Jul", "Ago", "Set", "Out", "Nov", "Dez"]

ITEM_FIELDS = [
    "VendedorReferencia", "Quantidade", "PrecoUnitario", "DescontoDigitado", "DescontoProporcional",
    "OutrasDespesasProporcional", "OutrasDespesasDigitado", "Percentual.ValorSt",
    "IPI.Valor", "Frete", "Seguro", "Cancelado",
]
ITEM_PROJECTION = {f"ItensBase.{field}": 1 for field in ITEM_FIELDS}
SHARED_PROJECTION = {
    "Numero": 1, "Serie": 1, "Pessoa.Nome": 1, "DataHoraEmissao": 1,
    "ItensBase.VendedorReferencia": 1, "ItensBase.ProdutoServico.CodigoInterno": 1,
    "ItensBase.ProdutoServico.Descricao": 1, "ItensBase.Numero": 1,
    "ItensPagamentos.Valor": 1, "Pagamentos.Valor": 1,
    "ItensPagamentos.EspeciePagamento.Descricao": 1,
    **ITEM_PROJECTION,
}

EXPLAIN = """\
Como esta ferramenta lê a base (somente leitura)
------------------------------------------------
Relatório mensal:
  - Filtra NFC-e com situação "Concluído", data de emissão, finalidade normal
    (FinalidadeEmissaoNfe.Codigo = 1) e, se --company for informado, a empresa.
    A base ativa tem uma empresa; sem --company o total é de todas.
  - Soma o TotalLiquido de cada item não cancelado, agrupado por mês e por
    VendedorReferencia. É a mesma regra de ObterTotalMovimentacoesPorMesPorVendedor.
  - TotalLiquido = round2(preço × quantidade) - descontos do item
    + outras despesas + ST + IPI + frete + seguro.
  - Cartão, PIX, taxas e parcelas NÃO são descontados. O relatório é bruto
    em relação à forma de pagamento e líquido apenas de descontos do item.

Documentos compartilhados:
  - O relatório simplificado usa Total = total do documento inteiro.
  - Quando um vendedor é selecionado, Itens = somente os itens desse vendedor.
  - Em documento com dois vendedores, a diferença entre o total do documento
    e o total do vendedor é o valor dos itens dos outros vendedores.
  - "pago" é a soma de ItensPagamentos[].Valor (ou Pagamentos[].Valor).

Parâmetros
----------
  --year ANO
      Ano do relatório mensal (ex.: 2026). Usa 01/01 a 31/12 do ano.
  --company NOME
      Empresa para o relatório mensal. Opcional; sem ele, soma todas as
      empresas da base. Use o nome exato de Empresa.Nome.
  --date AAAA-MM-DD
      Dia do relatório de documentos compartilhados. A data é inclusiva.
  --seller NOME
      Nome (ou parte) do vendedor. Se houver mais de uma pessoa, a ferramenta
      escolhe a que tem mais movimentações; se empatar, pede --seller-ref.
  --seller-ref OBJECTID
      Referência exata do vendedor (24 caracteres hexadecimais). Use quando o
      nome for ambíguo.
  --explain
      Mostra esta explicação e não consulta a base.

Colunas da saída de documentos compartilhados
---------------------------------------------
  documento   total do documento (todos os itens)
  VENDEDORA   total somente dos itens do vendedor escolhido
  outros      documento - vendedor (itens dos outros vendedores)
  pago        soma dos pagamentos registrados no documento
"""


def round2(value: float) -> float:
    return math.trunc(value * 100 + (0.500000001 if value >= 0 else -0.500000001)) / 100


def item_total(item: dict) -> float:
    def field(*path):
        current = item
        for key in path:
            if not isinstance(current, dict):
                return 0.0
            current = current.get(key)
        return float(current or 0)

    return (round2(float(item.get("Quantidade") or 0) * float(item.get("PrecoUnitario") or 0))
            - field("DescontoDigitado") - field("DescontoProporcional")
            + field("OutrasDespesasProporcional") + field("OutrasDespesasDigitado")
            + field("Percentual", "ValorSt") + field("IPI", "Valor")
            + field("Frete") + field("Seguro"))


def seller_names(db) -> dict[str, str]:
    return {str(person["_id"]): person.get("Nome", "") for person in db["Pessoas"].find({}, {"Nome": 1})}


def monthly_by_seller(db, year: int, company: str | None = None) -> None:
    coll = db["Movimentacoes"]
    match = {
        "DataHoraEmissao": {"$gte": datetime(year, 1, 1), "$lt": datetime(year + 1, 1, 1)},
        "_t": "NotaFiscalConsumidorEletronica",
        "Situacao.Descricao": "Concluído",
        "FinalidadeEmissaoNfe.Codigo": 1,
    }
    if company:
        match["Empresa.Nome"] = company
    pipeline = [
        {"$match": match},
        {"$project": {"DataHoraEmissao": 1, **ITEM_PROJECTION}},
        {"$unwind": "$ItensBase"},
        {"$match": {"ItensBase.Cancelado": {"$ne": True}}},
        {"$project": {
            "mes": {"$month": "$DataHoraEmissao"},
            "vendedor": "$ItensBase.VendedorReferencia",
            "valor": app.rebase(app.COMPUTED_VALUES["@item_total"], "$ItensBase."),
        }},
        {"$group": {"_id": {"mes": "$mes", "vendedor": "$vendedor"},
                    "total": {"$sum": "$valor"}, "itens": {"$sum": 1}}},
    ]
    rows = list(coll.aggregate(pipeline, maxTimeMS=60000, allowDiskUse=False))
    names = seller_names(db)
    by_seller: dict[str, list[float]] = defaultdict(lambda: [0.0] * 12)
    totals = [0.0] * 12
    for row in rows:
        month = row["_id"]["mes"]
        seller = str(row["_id"]["vendedor"]) if row["_id"]["vendedor"] else ""
        value = float(row["total"] or 0)
        by_seller[seller][month - 1] += value
        totals[month - 1] += value

    print(f"Monthly totals {year} (NFC-e, Concluído, item TotalLiquido)")
    print("Seller".ljust(22), " ".join(month.rjust(10) for month in MONTHS), "     Total")
    for seller, values in sorted(by_seller.items(), key=lambda item: -sum(item[1])):
        label = names.get(seller) or "Não informado"
        print(label[:22].ljust(22), " ".join(f"{value:10.2f}" for value in values), f"{sum(values):10.2f}")
    print("TOTAL".ljust(22), " ".join(f"{value:10.2f}" for value in totals), f"{sum(totals):10.2f}")


def resolve_seller(db, seller: str | None, seller_ref: str | None) -> tuple[str, str]:
    if seller_ref:
        person = db["Pessoas"].find_one({"_id": ObjectId(seller_ref)}, {"Nome": 1})
        if not person:
            raise SystemExit(f"Seller reference {seller_ref} not found.")
        return seller_ref, person.get("Nome", seller_ref)
    if not seller:
        raise SystemExit("Use --seller or --seller-ref.")
    exact = list(db["Pessoas"].find(
        {"Nome": {"$regex": f"^{seller}$", "$options": "i"}}, {"Nome": 1, "Ativo": 1}))
    candidates = exact or list(db["Pessoas"].find(
        {"Nome": {"$regex": seller, "$options": "i"}}, {"Nome": 1, "Ativo": 1}).limit(20))
    if not candidates:
        raise SystemExit(f"Seller '{seller}' not found.")
    if len(candidates) > 1:
        scored = sorted(
            ((db["Movimentacoes"].count_documents({"ItensBase.VendedorReferencia": person["_id"]}), person)
             for person in candidates), key=lambda pair: -pair[0])
        if scored[0][0] == scored[1][0]:
            listing = ", ".join(f"{person['_id']} ({person.get('Nome')})" for _, person in scored)
            raise SystemExit(f"Seller '{seller}' is ambiguous: {listing}. Use --seller-ref.")
        candidates = [scored[0][1]]
    person = candidates[0]
    return str(person["_id"]), person.get("Nome", seller)


def shared_documents(db, day: date, seller: str | None, seller_ref: str | None,
                     company: str | None = None) -> None:
    ref, name = resolve_seller(db, seller, seller_ref)
    start = datetime.combine(day, datetime.min.time())
    end = start + timedelta(days=1)
    match = {
        "DataHoraEmissao": {"$gte": start, "$lt": end},
        "_t": "NotaFiscalConsumidorEletronica",
        "Situacao.Descricao": "Concluído",
    }
    if company:
        match["Empresa.Nome"] = company
    docs = list(db["Movimentacoes"].find(match, SHARED_PROJECTION))

    all_total = seller_total = 0.0
    print(f"\n{day} — documents containing {name} ({ref})")
    for doc in docs:
        items = [item for item in doc.get("ItensBase", []) if not item.get("Cancelado")]
        mine = [item for item in items if str(item.get("VendedorReferencia") or "") == ref]
        if not mine:
            continue
        doc_total = round(sum(item_total(item) for item in items), 2)
        my_total = round(sum(item_total(item) for item in mine), 2)
        all_total += doc_total
        seller_total += my_total
        difference = round(doc_total - my_total, 2)
        if difference > 0.01 or len({str(item.get("VendedorReferencia") or "") for item in items}) > 1:
            payments = doc.get("ItensPagamentos") or doc.get("Pagamentos") or []
            paid = round(sum(float(payment.get("Valor") or 0) for payment in payments), 2)
            print(f"  {doc.get('Serie')}/{doc.get('Numero')}  {doc.get('DataHoraEmissao')}  "
                  f"documento {doc_total:.2f}  {name} {my_total:.2f}  outros {difference:.2f}  pago {paid:.2f}")
    print(f"  Total of documents containing {name}: {all_total:.2f}; {name}: {seller_total:.2f}; "
          f"difference: {all_total - seller_total:.2f}")


def main() -> None:
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--year", type=int, help="Year for the monthly report")
    parser.add_argument("--company", help="Empresa.Nome for the monthly report (optional; all companies when omitted)")
    parser.add_argument("--date", type=date.fromisoformat, help="Day for the shared-document check (YYYY-MM-DD)")
    parser.add_argument("--seller", help="Seller name (substring) for the shared-document check")
    parser.add_argument("--seller-ref", help="Seller ObjectId, when the name is ambiguous")
    parser.add_argument("--explain", action="store_true",
                        help="Explain each parameter, the report bases and the output columns, then exit")
    args = parser.parse_args()
    if args.explain:
        print(EXPLAIN)
        return
    client = app.mongo_client()
    db = client[app.MONGO_DATABASE]
    if args.year:
        monthly_by_seller(db, args.year, args.company)
    if args.date and (args.seller or args.seller_ref):
        shared_documents(db, args.date, args.seller, args.seller_ref, args.company)
    if not args.year and not (args.date and (args.seller or args.seller_ref)):
        parser.error("Use --year and/or --date with --seller/--seller-ref.")


if __name__ == "__main__":
    main()
