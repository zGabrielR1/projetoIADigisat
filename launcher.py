from pathlib import Path
import argparse
import os
import sys
import threading
import webbrowser
import socket
import urllib.request
import multiprocessing
from insight_version import VERSION

from dotenv import load_dotenv
import uvicorn


def main():
    parser = argparse.ArgumentParser(description="DigiSat Insight")
    parser.add_argument("--version", action="version", version="DigiSat Insight " + VERSION)
    parser.add_argument("--port", type=int, default=8088)
    parser.add_argument("--no-browser", action="store_true")
    args = parser.parse_args()
    if not 1 <= args.port <= 65535:
        parser.error("A porta deve estar entre 1 e 65535.")
    folder = Path(sys.executable).parent if getattr(sys, "frozen", False) else Path(__file__).parent
    load_dotenv(folder / "config.env", override=False)
    password = os.environ.get("MONGO_PASSWORD_FILE", "")
    if password and not Path(password).is_absolute():
        os.environ["MONGO_PASSWORD_FILE"] = str(folder / password)
    url = f"http://127.0.0.1:{args.port}"
    try:
        with socket.socket() as probe:
            probe.bind(("127.0.0.1", args.port))
    except OSError:
        try:
            with urllib.request.urlopen(url, timeout=2) as response:
                ours = b"DigiSat Insight" in response.read(20000)
        except Exception:
            ours = False
        if ours:
            print(f"DigiSat Insight já está aberto em {url}", flush=True)
            if not args.no_browser:
                webbrowser.open(url)
            return
        parser.error(f"A porta {args.port} está ocupada. Use --port com outra porta.")
    from app import app
    print(f"DigiSat Insight: {url}\nMantenha esta janela aberta. Para encerrar, pressione Ctrl+C.", flush=True)
    if not args.no_browser:
        timer = threading.Timer(2, webbrowser.open, args=(url,))
        timer.daemon = True
        timer.start()
    uvicorn.run(app, host="127.0.0.1", port=args.port, loop="asyncio", http="h11", ws="none")


if __name__ == "__main__":
    multiprocessing.freeze_support()
    main()
