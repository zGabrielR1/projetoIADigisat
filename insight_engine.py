"""Execute only validated, fixed report plans; attach provenance to every result."""
from __future__ import annotations

from time import perf_counter
from fastapi import HTTPException
from insight_plans import Plan, plan_summary, PROFILES
from insight_store import now, event


def execute_plan(plan: Plan, *, compare: bool = True) -> dict:
    import app
    started = perf_counter()
    if plan.profile == "native_nfce":
        plan.kind, plan.movement_group, plan.only_valid, plan.status_filter = ("quality" if plan.kind == "quality" else "investigation"), "nfce", True, "completed"
    elif plan.profile == "fiscal_valid":
        plan.movement_group, plan.only_valid = "fiscal_sales", True
    data = plan.model_dump(mode="json")
    detailed = plan.kind in {"documents", "investigation", "quality"} or plan.seller_query or plan.profile == "native_nfce" or plan.comparison or plan.status_filter == "completed"
    if detailed:
        req = app.InvestigationRequest(**{**data, "limit": min(plan.page_size, app.MAX_ROWS)})
        result = app.investigation(req)
        if result["investigation"]["seller_reference"]:
            # Keep a stable resolved identity in subsequent follow-ups and saved plans.
            plan.seller_query = result["investigation"]["seller_reference"]
        result["totals"] = result["investigation"]["totals"]
        if plan.kind == "quality":
            counts = result["quality"]["counts"]
            counts.pop("_id", None)
            labels = {"mismatch": "itens e pagamentos divergentes", "unpaid": "sem pagamento registrado",
                      "unassigned": "com itens sem vendedor", "shared": "compartilhadas entre vendedores", "negative": "com total negativo"}
            result["investigation"]["findings"].insert(0, "Qualidade dos dados: " + "; ".join(
                f"{counts.get(key, 0)} vendas {label}" for key, label in labels.items()) + ".")
            result["investigation"]["findings"].append(result["quality"]["note"])
        result["view_kind"] = "investigation"
    else:
        if plan.search or plan.offset:
            raise HTTPException(422, "Busca e paginação de documentos estão disponíveis em Documentos e Investigação. Escolha esse relatório.")
        # Aggregate views preserve the existing validated seven-report contract.
        req = app.ReportRequest(**{**data, "limit": min(app.MAX_ROWS, max(plan.page_size, 100))})
        result = app.run_report(req)
        rows = result.get("rows", [])
        amount = sum(float(r.get("total") or 0) for r in rows)
        paid = result.get("payments_total")
        result["totals"] = {"Itens": round(amount, 2), "Vendedor": round(amount, 2), "Outros": 0,
                            "Pagamentos": paid, "Diferenca": round(amount - paid, 2) if paid is not None else None}
        if plan.kind in {"top_products", "companies", "daily", "ticket"} and len(rows) >= req.limit:
            result.setdefault("warnings", []).append("A visão agregada atingiu o limite de linhas. Reduza o período; para exportar todas as vendas use Documentos ou Investigação.")
        result["view_kind"] = plan.kind
        result["pagination"] = None
    if compare and plan.comparison:
        other_data = plan.model_dump(mode="json")
        other_data.update(plan.comparison.model_dump(mode="json"))
        other_data.update(comparison=None, offset=0, kind="investigation")
        other_data.pop("label", None)
        other_plan = Plan(**other_data)
        try:
            other = execute_plan(other_plan, compare=False)
        except HTTPException as exc:
            exc.plan_field = "comparison_seller"
            raise
        deltas = {}
        for key in ("Itens", "Vendedor", "Outros", "Pagamentos", "Diferenca"):
            a = result["totals"].get(key)
            b = other["totals"].get(key)
            if a is not None and b is not None:
                deltas[key] = {"base": a, "comparison": b, "difference": round(b - a, 2),
                               "percent": round(100 * (b - a) / abs(a), 2) if abs(a) > 0.001 else None}
        result["comparison"] = {"plan": other_plan.model_dump(mode="json"), "result": other,
                                "metrics": deltas, "direction": "Comparação menos base; percentual sobre o valor absoluto da base."}
    summary = plan_summary(plan)
    if detailed and result["investigation"].get("seller"):
        summary[2] = "Vendedor: " + result["investigation"]["seller"]
    display_kind = "documents" if detailed else plan.kind
    result["interpretation"] = {"kind": display_kind, "kind_label": "Qualidade dos dados" if plan.kind == "quality" else "Investigação de vendas" if detailed else app.KIND_LABELS[display_kind],
                                "intent": plan.kind, "operation_allowlisted": True, "method": "plano conversacional validado",
                                "period_source": "pergunta", "company": plan.company_value, "summary": summary}
    result["provenance"] = {"generated_at": now(), "database": app.MONGO_DATABASE, "version": app.app.version,
                            "profile": PROFILES[plan.profile], "duration_ms": round((perf_counter() - started) * 1000, 1),
                            "source": "live", "read_only": True}
    result["plan"] = plan.model_dump(mode="json")
    event("query", f"{plan.kind}; profile={plan.profile}; documents={result.get('documents', 0)}", result["provenance"]["duration_ms"])
    return result


def all_results(plan: Plan):
    """Yield complete, stably ordered document pages, without changing global totals."""
    data = plan.model_dump(mode="json")
    # "Complete" means underlying sales documents, not a truncated ranking.
    data.update(offset=0, page_size=500, kind="investigation")
    first = execute_plan(Plan(**data))
    yield first
    pagination = first.get("pagination")
    if not pagination:
        return
    total = pagination["total"]
    if total > 1000000:
        raise HTTPException(422, "A consulta excede um milhão de documentos. Divida o período para exportar.")
    offset = len(first.get("rows", []))
    while offset < total:
        data["offset"] = offset
        page = execute_plan(Plan(**data), compare=False)
        if page.get("documents") != total:
            raise HTTPException(409, "A base mudou durante a exportação. Execute novamente para evitar um arquivo incompleto.")
        rows = page.get("rows", [])
        if not rows:
            raise HTTPException(409, "A consulta mudou durante a exportação. Execute novamente.")
        yield page
        offset += len(rows)
