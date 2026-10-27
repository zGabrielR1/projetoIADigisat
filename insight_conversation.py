from __future__ import annotations

from datetime import date
from fastapi import HTTPException
from pydantic import BaseModel, Field, ValidationError
from insight_plans import Plan, Comparison, infer_plan, explicit_period, classify, clarification
from insight_engine import execute_plan
import insight_store as store


class ConversationRequest(BaseModel):
    conversation_id: str | None = Field(default=None, max_length=100)
    message: str = Field(default="", max_length=1500)
    answer: str | None = Field(default=None, max_length=200)
    filters: dict = Field(default_factory=dict)
    action: str = Field(default="message", pattern="^(message|refine|execute|page)$")


def reply(req: ConversationRequest) -> dict:
    key, session = store.get_conversation(req.conversation_id)
    default = {"start": str(date.today()), "end": str(date.today())}
    raw = {**default, **(session.get("plan") or {}), **req.filters}
    try:
        base = Plan(**raw)
    except ValidationError as exc:
        raise HTTPException(422, "; ".join(e["msg"] for e in exc.errors())) from None
    pending = session.get("pending")
    question = req.message.strip()
    if pending and not question and req.answer is None:
        return {"status": "clarification", "conversation_id": key, "plan": pending["plan"],
                "clarification": pending["clarification"], "messages": session["messages"]}
    if pending and (req.answer is not None or question):
        plan = Plan(**pending["plan"])
        previous_plan = plan.model_copy(deep=True)
        answer = req.answer if req.answer is not None else question
        field = pending["clarification"]["field"]
        options = pending["clarification"].get("options", [])
        if field == "kind":
            kind = answer if answer in {"revenue", "investigation", "documents", "quality", "payments", "ticket", "daily", "companies", "top_products"} else classify(answer)
            if not kind:
                return ask_again(key, session, plan, pending["clarification"], answer)
            plan.kind = kind
            hint = {"revenue": "faturamento", "investigation": "investigação", "documents": "documentos", "quality": "qualidade dos dados"}.get(kind, kind)
            plan, prompt = infer_plan(pending["question"] + " " + hint, plan, True)
            if prompt:
                return ask_again(key, session, plan, prompt, answer)
        elif field in {"period", "comparison_period"}:
            try:
                period = explicit_period(answer, date.today())
            except HTTPException:
                period = None
            if not period:
                return ask_again(key, session, plan, clarification(field, "Não identifiquei uma data válida. Informe, por exemplo, 05/10/2026."), answer)
            a, b = period
            if field == "comparison_period":
                plan.comparison = Comparison(start=a, end=b, seller_query=plan.seller_query, company_value=plan.company_value)
            else:
                plan.start, plan.end = a, b
        elif field in {"seller_query", "comparison_seller"}:
            selected = next((o["value"] for o in options if answer in {o["value"], o["label"]}), answer.strip().strip('"'))
            if field == "comparison_seller" and plan.comparison:
                plan.comparison.seller_query = selected
            else:
                plan.seller_query = selected
            refers_to_day = any(word in pending["question"].casefold() for word in ("naquele dia", "mesmo dia"))
            import re
            incomplete_day = bool(re.search(r"\bdia\s+\d{1,2}\b", pending["question"].casefold())) and explicit_period(pending["question"], date.today()) is None
            if field == "seller_query" and ((refers_to_day and (not session.get("plan") or plan.start != plan.end)) or incomplete_day):
                return ask_again(key, session, plan, clarification("period", "E qual é a data completa dessa investigação? Informe dia/mês/ano."), answer)
        try:
            plan = Plan(**plan.model_dump())
        except ValidationError as exc:
            return ask_again(key, session, previous_plan, clarification(field, "; ".join(e["msg"] for e in exc.errors())), answer)
        session["pending"] = None
        question = pending["question"]
        session["messages"].append({"role": "user", "text": answer, "at": store.now()})
    elif req.action == "message":
        if not question:
            raise HTTPException(400, "Digite uma pergunta ou continuação.")
        session["messages"].append({"role": "user", "text": question, "at": store.now()})
        try:
            plan, prompt = infer_plan(question, base, bool(session.get("plan")))
        except HTTPException as exc:
            return ask_again(key, session, base, clarification("period", str(exc.detail)), question)
        except ValidationError as exc:
            raise HTTPException(422, "; ".join(e["msg"] for e in exc.errors())) from None
        if prompt:
            return ask_again(key, session, plan, prompt, question)
    else:
        plan = base
    try:
        result = execute_plan(plan)
    except HTTPException as exc:
        if exc.status_code == 422 and (getattr(exc, "choices", None) or "Vendedor '" in str(exc.detail)):
            field = getattr(exc, "plan_field", "seller_query")
            return ask_again(key, session, plan, clarification(field, str(exc.detail), getattr(exc, "choices", [])), question)
        raise
    session["plan"] = result["plan"]
    session["pending"] = None
    result["question"] = question or next((m["text"] for m in reversed(session["messages"]) if m["role"] == "user"), "Consulta pelos filtros da interface")
    result["context_messages"] = [{"role": m["role"], "text": m["text"]} for m in session["messages"][-10:]]
    run_id = store.save_run(result["plan"], result)
    message = f'{result.get("documents", 0)} documentos encontrados. '
    if result.get("investigation"):
        findings = result["investigation"]["findings"]
        message += findings[1] if len(findings) > 1 else findings[0]
    else:
        message += result["interpretation"]["kind_label"] + " calculado com o plano exibido."
    session["messages"].append({"role": "assistant", "text": message, "at": store.now(), "run_id": run_id})
    store.put_conversation(key, session)
    return {"status": "result", "conversation_id": key, "run_id": run_id, "plan": result["plan"],
            "result": result, "messages": session["messages"], "message": message}


def ask_again(key: str, session: dict, plan: Plan, prompt: dict, question: str) -> dict:
    original = (session.get("pending") or {}).get("question", question)
    session["pending"] = {"plan": plan.model_dump(mode="json"), "question": original, "clarification": prompt}
    session["messages"].append({"role": "assistant", "text": prompt["question"], "at": store.now(), "clarification": True})
    store.put_conversation(key, session)
    return {"status": "clarification", "conversation_id": key, "plan": plan.model_dump(mode="json"),
            "clarification": prompt, "messages": session["messages"]}
