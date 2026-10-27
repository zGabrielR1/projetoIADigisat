param(
    [string]$BackupDirectory = 'C:\Digisat\SuiteG6\servidor\backup\dados',
    [string]$MongoBin = 'C:\Digisat\SuiteG6\MongoDB\bin',
    [string]$DatabaseName = 'InsightBackup',
    [int]$Port = 12231
)
$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.IO.Compression.FileSystem

# Backup jobs can leave a partial archive behind (catalogs only, movements empty).
# Restoring it would produce a database with nothing to report, so the newest
# archive that actually carries the movements is selected instead.
function Get-UsableBackup {
    param([string]$Directory)
    $candidates = Get-ChildItem -LiteralPath $Directory -Filter 'DigisatServer*.zip' -File | Sort-Object LastWriteTime -Descending
    $skipped = @()
    foreach ($file in $candidates) {
        try {
            $archive = [System.IO.Compression.ZipFile]::OpenRead($file.FullName)
            try {
                $entry = $archive.GetEntry('Movimentacoes.bson.gz')
            } finally { $archive.Dispose() }
            if ($entry -and $entry.Length -gt 1024) {
                if ($skipped.Count) { Write-Warning "Ignorados por não conterem movimentações: $($skipped -join ', ')" }
                return $file
            }
            $skipped += $file.Name
        } catch { $skipped += $file.Name }
    }
    return $null
}

$archive = Get-UsableBackup -Directory $BackupDirectory
if (-not $archive) { throw "Não encontrei nenhum DigisatServer*.zip com dados em $BackupDirectory" }
foreach ($exe in @('mongod.exe','mongorestore.exe')) { if (-not (Test-Path (Join-Path $MongoBin $exe))) { throw "Executável não encontrado: $(Join-Path $MongoBin $exe)" } }
$probe = [System.Net.Sockets.TcpListener]::new([System.Net.IPAddress]::Loopback, $Port)
$portAvailable = $false
try { $probe.Start(); $portAvailable = $true } catch { throw "A porta $Port já está ocupada ou reservada pelo Windows (faixa excluída). Escolha outra porta com -Port. Não vou conectar nem restaurar sobre o processo existente." } finally { if ($portAvailable) { $probe.Stop() } }
$stamp = Get-Date -Format 'yyyyMMdd-HHmmss'
$base = Join-Path $env:LOCALAPPDATA "DigiSatInsight\backups\$stamp"
$dump = Join-Path $base 'dump'
$dbPath = Join-Path $base 'mongo-data'
New-Item -ItemType Directory -Path $dump,$dbPath -Force | Out-Null
# Validate ZIP paths before extraction (reject absolute paths and parent traversal).
$zip = [System.IO.Compression.ZipFile]::OpenRead($archive.FullName)
try {
    foreach ($entry in $zip.Entries) {
        $name = $entry.FullName.Replace('\','/')
        if ($name.StartsWith('/') -or $name -match '(^|/)\.\.(?:/|$)' -or $name -match '^[A-Za-z]:') { throw "Entrada insegura no ZIP: $($entry.FullName)" }
    }
} finally { $zip.Dispose() }
Expand-Archive -LiteralPath $archive.FullName -DestinationPath $dump
$mongod = Start-Process -FilePath (Join-Path $MongoBin 'mongod.exe') -ArgumentList @('--dbpath',"`"$dbPath`"",'--port',"$Port",'--bind_ip','127.0.0.1','--logpath',"`"$(Join-Path $base 'mongod.log')`"") -WindowStyle Hidden -PassThru
try {
    $ready = $false
    for ($i=0; $i -lt 30; $i++) {
        if ($mongod.HasExited) { throw 'mongod isolado encerrou antes de ficar pronto.' }
        $client = [System.Net.Sockets.TcpClient]::new()
        try { $client.Connect('127.0.0.1',$Port); $ready=$true; break } catch { Start-Sleep -Milliseconds 500 } finally { $client.Dispose() }
    }
    if (-not $ready) { throw "mongod não abriu a porta $Port. Consulte $base\mongod.log" }
    & (Join-Path $MongoBin 'mongorestore.exe') --host 127.0.0.1 --port $Port --db $DatabaseName --gzip --dir $dump
    if ($LASTEXITCODE -ne 0) { throw "mongorestore retornou $LASTEXITCODE; veja o log e a pasta $base" }
} catch {
    Write-Error "Restauração interrompida. A instância isolada pode continuar ativa (PID $($mongod.Id)); os dados estão em $base. Detalhe: $_"
    throw
}
Write-Host "Backup restaurado. ZIP: $($archive.FullName)"
Write-Host "Mongo isolado: 127.0.0.1:$Port / banco $DatabaseName (PID $($mongod.Id))"
Write-Host "Dados locais: $dbPath"
Write-Host 'Configure DIGISAT_MODE=backup e MONGO_DATABASE=InsightBackup no .env e inicie o painel.'
