from __future__ import annotations

import json
import os
from pathlib import Path
from fastapi import APIRouter, HTTPException
from fastapi.responses import FileResponse
from pydantic import BaseModel, Field, field_validator
import insight_store as store
from insight_plans import Plan, PROFILES
from insight_conversation import ConversationRequest, reply
from insight_exports import ExportRequest, start_export, get_job
from insight_connection import ConnectionSettings, current_settings, test_connection, inspect_connection, save_connection, diagnostics
from insight_schedule import ScheduleRequest
import insight_schedule as schedules
from insight_stock import ExpiryRequest, companies as stock_companies, query_expiry

router = APIRouter(prefix="/api/workspace")


@router.get("/stock/companies")
def expiry_companies():
    return stock_companies()


@router.post("/stock/expiry")
def expiry(req: ExpiryRequest):
    return query_expiry(req)


@router.post("/conversation")
def conversation(req: ConversationRequest):
    return reply(req)


@router.get("/conversations/{key}")
def get_conversation(key: str):
    found, session = store.get_conversation(key)
    if found != key:
        raise HTTPException(404, "Conversa não encontrada. Inicie uma nova conversa.")
    return {"conversation_id": key, **session}


@router.get("/profiles")
def profiles():
    return {"profiles": PROFILES}


class SavedTitle(BaseModel):
    title: str = Field(min_length=1, max_length=120)

    @field_validator('title')
    @classmethod
    def clean_title(cls, value: str):
        value = value.strip()
        if not value:
            raise ValueError('Informe um título.')
        return value


class SavedRequest(SavedTitle):
    run_id: str = Field(min_length=10, max_length=100)


@router.post("/saved")
def save_investigation(req: SavedRequest):
    if not store.get_run(req.run_id):
        raise HTTPException(404, "Resultado não encontrado.")
    key = store.identifier()
    store.write("INSERT INTO saved VALUES(?,?,?,?)", (key, req.title, req.run_id, store.now()))
    return {"id": key, "title": req.title, "run_id": req.run_id}


@router.get("/saved")
def saved_list():
    return {"saved": store.read("SELECT * FROM saved ORDER BY created DESC LIMIT 200")}


@router.patch("/saved/{key}")
def rename_saved(key: str, req: SavedTitle):
    with store._lock, store.connect() as db:
        changed = db.execute("UPDATE saved SET title=? WHERE id=?", (req.title, key))
        if not changed.rowcount:
            raise HTTPException(404, "Investigação salva não encontrada.")
    return {"id": key, "title": req.title}


@router.delete("/saved/{key}")
def remove_saved(key: str):
    store.write("DELETE FROM saved WHERE id=?", (key,))
    return {"removed": True}


@router.get("/runs/{key}")
def saved_run(key: str):
    run = store.get_run(key)
    if not run:
        raise HTTPException(404, "Resultado não encontrado.")
    run["result"].setdefault("provenance", {})["source"] = "saved_snapshot"
    return run


class ResumeRequest(BaseModel):
    run_id: str = Field(min_length=10, max_length=100)


@router.post("/conversation/resume")
def resume(req: ResumeRequest):
    run = saved_run(req.run_id)
    if run['plan'].get('module') == 'stock_expiry':
        raise HTTPException(422, 'Abra a consulta pelo módulo Validade de lotes.')
    key = store.identifier()
    messages = [{"role": "assistant", "text": "Cópia salva aberta. Os números não foram recalculados. Uma nova pergunta ou Executar consulta a base atual.", "at": store.now(), "run_id": run["id"]}]
    store.put_conversation(key, {"plan": run["plan"], "pending": None, "messages": messages})
    return {"status": "result", "conversation_id": key, "run_id": run["id"], "plan": run["plan"], "result": run["result"], "messages": messages}


@router.post("/exports")
def export(req: ExportRequest):
    return start_export(req)


@router.get("/exports/{key}")
def export_status(key: str):
    job = get_job(key)
    if not job:
        raise HTTPException(404, "Exportação não encontrada.")
    return {k: v for k, v in job.items() if k != "filename"}


