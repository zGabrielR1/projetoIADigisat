param([switch]$StageOnly)
$ErrorActionPreference = 'Stop'
$projectRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
$pythonPath = Join-Path $projectRoot '.venv\Scripts\python.exe'
if (-not (Test-Path -LiteralPath $pythonPath)) { throw 'Crie o ambiente Python e instale requirements.txt e PyInstaller antes de compilar.' }
$stamp = Get-Date -Format 'yyyyMMdd-HHmmss-fff'
$buildRoot = Join-Path $projectRoot ".release-build\build-$stamp"
$distPath = Join-Path $buildRoot 'dist'
$releasePath = Join-Path $projectRoot 'release'
New-Item -ItemType Directory -Path $buildRoot,$distPath,$releasePath -Force | Out-Null
foreach ($name in @('app.py','report_questions.py','launcher.py')) {
    Copy-Item -LiteralPath (Join-Path $projectRoot $name) -Destination (Join-Path $buildRoot $name)
}
Get-ChildItem -LiteralPath $projectRoot -Filter 'insight_*.py' -File | ForEach-Object { Copy-Item -LiteralPath $_.FullName -Destination $buildRoot }
Copy-Item -LiteralPath (Join-Path $projectRoot 'static') -Destination (Join-Path $buildRoot 'static') -Recurse
$indexPath = Join-Path $buildRoot 'static\index.html'
$indexText = [IO.File]::ReadAllText($indexPath)
Get-ChildItem -LiteralPath (Join-Path $buildRoot 'static') -File | Where-Object Extension -in @('.js','.css') | ForEach-Object {
    $assetHash = (Get-FileHash -LiteralPath $_.FullName -Algorithm SHA256).Hash.Substring(0,12).ToLowerInvariant()
    $assetPattern = '/static/' + [Regex]::Escape($_.Name) + '(?:\?[^"\s]*)?"'
    $indexText = [Regex]::Replace($indexText,$assetPattern,('/static/' + $_.Name + '?v=' + $assetHash + '"'))
}
[IO.File]::WriteAllText($indexPath,$indexText,[Text.UTF8Encoding]::new($false))
& $pythonPath -m PyInstaller --noconfirm --onefile --console --name DigiSatInsight --distpath $distPath --workpath (Join-Path $buildRoot 'work') --specpath $buildRoot --add-data ((Join-Path $buildRoot 'static') + ';static') --paths $buildRoot --collect-data reportlab --collect-submodules uvicorn --exclude-module unittest --exclude-module pytest (Join-Path $buildRoot 'launcher.py')
if ($LASTEXITCODE -ne 0) { throw 'Falha na compilação; a release anterior foi preservada.' }
$candidatePath = Join-Path $distPath 'DigiSatInsight.exe'
& $candidatePath --version
if ($LASTEXITCODE -ne 0) { throw 'O executável não passou na verificação de versão.' }
[IO.File]::WriteAllText((Join-Path $projectRoot '.release-build\latest-build-path'),$buildRoot,[Text.UTF8Encoding]::new($false))
if ($StageOnly) { Write-Output "BUILD_READY=$buildRoot"; return }
& $pythonPath (Join-Path $projectRoot 'tools\verify-release.py')
if ($LASTEXITCODE -ne 0) { throw 'A validação do executável falhou. A release anterior foi preservada.' }
& (Join-Path $projectRoot 'finalize-release.ps1') -BuildRoot $buildRoot
