param([Parameter(Mandatory=$true)][string]$BuildRoot,[switch]$PrepareOnly)
$ErrorActionPreference = 'Stop'
$projectRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
$versionMatch = [Regex]::Match([IO.File]::ReadAllText((Join-Path $projectRoot 'insight_version.py')), 'VERSION\s*=\s*"([^"]+)"')
if (-not $versionMatch.Success) { throw 'Versão da aplicação não encontrada.' }
$versionNumber = $versionMatch.Groups[1].Value
$resolvedBuild = [IO.Path]::GetFullPath($BuildRoot)
$allowedBuildRoot = [IO.Path]::GetFullPath((Join-Path $projectRoot '.release-build')) + [IO.Path]::DirectorySeparatorChar
if (-not $resolvedBuild.StartsWith($allowedBuildRoot,[StringComparison]::OrdinalIgnoreCase)) { throw 'A compilação precisa estar dentro de .release-build.' }
$releasePath = Join-Path $projectRoot 'release'
if ($PrepareOnly) { $releasePath = Join-Path $resolvedBuild 'prepared-release' }
$candidatePath = Join-Path $resolvedBuild 'dist\DigiSatInsight.exe'
if (-not (Test-Path -LiteralPath $candidatePath)) { throw 'Executável candidato não encontrado.' }
$verificationPath = Join-Path $resolvedBuild 'verification\verification.json'
if (-not (Test-Path -LiteralPath $verificationPath)) { throw 'Execute tools/verify-release.py antes de finalizar este candidato.' }
$verification = Get-Content -LiteralPath $verificationPath -Raw | ConvertFrom-Json
foreach ($newGate in @('dataset_discovery','database_editor_collection_create','database_editor_schema','database_editor_saved_queries','database_editor_page_export')) {
    if ($verification.$newGate -ne $true) { throw "Critério de validação não aprovado: $newGate" }
}
foreach ($nativeGate in @('native_contract_and_argument_tests','native_assets_and_authentication','finance_isolated_rules','native_natural_language_discovery','native_discovery_in_conversation','native_smart_filter_preview','native_preset_reference_injection_rejected')) {
    if ($verification.$nativeGate -ne $true) { throw "Critério de validação não aprovado: $nativeGate" }
}
foreach ($biGate in @('bi_rules','bi_presentation_math','bi_assets_and_authentication','bi_live_dashboard','bi_models_and_saved_views','bi_restart_persistence')) {
    if ($verification.$biGate -ne $true) { throw "Critério de validação não aprovado: $biGate" }
}
if ($verification.native_service_entries -ne 356) { throw 'Cobertura dos serviços nativos não confirmada.' }
if ($verification.stock_expiry -ne $true) { throw 'Critério de validação não aprovado: stock_expiry' }
foreach ($editorGate in @('database_editor_write_crud','database_editor_query_modes','database_editor_history')) {
    if ($verification.$editorGate -ne $true) { throw "Critério de validação não aprovado: $editorGate" }
}
if ($verification.exe_sha256 -ne (Get-FileHash -LiteralPath $candidatePath -Algorithm SHA256).Hash.ToLowerInvariant()) { throw 'O executável mudou depois da validação.' }
foreach ($gate in @('minimal_path','authentication','operator_fields_rejected','followups','clarifications','restart_persistence','native_quality','paging_and_complete_exports','pdf_excel_csv','schedule_alert_deduplication','profile_status_intersection','sqlite_backup_integrity','strict_read_only_rejects_broad_account','stability_regressions','saved_rename_persistence')) {
    if ($verification.$gate -ne $true) { throw "Critério de validação não aprovado: $gate" }
}
$destination = Join-Path $releasePath 'DigiSatInsight.exe'
if (Get-Process DigiSatInsight -ErrorAction SilentlyContinue | Where-Object { $_.Path -eq $destination }) { throw 'Encerre a instância local do DigiSat Insight antes de substituir o executável. Nenhum arquivo foi atualizado.' }
New-Item -ItemType Directory -Path $releasePath -Force | Out-Null
if ((Test-Path -LiteralPath (Join-Path $releasePath 'LEIA-ME.md')) -and -not (Test-Path -LiteralPath (Join-Path $resolvedBuild 'LEIA-ME-previous.md'))) { Copy-Item -LiteralPath (Join-Path $releasePath 'LEIA-ME.md') -Destination (Join-Path $resolvedBuild 'LEIA-ME-previous.md') }
if ((Test-Path -LiteralPath $destination) -and -not (Test-Path -LiteralPath (Join-Path $resolvedBuild 'DigiSatInsight-previous.exe'))) { Copy-Item -LiteralPath $destination -Destination (Join-Path $resolvedBuild 'DigiSatInsight-previous.exe') }
for ($attempt=0; $attempt -lt 10; $attempt++) {
    try { Copy-Item -LiteralPath $candidatePath -Destination $destination -Force; break }
    catch { if ($attempt -eq 9) { throw }; Start-Sleep -Milliseconds 500 }
}
if ((Get-FileHash $candidatePath).Hash -ne (Get-FileHash $destination).Hash) { throw 'Hash do executável publicado difere do candidato.' }
Copy-Item -LiteralPath (Join-Path $projectRoot 'docs\guia-1.0.md') -Destination (Join-Path $releasePath 'LEIA-ME.md') -Force
$version = @{version=$versionNumber;status='release-candidate';platform='Windows x64';built_at=[DateTime]::UtcNow.ToString('o');python_required=$false;external_upload=$false}
$version | ConvertTo-Json | Set-Content -LiteralPath (Join-Path $releasePath 'VERSION.json') -Encoding UTF8
$distribution = Join-Path $resolvedBuild 'distribution'
New-Item -ItemType Directory -Path $distribution -Force | Out-Null
foreach ($name in @('DigiSatInsight.exe','LEIA-ME.md','VERSION.json')) { Copy-Item -LiteralPath (Join-Path $releasePath $name) -Destination $distribution }
# Only a credential-free template enters the handoff ZIP. Local settings, keys,
# saved customer results, tests, agents and engineering artifacts are never copied.
$template = @'
MONGO_URI=mongodb://127.0.0.1:12220/?directConnection=true&readPreference=secondaryPreferred
MONGO_DATABASE=DigisatServer
MONGO_USERNAME=insight
MONGO_AUTH_SOURCE=admin
MONGO_PASSWORD_FILE=
DIGISAT_MODE=live
MAX_REPORT_ROWS=500
INSIGHT_USER=
INSIGHT_PASSWORD=
INSIGHT_REQUIRE_READ_ONLY=1
AI_ENDPOINT=
AI_MODEL=
AI_API_KEY=
'@
[IO.File]::WriteAllText((Join-Path $distribution 'config.env'),$template,[Text.UTF8Encoding]::new($false))
if (-not (Test-Path -LiteralPath (Join-Path $releasePath 'config.env'))) { Copy-Item -LiteralPath (Join-Path $distribution 'config.env') -Destination $releasePath }
$checksums = Get-ChildItem -LiteralPath $distribution -File | Where-Object Name -ne 'SHA256SUMS' | Sort-Object Name | ForEach-Object { '{0}  {1}' -f (Get-FileHash -LiteralPath $_.FullName -Algorithm SHA256).Hash.ToLowerInvariant(),$_.Name }
[IO.File]::WriteAllLines((Join-Path $distribution 'SHA256SUMS'),[string[]]$checksums,[Text.UTF8Encoding]::new($false))
$archiveName = "DigiSatInsight-$versionNumber-win64.zip"
$zipPath = Join-Path $releasePath $archiveName
Compress-Archive -Path (Join-Path $distribution '*') -DestinationPath $zipPath -Force
$checksums = @('DigiSatInsight.exe','LEIA-ME.md','VERSION.json',$archiveName) | ForEach-Object { '{0}  {1}' -f (Get-FileHash -LiteralPath (Join-Path $releasePath $_) -Algorithm SHA256).Hash.ToLowerInvariant(),$_ }
[IO.File]::WriteAllLines((Join-Path $releasePath 'SHA256SUMS'),[string[]]$checksums,[Text.UTF8Encoding]::new($false))
if ($PrepareOnly) { Write-Output ('Candidato preparado sem alterar a release local: ' + $releasePath) }
else { Write-Output 'Release atualizada. ZIP de entrega contém apenas o executável, configuração sem credenciais, guia, versão e hashes. Configuração e dados locais preservados.' }
