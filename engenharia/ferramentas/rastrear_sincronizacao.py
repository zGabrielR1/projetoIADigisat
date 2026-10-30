"""Static dnSpy extraction of selected installed SuiteG6 types; no target execution."""
import hashlib
import argparse
import json
import subprocess
from pathlib import Path

ROOT = Path(__file__).resolve().parents[2]
OUT = ROOT / 'engenharia' / 'sincronizacao-2026-10-30' / 'fontes'
DNSPY = Path('D:/Downloads/stuff/winUtils/dnSpy-net-win64/dnSpy.Console.exe')
SUITE = Path('C:/Digisat/SuiteG6')
TARGETS = [
    ('Servidor/DigisatServer.Distributed.Akka.Main.dll', 'DigisatServer.Distributed.Akka.Main.Actors.SincronizacaoServerActor'),
    ('Servidor/DigisatServer.Distributed.Akka.Main.dll', 'DigisatServer.Distributed.Akka.Main.Actors.SincronizacaoClient'),
    ('Sincronizador/SincronizadorG6.exe', 'Sincronizador.SincronizacaoClientActor'),
    ('Sincronizador/SincronizadorG6.exe', 'Sincronizador.SincronizacaoClientActorSystem'),
    ('Sincronizador/SincronizadorG6.exe', 'Sincronizador.SincronizacaoService'),
    ('Sistema/DigisatServer.Infrastructure.Data.Core.dll', 'DigisatServer.Infrastructure.Data.Core.OpLogTail'),
    ('Sistema/DigisatServer.Infrastructure.Data.Core.dll', 'DigisatServer.Infrastructure.Data.Core.CollectionReplicator'),
    ('Sistema/DigisatServer.Infrastructure.Data.Core.dll', 'DigisatServer.Infrastructure.Data.Core.RepositorySincronizacao'),
    ('Sistema/DigisatServer.Application.Main.dll', 'DigisatServer.Application.Main.AuxiliaresModulo.Services.SincronizacaoAppService'),
    ('Sistema/DigisatServer.Application.Main.dll', 'DigisatServer.Application.Main.AuxiliaresModulo.Services.MovimentacaoSaidaRelatorioAppService'),
]

def sha(path):
    return hashlib.sha256(path.read_bytes()).hexdigest()

def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('--only', help='Extract only types containing this exact fragment.')
    args = parser.parse_args()
    OUT.mkdir(parents=True, exist_ok=True)
    records = []
    for relative, typename in TARGETS:
        if args.only and args.only not in typename:
            continue
        artifact = SUITE / relative
        source = OUT / (typename.rsplit('.', 1)[-1] + '.cs')
        before = sha(artifact)
        command = [str(DNSPY), '--no-color', '--no-resources', '--no-baml', '-t', typename, str(artifact)]
        run = subprocess.run(command, capture_output=True, timeout=180)
        if run.returncode or b'class ' not in run.stdout:
            raise RuntimeError(f'dnSpy extraction failed: {typename}')
        if sha(artifact) != before:
            raise RuntimeError('Artifact changed during static inspection.')
        source.write_bytes(run.stdout)
        records.append(dict(artifact=str(artifact), artifact_sha256=before, type=typename,
                            source=str(source.relative_to(ROOT)), source_sha256=sha(source)))
        print(typename, flush=True)
    manifest = dict(target_executed=False, decompiler=str(DNSPY), decompiler_sha256=sha(DNSPY),
                    limitations=['Decompiler reconstruction; use REA metadata/CIL evidence for canonical identity.',
                                 'Local installed build is not proof of the customer build or runtime configuration.'],
                    types=records)
    manifest_path = OUT.parent / 'manifesto-fontes.json'
    if args.only and manifest_path.exists():
        old = json.loads(manifest_path.read_text(encoding='utf-8'))
        replaced = {row['type'] for row in records}
        manifest['types'] = [row for row in old['types'] if row['type'] not in replaced] + records
    manifest_path.write_text(json.dumps(manifest, indent=2), encoding='utf-8')

if __name__ == '__main__':
    main()
