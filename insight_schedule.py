"""Opt-in local scheduling; alerts stay inside this application."""
from __future__ import annotations

from datetime import date, timedelta
import hashlib
import json
import os
import threading
import time
from pydantic import BaseModel, Field
from typing import Literal
from fastapi import HTTPException
import insight_store as store
from insight_plans import Plan
from insight_engine import execute_plan

_stop = threading.Event()
_thread = None
_running = set()
_guard = threading.Lock()


class ScheduleRequest(BaseModel):
    run_id: str = Field(min_length=10, max_length=100)
    title: str = Field(min_length=1, max_length=120)
    interval_minutes: int = Field(default=60, ge=1, le=10080)
    period_policy: Literal["fixed", "today", "yesterday"] = "yesterday"
    metric: Literal["Diferenca", "Outros", "quality_issues"] = "Diferenca"
    threshold: float = Field(default=0.01, ge=0, le=1e12, allow_inf_nan=False)
    enabled: bool = False


def create(req):
    run = store.get_run(req.run_id)
    if not run:
        raise HTTPException(404, "Consulta não encontrada.")
    plan = Plan(**run["plan"])
    if not (run["result"].get("investigation")):
        raise HTTPException(422, "Crie agendamentos a partir de Documentos, Investigação ou Qualidade dos dados.")
    key = store.identifier()
    body = {**req.model_dump(), "id": key, "plan": plan.model_dump(mode="json"), "last_status": "never",
            "last_run": None, "last_signature": None,
            "note": "Executa apenas enquanto o DigiSat Insight estiver aberto. Alertas aparecem aqui; não são enviados a terceiros."}
    store.write("INSERT INTO schedules VALUES(?,?,?,?)", (key, store.dump(body), time.time() + req.interval_minutes * 60, int(req.enabled)))
    return body


def listing():
    result = []
    for row in store.read("SELECT * FROM schedules ORDER BY rowid DESC LIMIT 100"):
        body = json.loads(row["body"])
        body.update(enabled=bool(row["enabled"]), next_run=row["next_run"])
        result.append(body)
    return result


def run_once(key: str):
    with _guard:
        if key in _running:
            raise HTTPException(409, "Este agendamento já está em execução.")
        _running.add(key)
    try:
        rows = store.read("SELECT * FROM schedules WHERE id=?", (key,))
        if not rows:
            raise HTTPException(404, "Agendamento não encontrado.")
        body = json.loads(rows[0]["body"])
        values = dict(body["plan"])
        if body["period_policy"] in {"today", "yesterday"}:
            day = date.today() - timedelta(days=int(body["period_policy"] == "yesterday"))
            values.update(start=str(day), end=str(day), offset=0)
            if values.get("comparison"):
                duration = (date.fromisoformat(values["comparison"]["end"]) - date.fromisoformat(values["comparison"]["start"])).days
                values["comparison"].update(start=str(day - timedelta(days=duration + 1)), end=str(day - timedelta(days=1)))
        plan = Plan(**values)
        try:
            result = execute_plan(plan)
            run_id = store.save_run(result["plan"], result)
            if body["metric"] == "quality_issues":
                counts = result.get("quality", {}).get("counts", {})
                value = sum(int(counts.get(k) or 0) for k in ("mismatch", "unpaid", "unassigned", "negative"))
            else:
                value = abs(float(result.get("totals", {}).get(body["metric"]) or 0))
            signature = hashlib.sha256(store.dump({"value": value, "documents": result.get("documents"),
                "period": {"start": str(plan.start), "end": str(plan.end)}}).encode()).hexdigest()
            triggered = value > body["threshold"] and signature != body.get("last_signature")
            if triggered:
                alert = {"schedule_id": key, "title": body["title"], "run_id": run_id,
                         "metric": body["metric"], "value": value, "threshold": body["threshold"],
                         "message": "Resultado mudou e ultrapassou o limite configurado. Abra a evidência para conferir."}
                store.write("INSERT INTO alerts(id,body,created) VALUES(?,?,?)", (store.identifier(), store.dump(alert), store.now()))
            body.update(last_status="succeeded", last_run=store.now(), last_run_id=run_id,
                        last_signature=signature, last_value=value, last_error=None)
        except Exception as exc:
            body.update(last_status="failed", last_run=store.now(), last_error=(str(exc.detail) if isinstance(exc, HTTPException) else type(exc).__name__))
        store.write("UPDATE schedules SET body=?,next_run=? WHERE id=?", (store.dump(body), time.time() + body["interval_minutes"] * 60, key))
        return body
    finally:
        with _guard:
            _running.discard(key)


def tick():
    current = time.time()
    for row in store.read("SELECT id,body FROM schedules WHERE enabled=1 AND next_run<=? LIMIT 10", (current,)):
        # Transactional claim prevents duplicate due jobs across two app processes.
        with store.connect() as db:
            body = json.loads(row["body"])
            claimed = db.execute("UPDATE schedules SET next_run=? WHERE id=? AND enabled=1 AND next_run<=?",
                                 (current + max(body["interval_minutes"] * 60, 300), row["id"], current)).rowcount
        if claimed:
            try:
                run_once(row["id"])
            except Exception as exc:
                store.event("scheduler", type(exc).__name__)


def start():
    global _thread
    if os.environ.get("INSIGHT_SCHEDULER", "1") == "0" or (_thread and _thread.is_alive()):
        return
    _stop.clear()
    def loop():
        while not _stop.wait(15):
            try:
                tick()
            except Exception:
                # A read-only/unavailable local directory must not crash the reporting API.
                continue
    _thread = threading.Thread(target=loop, name="insight-scheduler", daemon=True)
    _thread.start()


def stop():
    _stop.set()
