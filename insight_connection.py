"""Connection setup and privilege verification without issuing database writes."""
from __future__ import annotations

import os
from pathlib import Path
import re
import threading
import secrets
from time import perf_counter, monotonic
from pydantic import BaseModel, Field
from fastapi import HTTPException
from insight_store import runtime_dir, event

READ_ACTIONS = {"find", "listCollections", "listIndexes", "collStats", "dbStats", "indexStats", "planCacheRead",
                "killCursors", "changeStream", "listDatabases", "dbHash", "connectionStatus", "getMore"}
_security_cache = {}
_config_lock = threading.Lock()


def privileges(client) -> dict:
    try:
        info = client.admin.command("connectionStatus", showPrivileges=True).get("authInfo", {})
        grants = info.get("authenticatedUserPrivileges", [])
        actions = {action for grant in grants for action in grant.get("actions", [])}
        authenticated = bool(info.get("authenticatedUsers"))
        unsafe = sorted(actions - READ_ACTIONS)
        verified = authenticated and bool(actions) and not unsafe
        return {"verified_read_only": verified, "authenticated": authenticated,
                "has_broader_privileges": bool(unsafe),
                "message": "Permissões de consulta verificadas no MongoDB." if verified else
                "A conta possui permissões mais amplas que leitura ou não foi possível comprovar as permissões. Solicite ao administrador uma conta dedicada com role read."}
    except Exception:
        return {"verified_read_only": False, "authenticated": None, "has_broader_privileges": None,
                "message": "Não foi possível verificar as permissões. O administrador deve confirmar a role read."}


def enforce(client):
    if os.environ.get("INSIGHT_REQUIRE_READ_ONLY", "0").lower() not in {"1", "true", "yes"}:
        return
    import app
    key = (app.MONGO_URI, app.MONGO_USERNAME, app.MONGO_DATABASE)
    cached = _security_cache.get(key)
    if cached and monotonic() - cached[0] < 30:
        verified = cached[1]
    else:
        verified = privileges(client)["verified_read_only"]
        _security_cache[key] = (monotonic(), verified)
    if not verified:
        client.close()
        raise HTTPException(503, "A conexão exige uma conta comprovadamente somente leitura. Revise a conta no assistente de conexão.")


class ConnectionSettings(BaseModel):
    host: str = Field(default="127.0.0.1", min_length=1, max_length=200, pattern=r"^[a-zA-Z0-9_.-]+$")
    port: int = Field(default=12220, ge=1, le=65535)
    database: str = Field(min_length=1, max_length=120, pattern=r"^[^/\\.\s\"$\x00]+$")
    username: str = Field(default="", max_length=120, pattern=r"^[^\r\n\x00]*$")
    auth_source: str = Field(default="admin", min_length=1, max_length=120, pattern=r"^[\w-]+$")
    password: str | None = Field(default=None, max_length=1000, pattern=r"^[^\r\n\x00]*$")
    require_read_only: bool = False


def current_settings() -> dict:
    import app
    from pymongo.uri_parser import parse_uri
    try:
        uri = parse_uri(app.MONGO_URI)
        host, port = uri["nodelist"][0]
    except Exception:
        host, port = "127.0.0.1", 12220
    return {"host": host, "port": port, "database": app.MONGO_DATABASE, "username": app.MONGO_USERNAME,
            "auth_source": app.MONGO_AUTH_SOURCE, "password_configured": bool(app.MONGO_PASSWORD_FILE),
            "require_read_only": os.environ.get("INSIGHT_REQUIRE_READ_ONLY", "0").lower() in {"1", "true", "yes"}}


def password_from_current() -> str | None:
    import app
    if not app.MONGO_PASSWORD_FILE:
        return None
    path = Path(app.MONGO_PASSWORD_FILE)
    if not path.is_absolute():
        path = runtime_dir() / path
    try:
        lines = [line.strip() for line in path.read_text(encoding="utf-8-sig").splitlines() if line.strip()]
        return lines[-1] if len(lines) in {1, 2} else None
    except OSError:
        return None


def test_connection(settings: ConnectionSettings) -> dict:
    from pymongo import MongoClient
    from pymongo.errors import PyMongoError
    options = {"serverSelectionTimeoutMS": 3000, "connectTimeoutMS": 3000, "appname": "DigiSatInsight-Setup"}
    password = settings.password if settings.password is not None else password_from_current()
    if settings.username:
        options.update(username=settings.username, password=password or "", authSource=settings.auth_source)
    started = perf_counter()
    try:
        with MongoClient(f"mongodb://{settings.host}:{settings.port}/?directConnection=true", **options) as client:
            client.admin.command("ping")
            # A successful ping alone does not establish access to the chosen database.
            names = client[settings.database].list_collection_names()
            if not names:
                raise HTTPException(422, "O banco está vazio ou não foi encontrado. Confira o nome do banco antes de salvar a conexão.")
            security = privileges(client)
            if settings.require_read_only and not security["verified_read_only"]:
                raise HTTPException(422, security["message"])
            return {"connected": True, "collections": len(names), "latency_ms": round((perf_counter() - started) * 1000, 1),
                    "security": security}
    except PyMongoError as exc:
        raise HTTPException(422, f"Conexão não aprovada: {exc.__class__.__name__}. Confira endereço, porta, banco e credenciais.") from None


