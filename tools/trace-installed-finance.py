"""Decompile selected installed finance types without loading SuiteG6 code.

Uses the operator's dnSpy CLI and (optionally) REA's execution-free parser.
Every output is tied to the exact DLL digest, type name, and tool digest.
"""
from __future__ import annotations

import argparse
import hashlib
import json
from pathlib import Path
import re
import subprocess

ROOT = Path(__file__).resolve().parents[1]
DEST = ROOT / 'engenharia/assemblies-descompilados/installed-finance-1.0.3'
SUITE = Path('C:/Digisat/SuiteG6/Sistema')
DNSPY = Path('D:/Downloads/stuff/winUtils/dnSpy-net-win64/dnSpy.Console.exe')
REA = Path('C:/Users/gabrielrenostro/AppData/Local/npm-cache/_npx/a1722018f28f9df2/node_modules/.bin/rea.cmd')
DOMAIN = 'DigisatServer.Domain.Main.Aggregates.FinanceirosAgg.PagamentosRecebimentos'
TARGETS = [
    ('DigisatServer.Application.Main', 'DigisatServer.Application.Main.AuxiliaresModulo.Services.PagamentoRecebimentoRelatorioAppService'),
    ('DigisatServer.Application.Core', 'DigisatServer.Application.Core.Projections.FinanceirosProjection.PagamentosRecebimentos.PagamentoRecebimentoProjection'),
    ('DigisatServer.Application.Core', 'DigisatServer.Application.Core.Projections.FinanceirosProjection.PagamentosRecebimentos.RecebimentoProjection'),
    ('DigisatServer.Domain.Main', DOMAIN + '.ParcelaSpecification`1'),
    ('DigisatServer.Domain.Main', DOMAIN + '.ParcelaPagamentoSpecifications'),
    ('DigisatServer.Domain.Main', DOMAIN + '.ParcelaRecebimentoSpecifications'),
    ('DigisatServer.Domain.Main', DOMAIN + '.Parcelas.Parcela'),
    ('DigisatServer.Domain.Main', DOMAIN + '.Parcelas.Pagamentos.ParcelaPagamento'),
    ('DigisatServer.Domain.Main', DOMAIN + '.Parcelas.Recebimentos.ParcelaRecebimento'),
    ('DigisatServer.Domain.Main', DOMAIN + '.Parcelas.Recebimentos.ParcelaRecebimentoCartao'),
    ('DigisatServer.Domain.Main', DOMAIN + '.Parcelas.Recebimentos.ParcelaRecebimentoCarteiraDigital'),
    ('DigisatServer.Domain.Main', DOMAIN + '.Juros.JuroSimples'),
    ('DigisatServer.Domain.Main', DOMAIN + '.Juros.JuroComposto'),
    ('DigisatServer.Domain.Main', DOMAIN + '.Multas.Multa'),
    ('DigisatServer.Domain.Main', 'DigisatServer.Domain.Main.Aggregates.CadastrosAgg.Pessoas.PessoaSpecifications'),
    ('DigisatServer.Domain.Main', 'DigisatServer.Domain.Main.Aggregates.CadastrosAgg.Pessoas.Juridicas.Taxa'),
    ('DigisatServer.Domain.Main', 'DigisatServer.Domain.Main.Services.FinanceiroSer.CalculoProRataDieService'),
    ('DigisatServer.Infrastructure.Data.Core', 'DigisatServer.Infrastructure.Data.Core.Mappings.DateTimeSerializerEx'),
    ('DigisatServer.Infrastructure.Data.Main', 'DigisatServer.Infrastructure.Data.Main.Repositories.ParcelaRepositoryRead`1'),
]


