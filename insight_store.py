"""Portable local application state. No writes to DigiSat databases."""
from __future__ import annotations

import json
import os
from pathlib import Path
import secrets
import sqlite3
import sys
import threading
from contextlib import contextmanager, closing
from datetime import datetime, timezone

_lock = threading.RLock()


def runtime_dir() -> Path:
    return Path(sys.executable).resolve().parent if getattr(sys, "frozen", False) else Path(__file__).resolve().parent


def data_dir() -> Path:
    path = Path(os.environ.get("INSIGHT_DATA_DIR", str(runtime_dir() / "data"))).resolve()
    path.mkdir(parents=True, exist_ok=True)
    return path


def now() -> str:
    return datetime.now(timezone.utc).isoformat(timespec="seconds")


def identifier() -> str:
    return secrets.token_urlsafe(24)


@contextmanager
def connect():
    connection = sqlite3.connect(data_dir() / "insight.sqlite3", timeout=15)
    connection.row_factory = sqlite3.Row
    connection.execute("PRAGMA foreign_keys=ON")
    connection.executescript("""
    CREATE TABLE IF NOT EXISTS conversations(id TEXT PRIMARY KEY, body TEXT NOT NULL, updated TEXT NOT NULL);
    CREATE TABLE IF NOT EXISTS runs(id TEXT PRIMARY KEY, plan TEXT NOT NULL, result TEXT NOT NULL, created TEXT NOT NULL);
    CREATE TABLE IF NOT EXISTS saved(id TEXT PRIMARY KEY, title TEXT NOT NULL, run_id TEXT NOT NULL REFERENCES runs(id), created TEXT NOT NULL);
    CREATE TABLE IF NOT EXISTS schedules(id TEXT PRIMARY KEY, body TEXT NOT NULL, next_run REAL NOT NULL, enabled INTEGER NOT NULL);
    CREATE TABLE IF NOT EXISTS alerts(id TEXT PRIMARY KEY, body TEXT NOT NULL, created TEXT NOT NULL, seen INTEGER NOT NULL DEFAULT 0);
    CREATE TABLE IF NOT EXISTS jobs(id TEXT PRIMARY KEY, body TEXT NOT NULL, updated TEXT NOT NULL);
    CREATE TABLE IF NOT EXISTS events(id INTEGER PRIMARY KEY AUTOINCREMENT, category TEXT NOT NULL, duration_ms REAL, detail TEXT NOT NULL, created TEXT NOT NULL);
    CREATE TABLE IF NOT EXISTS editor_changes(id TEXT PRIMARY KEY, body TEXT NOT NULL, created TEXT NOT NULL);
    CREATE TABLE IF NOT EXISTS editor_queries(id TEXT PRIMARY KEY, body TEXT NOT NULL, updated TEXT NOT NULL);
    CREATE TABLE IF NOT EXISTS bi_models(id TEXT PRIMARY KEY, body TEXT NOT NULL, updated TEXT NOT NULL);
    CREATE TABLE IF NOT EXISTS bi_views(id TEXT PRIMARY KEY, body TEXT NOT NULL, updated TEXT NOT NULL);
    """)
    try:
        with connection:
            yield connection
    finally:
        connection.close()


def read(sql: str, args=()):
    with _lock, connect() as db:
        return [dict(row) for row in db.execute(sql, args).fetchall()]


def write(sql: str, args=()):
    with _lock, connect() as db:
        db.execute(sql, args)


def dump(value) -> str:
    return json.dumps(value, ensure_ascii=False, default=str, allow_nan=False)


def get_conversation(key: str | None) -> tuple[str, dict]:
    if key:
        rows = read("SELECT body FROM conversations WHERE id=?", (key,))
        if rows:
            return key, json.loads(rows[0]["body"])
    return identifier(), {"plan": None, "pending": None, "messages": []}


def put_conversation(key: str, body: dict):
    body["messages"] = body.get("messages", [])[-40:]
    write("INSERT OR REPLACE INTO conversations VALUES(?,?,?)", (key, dump(body), now()))


def save_run(plan: dict, result: dict) -> str:
    key = identifier()
    write("INSERT INTO runs VALUES(?,?,?,?)", (key, dump(plan), dump(result), now()))
    return key


def get_run(key: str) -> dict | None:
    rows = read("SELECT * FROM runs WHERE id=?", (key,))
    if not rows:
        return None
    row = rows[0]
    return {"id": key, "plan": json.loads(row["plan"]), "result": json.loads(row["result"]), "created": row["created"]}


def event(category: str, detail: str, duration_ms: float | None = None):
    write("INSERT INTO events(category,duration_ms,detail,created) VALUES(?,?,?,?)",
          (category, duration_ms, detail[:400], now()))


def backup_state() -> Path:
    path = data_dir() / "backups"
    path.mkdir(exist_ok=True)
    target = path / f"insight-{datetime.now():%Y%m%d-%H%M%S}-{secrets.token_hex(3)}.sqlite3"
    with _lock, connect() as db, closing(sqlite3.connect(target)) as destination:
        db.backup(destination)
    return target