def save_connection(settings: ConnectionSettings) -> dict:
    result = test_connection(settings)
    import app
    from dotenv import dotenv_values
    with _config_lock:
        folder = runtime_dir()
        path = folder / ("config.env" if getattr(__import__("sys"), "frozen", False) else ".env")
        content = path.read_text(encoding="utf-8-sig") if path.exists() else ""
        uri = f"mongodb://{settings.host}:{settings.port}/?directConnection=true&readPreference=secondaryPreferred"
        password_path = folder / ("mongo-" + secrets.token_hex(8) + ".local.key")
        if settings.password is not None:
            temporary = password_path.with_suffix(".tmp")
            temporary.write_text(settings.password + "\n", encoding="utf-8")
            temporary.replace(password_path)
            selected_password = password_path.name
        else:
            selected_password = app.MONGO_PASSWORD_FILE
        updates = {"MONGO_URI": uri, "MONGO_DATABASE": settings.database, "MONGO_USERNAME": settings.username,
                   "MONGO_AUTH_SOURCE": settings.auth_source, "MONGO_PASSWORD_FILE": selected_password,
                   "DIGISAT_MODE": "live", "INSIGHT_REQUIRE_READ_ONLY": "1" if settings.require_read_only else "0"}
        for key, value in updates.items():
            # Quote values using dotenv syntax; never interpolate executable shell text.
            escaped = str(value).replace("\\", "\\\\").replace("'", "\\'")
            line = f"{key}='{escaped}'"
            expression = re.compile(r"^" + re.escape(key) + r"=.*$", re.M)
            content = expression.sub(lambda _: line, content) if expression.search(content) else content.rstrip() + "\n" + line + "\n"
        if path.exists():
            backup = folder / (path.name + ".previous")
            backup.write_bytes(path.read_bytes())
        temporary = path.with_name(path.name + ".tmp")
        temporary.write_text(content, encoding="utf-8")
        # Validate the serialized configuration before switching to it.
        parsed = dotenv_values(temporary)
        if any(parsed.get(key) != str(value) for key, value in updates.items()):
            raise HTTPException(500, "Não foi possível serializar a configuração. A configuração anterior foi preservada.")
        temporary.replace(path)
        app.MONGO_URI, app.MONGO_DATABASE, app.MONGO_USERNAME = uri, settings.database, settings.username
        app.MONGO_AUTH_SOURCE = settings.auth_source
        app.MONGO_PASSWORD_FILE = str(folder / selected_password) if selected_password and not Path(selected_password).is_absolute() else selected_password
        app.MODE = "live"
        os.environ["INSIGHT_REQUIRE_READ_ONLY"] = updates["INSIGHT_REQUIRE_READ_ONLY"]
        _security_cache.clear()
        app._COMPANY_CACHE.clear()
    event("configuration", "Connection tested and saved; password values omitted.")
    return {**result, "saved": True, "message": "Conexão testada e aplicada. A configuração anterior foi preservada."}


def diagnostics() -> dict:
    import app
    from insight_store import read, data_dir
    started = perf_counter()
    result = {"version": app.app.version, "database": app.MONGO_DATABASE, "data_directory_writable": False,
              "connected": False, "security": None}
    try:
        with app.mongo_client() as client:
            result["connected"] = True
            result["security"] = privileges(client)
            coll = client[app.MONGO_DATABASE].Movimentacoes
            result["indexes"] = [i["name"] for i in coll.list_indexes()]
            for direction, label in [(1, "first_document"), (-1, "last_document")]:
                rows = list(coll.find({"DataHoraEmissao": {"$exists": True}}, {"DataHoraEmissao": 1, "_id": 0})
                            .sort("DataHoraEmissao", direction).limit(1).max_time_ms(5000))
                result[label] = str(rows[0]["DataHoraEmissao"]) if rows else None
    except HTTPException as exc:
        result["message"] = str(exc.detail)
    except app.PyMongoError as exc:
        result["message"] = f"Diagnóstico parcial: {exc.__class__.__name__}."
    result["latency_ms"] = round((perf_counter() - started) * 1000, 1)
    try:
        result["recent_queries"] = read("SELECT category,duration_ms,detail,created FROM events ORDER BY id DESC LIMIT 20")
        result["data_directory_writable"] = data_dir().exists()
    except OSError:
        result["data_directory_writable"] = False
    result["performance_targets"] = {"interactive_seconds": 5, "investigation_seconds": 20,
                                     "note": "Metas, não garantias. Meça com volume e índices reais; o app não cria índices no banco."}
    return result