@router.get("/exports/{key}/download")
def download(key: str):
    job = get_job(key)
    if not job or job["status"] != "succeeded":
        raise HTTPException(404, "A exportação não está disponível.")
    folder = (store.data_dir() / "exports").resolve()
    path = (folder / job["filename"]).resolve()
    if path.parent != folder or not path.is_file():
        raise HTTPException(404, "Arquivo não encontrado. Gere a exportação novamente.")
    types = {"pdf": "application/pdf", "xlsx": "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "csv": "text/csv; charset=utf-8"}
    return FileResponse(path, media_type=types[job["format"]], filename=f"DigiSat-evidencias-{key[:8]}.{job['format']}")


@router.post("/exports/{key}/cancel")
def cancel_export(key: str):
    from insight_exports import cancel_job
    cancel_job(key)
    return {"cancel_requested": True}


@router.get("/connection")
def settings():
    return current_settings()


@router.post("/connection/test")
def check_connection(req: ConnectionSettings):
    return test_connection(req)


@router.post('/connection/inspect')
def inspect_setup(req: ConnectionSettings):
    return inspect_connection(req)


@router.post("/connection/save")
def apply_connection(req: ConnectionSettings):
    return save_connection(req)


@router.get("/diagnostics")
def health():
    return diagnostics()


@router.post("/state/backup")
def backup_state():
    path = store.backup_state()
    return FileResponse(path, media_type="application/vnd.sqlite3", filename=path.name)


@router.post("/schedules")
def create_schedule(req: ScheduleRequest):
    return schedules.create(req)


@router.get("/schedules")
def schedule_list():
    return {"schedules": schedules.listing(), "note": "Executa enquanto o aplicativo estiver aberto. Sem envios externos."}


class Toggle(BaseModel):
    enabled: bool


@router.patch("/schedules/{key}")
def toggle_schedule(key: str, req: Toggle):
    store.write("UPDATE schedules SET enabled=? WHERE id=?", (int(req.enabled), key))
    return {"enabled": req.enabled}


@router.delete("/schedules/{key}")
def remove_schedule(key: str):
    store.write("DELETE FROM schedules WHERE id=?", (key,))
    return {"removed": True}


@router.post("/schedules/{key}/run")
def run_schedule(key: str):
    return schedules.run_once(key)


@router.get("/alerts")
def alerts():
    return {"alerts": [{**row, "body": json.loads(row["body"])} for row in store.read("SELECT * FROM alerts ORDER BY created DESC LIMIT 100")]}


@router.post("/alerts/{key}/seen")
def mark_seen(key: str):
    store.write("UPDATE alerts SET seen=1 WHERE id=?", (key,))
    return {"seen": True}


def startup():
    # Persisted export jobs interrupted by a previous shutdown cannot remain "running" forever.
    for row in store.read("SELECT id,body FROM jobs"):
        body = json.loads(row["body"])
        if body.get("status") in {"queued", "running"} and not process_alive(body.get("owner_pid")):
            body.update(status="failed", error="A aplicação foi encerrada durante a exportação. Gere o arquivo novamente.")
            store.write("UPDATE jobs SET body=?,updated=? WHERE id=?", (store.dump(body), store.now(), row["id"]))
    schedules.start()


def process_alive(pid):
    if not pid:
        return False
    if os.name == "nt":
        import ctypes
        from ctypes import wintypes
        kernel = ctypes.WinDLL("kernel32", use_last_error=True)
        kernel.OpenProcess.argtypes = [wintypes.DWORD, wintypes.BOOL, wintypes.DWORD]
        kernel.OpenProcess.restype = wintypes.HANDLE
        kernel.GetExitCodeProcess.argtypes = [wintypes.HANDLE, ctypes.POINTER(wintypes.DWORD)]
        kernel.GetExitCodeProcess.restype = wintypes.BOOL
        kernel.CloseHandle.argtypes = [wintypes.HANDLE]
        kernel.CloseHandle.restype = wintypes.BOOL
        handle = kernel.OpenProcess(0x1000, False, int(pid))
        if not handle:
            return False
        try:
            code = wintypes.DWORD()
            return bool(kernel.GetExitCodeProcess(handle, ctypes.byref(code))) and code.value == 259
        finally:
            kernel.CloseHandle(handle)
    try:
        os.kill(int(pid), 0)
        return True
    except OSError:
        return False


def shutdown():
    schedules.stop()
