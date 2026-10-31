"""Publish the verified portable release, without logging authentication material.

Default mode is read-only. --publish requires the matching remote tag and a
verified package. Interrupted uploads can resume only into this tag's draft.
"""
from pathlib import Path
import argparse
import hashlib
import json
import os
import subprocess
import sys
import urllib.error
import urllib.parse
import urllib.request

ROOT = Path(__file__).resolve().parents[1]
sys.path.insert(0, str(ROOT))
from insight_version import VERSION

REPO = 'zGabrielR1/projetoIADigisat'
TAG = 'v' + VERSION
parser = argparse.ArgumentParser()
parser.add_argument('--publish', action='store_true')
parser.add_argument('--assets-dir', type=Path, default=ROOT / 'release')
args = parser.parse_args()

token = os.environ.get('GH_TOKEN') or os.environ.get('GITHUB_TOKEN')
if not token:
    environment = os.environ.copy()
    environment.update(GIT_TERMINAL_PROMPT='0', GCM_INTERACTIVE='never')
    filled = subprocess.run(['git', 'credential', 'fill'], input='protocol=https\nhost=github.com\n\n',
                            cwd=ROOT, text=True, capture_output=True, env=environment, timeout=60)
    credentials = dict(line.split('=', 1) for line in filled.stdout.splitlines() if '=' in line)
    token = credentials.get('password')
    if not token:
        raise SystemExit('GitHub authentication unavailable. No credential values were logged.')


def api(path, method='GET', data=None, binary=False):
    url = path if path.startswith('https://') else 'https://api.github.com' + path
    if urllib.parse.urlparse(url).hostname not in {'api.github.com', 'uploads.github.com'}:
        raise ValueError('Unexpected GitHub API host')
    headers = {'Authorization': 'Bearer ' + token, 'Accept': 'application/vnd.github+json',
               'User-Agent': 'DigiSatInsight-release', 'X-GitHub-Api-Version': '2022-11-28'}
    if data is not None:
        headers['Content-Type'] = 'application/octet-stream' if binary else 'application/json'
        if not binary:
            data = json.dumps(data).encode('utf-8')
    try:
        with urllib.request.urlopen(urllib.request.Request(url, data=data, headers=headers, method=method), timeout=180) as response:
            return json.load(response)
    except urllib.error.HTTPError as exc:
        raise SystemExit(f'GitHub returned HTTP {exc.code} for {method} {urllib.parse.urlparse(url).path}') from None


repository = api('/repos/' + REPO)
releases = api('/repos/' + REPO + '/releases?per_page=100')
print(json.dumps({'repository': repository['full_name'], 'visibility': repository['visibility'],
                  'can_push': repository.get('permissions', {}).get('push'), 'target_tag': TAG,
                  'releases': [{'tag': item['tag_name'], 'draft': item['draft']} for item in releases]}), flush=True)
if not args.publish:
    raise SystemExit(0)

assets = args.assets_dir.resolve()
checked = subprocess.run([sys.executable, str(ROOT / 'tools/check-release-package.py'), '--release-dir', str(assets)], cwd=ROOT)
if checked.returncode:
    raise SystemExit('Package validation failed; no release was created.')
build = Path((ROOT / '.release-build/latest-build-path').read_text(encoding='utf-8'))
verification = json.loads((build / 'verification/verification.json').read_text(encoding='utf-8'))
if verification['version'] != VERSION or verification['exe_sha256'] != hashlib.sha256((assets / 'DigiSatInsight.exe').read_bytes()).hexdigest():
    raise SystemExit('The release candidate does not match its verification report.')
for gate in ['independent_monthly_audit', 'fractional_unit_price_audit', 'snapshot_validation', 'bi_rules', 'bi_presentation_math', 'bi_assets_and_authentication', 'bi_live_dashboard', 'bi_models_and_saved_views', 'bi_restart_persistence', 'connection_inspection', 'light_theme_assets', 'finance_drill_down']:
    if verification.get(gate) is not True:
        raise SystemExit('BI gate not approved: ' + gate)

sha = subprocess.check_output(['git', 'rev-parse', 'HEAD'], cwd=ROOT, text=True).strip()
reference = api('/repos/' + REPO + '/git/ref/tags/' + TAG)
if reference['object']['type'] == 'tag':
    reference = api('/repos/' + REPO + '/git/tags/' + reference['object']['sha'])
if reference['object']['sha'] != sha:
    raise SystemExit('The remote tag does not identify the current commit.')
notes = (ROOT / 'docs' / ('release-' + VERSION + '.md')).read_text(encoding='utf-8')
name = 'DigiSat Insight ' + VERSION.replace('-rc.', ' RC')
matches = [item for item in releases if item['tag_name'] == TAG]
if matches:
    release = matches[0]
    if not release['draft']:
        raise SystemExit('This version is already published; refusing to alter it.')
else:
    release = api('/repos/' + REPO + '/releases', 'POST', {'tag_name': TAG, 'target_commitish': sha,
                  'name': name, 'body': notes, 'draft': True, 'prerelease': '-rc.' in VERSION})
upload = release['upload_url'].split('{')[0]
existing = {item['name']: item for item in release.get('assets', [])}
names = ['DigiSatInsight-' + VERSION + '-win64.zip', 'DigiSatInsight.exe', 'SHA256SUMS', 'VERSION.json', 'LEIA-ME.md']
for filename in names:
    content = (assets / filename).read_bytes()
    digest = 'sha256:' + hashlib.sha256(content).hexdigest()
    if filename in existing:
        item = existing[filename]
        if item['size'] != len(content) or item.get('digest') != digest:
            raise SystemExit('An existing draft asset differs from the verified candidate: ' + filename)
        print('Verified existing asset: ' + filename, flush=True)
        continue
    item = api(upload + '?name=' + urllib.parse.quote(filename), 'POST', content, binary=True)
    if item['size'] != len(content) or (item.get('digest') and item['digest'] != digest):
        raise SystemExit('Uploaded asset failed size/digest verification: ' + filename)
    print('Uploaded ' + filename, flush=True)

published = api('/repos/' + REPO + '/releases/' + str(release['id']), 'PATCH',
                {'draft': False, 'prerelease': '-rc.' in VERSION, 'name': name, 'body': notes})
confirmed = api('/repos/' + REPO + '/releases/' + str(release['id']))
if confirmed['draft'] or {item['name'] for item in confirmed['assets']} != set(names):
    raise SystemExit('Release publication could not be confirmed.')
print(json.dumps({'published': published['html_url'], 'tag': TAG, 'commit': sha,
                  'prerelease': confirmed['prerelease'], 'assets': [item['name'] for item in confirmed['assets']]}), flush=True)
