"""Verify the handoff allowlist, hashes, sanitized config, and Windows PE target."""
from pathlib import Path
import hashlib
import io
import json
import struct
import zipfile
import sys
import argparse
from dotenv import dotenv_values

root=Path(__file__).resolve().parent.parent
sys.path.insert(0,str(root))
from insight_version import VERSION
parser=argparse.ArgumentParser()
parser.add_argument('--release-dir',type=Path,default=root/'release')
release=parser.parse_args().release_dir.resolve()
archive=release/f'DigiSatInsight-{VERSION}-win64.zip'
expected={'DigiSatInsight.exe','config.env','LEIA-ME.md','VERSION.json','SHA256SUMS'}
with zipfile.ZipFile(archive) as package:
    assert set(package.namelist())==expected,package.namelist()
    config=dotenv_values(stream=io.StringIO(package.read('config.env').decode('utf-8-sig')))
    assert not config.get('MONGO_PASSWORD_FILE')
    assert not config.get('INSIGHT_PASSWORD')
    assert not config.get('AI_API_KEY')
    assert '@' not in config['MONGO_URI']
    assert config['INSIGHT_REQUIRE_READ_ONLY']=='1'
    for line in package.read('SHA256SUMS').decode('utf-8-sig').splitlines():
        digest,name=line.split('  ',1)
        assert hashlib.sha256(package.read(name)).hexdigest()==digest
    assert hashlib.sha256(package.read('DigiSatInsight.exe')).hexdigest()==hashlib.sha256((release/'DigiSatInsight.exe').read_bytes()).hexdigest()
    binary=package.read('DigiSatInsight.exe')
    assert binary[:2]==b'MZ'
    offset=struct.unpack_from('<I',binary,60)[0]
    assert binary[offset:offset+4]==b'PE\0\0'
    assert struct.unpack_from('<H',binary,offset+4)[0]==0x8664
    version=json.loads(package.read('VERSION.json').decode('utf-8-sig'))
    assert version['version']==VERSION
for line in (release/'SHA256SUMS').read_text(encoding='utf-8-sig').splitlines():
    digest,name=line.split('  ',1)
    assert hashlib.sha256((release/name).read_bytes()).hexdigest()==digest
build=Path((root/'.release-build/latest-build-path').read_text(encoding='utf-8'))
analysis=(build/'work/DigiSatInsight/Analysis-00.toc').read_text(encoding='utf-8')
for forbidden in ['test_workspace.py','test_e2e_reports.py','test_question_investigation.py','test_stability.py','senhamongo.txt','engenharia\\','AGENTS.md','.commandcode']:
    assert forbidden not in analysis,forbidden
print('Verified: Windows x64 PE; handoff ZIP allowlist; no credentials, local data, tests, agents or engineering artifacts; all SHA-256 hashes match.')
