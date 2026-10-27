$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location $root
if (-not (Test-Path '.env')) { Copy-Item '.env.example' '.env'; Write-Host 'Criei .env a partir do exemplo. Revise as opções antes de consultar.' }
$venvPython = Join-Path $root '.venv\Scripts\python.exe'
if (-not (Test-Path $venvPython)) { python -m venv --without-pip .venv }
# Bootstrap/install through the system pip so a Python distribution without ensurepip still works.
python -m pip --python $venvPython install -r requirements.txt
$hostAddress = if ($env:INSIGHT_HOST) { $env:INSIGHT_HOST } else { '127.0.0.1' }
& '.venv\Scripts\python.exe' -m uvicorn app:app --host $hostAddress --port 8088 --env-file .env