def digest(path):
    return hashlib.sha256(path.read_bytes()).hexdigest()


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('--rea', action='store_true')
    parser.add_argument('--reuse', action='store_true', help='Reuse hash-verified completed type traces')
    parser.add_argument('--check', action='store_true', help='Verify saved sources/artifact identities without invoking tools')
    args = parser.parse_args()
    DEST.mkdir(parents=True, exist_ok=True)
    manifest = {'schema_version': 1, 'target_release': '1.0.3-rc.1',
                'tool': {'name': 'dnSpy.Console', 'path': str(DNSPY), 'sha256': digest(DNSPY)},
                'limitations': ['Static decompilation only; target code was not executed.',
                                'dnSpy source reconstruction is separate from canonical REA metadata inspection.',
                                'Runtime and native-report output equivalence remain unverified.'], 'types': []}
    manifest_path = ROOT / 'docs/finance-artifacts-1.0.3-rc.1.json'
    old = json.loads(manifest_path.read_text(encoding='utf-8')) if (args.reuse or args.check) and manifest_path.exists() else {}
    previous = {t['type']: t for t in old.get('types', [])}
    for assembly, typename in TARGETS:
        artifact = SUITE / (assembly + '.dll')
        output = DEST / (typename.rsplit('.', 1)[-1].replace('`', '-') + '.cs')
        prior = previous.get(typename)
        if (prior and prior['artifact_sha256'] == digest(artifact) and output.exists()
                and prior['source_sha256'] == digest(output) and old['tool']['sha256'] == digest(DNSPY)
                and (not args.rea or prior.get('rea_evidence_id'))):
            if prior.get('rea_evidence'):
                evidence_path = ROOT / prior['rea_evidence']
                evidence = json.loads(evidence_path.read_text(encoding='utf-8-sig'))
                if prior.get('rea_evidence_sha256') and prior['rea_evidence_sha256'] != digest(evidence_path):
                    raise RuntimeError('Saved REA evidence changed: ' + typename)
                if evidence['subject']['digest']['sha256'] != digest(artifact) or evidence['evidence_id'] != prior['rea_evidence_id']:
                    raise RuntimeError('REA identity mismatch: ' + typename)
                prior['rea_evidence_sha256'] = digest(evidence_path)
                prior['rea_coverage'] = evidence['normalized_result'].get('coverage')
            manifest['types'].append(prior)
            print('Reused ' + typename, flush=True)
            continue
        if args.check:
            raise RuntimeError('Missing or changed type trace: ' + typename)
        command = [str(DNSPY), '--no-color', '--no-resources', '--no-baml', '-t', typename, str(artifact)]
        completed = subprocess.run(command, capture_output=True, timeout=180)
        if completed.returncode or not re.search(rb'\b(?:class|interface)\s+' + typename.rsplit('.', 1)[-1].split('`')[0].encode() + rb'\b', completed.stdout):
            raise RuntimeError(f'dnSpy failed for {typename}: {completed.stderr[-500:]!r}')
        # Decode BOM-aware stdout; preserve exact decompiler output bytes on disk.
        output.write_bytes(completed.stdout)
        item = {'type': typename, 'artifact': str(artifact), 'artifact_sha256': digest(artifact),
                'source': output.relative_to(ROOT).as_posix(), 'source_sha256': digest(output),
                'command': command}
        if args.rea:
            evidence_path = DEST / (output.stem + '.rea.json')
            with evidence_path.open('wb') as stream:
                # Windows .cmd launcher requires cmd.exe. Inputs are fixed local identities.
                result = subprocess.run(['cmd.exe', '/d', '/c', str(REA), 'inspect-managed-members',
                                         str(artifact), '--type', typename, '--format', 'json'],
                                        stdout=stream, stderr=subprocess.PIPE, timeout=180)
            if result.returncode:
                raise RuntimeError(f'REA failed for {typename}: {result.stderr[-500:]!r}')
            evidence = json.loads(evidence_path.read_text(encoding='utf-8-sig'))
            item.update(rea_evidence=evidence_path.relative_to(ROOT).as_posix(),
                        rea_evidence_id=evidence['evidence_id'], rea_evidence_sha256=digest(evidence_path),
                        rea_coverage=evidence['normalized_result'].get('coverage'))
        manifest['types'].append(item)
        print(typename, flush=True)
        manifest_path.write_text(
            json.dumps(manifest, ensure_ascii=False, indent=2) + '\n', encoding='utf-8')
    # Reused entries also belong in the final manifest, including those after
    # the last newly inspected type.
    manifest['canonical_metadata_counts'] = {state: sum((t.get('rea_coverage') or {}).get('state') == state for t in manifest['types'])
                                            for state in ['complete', 'partial', 'unavailable']}
    if manifest['canonical_metadata_counts']['unavailable']:
        manifest['limitations'].append('REA hit its 64 MiB metadata representation budget on Domain.Main. Those type traces use the separate dnSpy reconstruction; REA member metadata is unavailable, not confirmed.')
    if not args.check:
        manifest_path.write_text(json.dumps(manifest, ensure_ascii=False, indent=2) + '\n', encoding='utf-8')
    print(json.dumps({'types': len(manifest['types']), 'canonical_metadata': manifest['canonical_metadata_counts']}))


if __name__ == '__main__':
    main()
