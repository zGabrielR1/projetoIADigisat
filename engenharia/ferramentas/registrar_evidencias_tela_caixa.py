import json,hashlib,re
from pathlib import Path
from datetime import datetime,timezone
root=Path(r'C:\Digisat\SuiteG6')
assemblies=[root/'Sistema/Modulos/Modulo.Turnos.dll',*[root/'Servidor'/n for n in ['DigisatServer.Application.Core.dll','DigisatServer.Application.Main.Dto.dll','DigisatServer.Application.Main.dll']]]
logs=[];matches=[]
pat=re.compile(r'6ad0d1f435c0e8154819c2a7|6ad0d9db35c0e814887e5694|411[,.]8(?:0)?|TotalCreditoLoja',re.I)
for base in [root/'Log',root/'Servidor/Logs',root/'MongoDB/log']:
 files=list(base.rglob('*')) if base.exists() else []
 files=[p for p in files if p.is_file()]
 item={'path':str(base),'files':len(files),'bytes':sum(p.stat().st_size for p in files),'matching_files':[]}
 for p in files:
  if p.suffix.lower() not in ['.txt','.log'] and not p.name.startswith('mongo.log'):continue
  txt=p.read_text(encoding='utf-8',errors='replace')
  hits=[i for i,l in enumerate(txt.splitlines(),1) if pat.search(l)]
  if hits:item['matching_files'].append({'file':str(p),'lines':hits})
 logs.append(item)
result={'assemblies':[{'path':str(p),'sha256':hashlib.sha256(p.read_bytes()).hexdigest(),'bytes':p.stat().st_size} for p in assemblies],'log_search':logs,'oplog_bounds_utc':[datetime.fromtimestamp(t,timezone.utc).isoformat() for t in [1792505463,1792669057]],'tools':'ILSpy 11.1.0.9782; REA MCP ausente, ilspycmd local, sem execução da aplicação-alvo'}
Path('docs/evidencias-tela-logs-caixa.json').write_text(json.dumps(result,ensure_ascii=False,indent=2),encoding='utf-8')
print(json.dumps(result,ensure_ascii=False,indent=2))
