"""Read a temporary native login from stdin; save only counts and pass flags."""
import argparse
import json
from pathlib import Path
import sys
import getpass
import urllib.request

parser=argparse.ArgumentParser()
parser.add_argument('--base',default='http://127.0.0.1:8104')
args=parser.parse_args()
if sys.stdin.isatty():
    login={'username':input('SuiteG6 user: '),'company_id':input('Company ID: '),'password':getpass.getpass('SuiteG6 password: ')}
else:
    login=json.loads(sys.stdin.readline())
session=None
def request(path,body):
    headers={'Content-Type':'application/json'}
    if session:headers['X-Native-Session']=session
    req=urllib.request.Request(args.base+path,data=json.dumps(body).encode(),headers=headers)
    with urllib.request.urlopen(req,timeout=90) as response:return json.load(response)
try:
    session=request('/api/native/connect',login)['session']
    login.clear()
    result=request('/api/native/query',{'report_id':'OperacaoFiscal.ListaCadastroCfop','arguments':{},'offset':10,'page_size':10})
    pagination=result['pagination']
    assert pagination['offset']==10 and len(result['rows'])==10 and pagination['total']==40
    evidence={'report_id':result['report_id'],'status':200,'total':pagination['total'],'offset':10,'returned':len(result['rows'])}
    path=Path(__file__).resolve().parents[1]/'docs/native-verification-1.0.3-rc.1.json'
    saved=json.loads(path.read_text(encoding='utf-8'))
    saved['packaged_http']=evidence
    path.write_text(json.dumps(saved,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
    print(json.dumps(evidence),flush=True)
finally:
    login.clear()
    if session:
        try:request('/api/native/disconnect',{})
        except Exception:pass
