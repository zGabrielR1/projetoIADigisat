param(
    [string]$SuiteRoot = 'C:\Digisat\SuiteG6',
    [Parameter(Mandatory=$true)][string]$OutputDirectory,
    [string]$Identificacao = $env:COMPUTERNAME
)
# Read-only inspection. Writes only the requested diagnostic JSON directory.
# No logon, database reads, synchronization, service control or target execution.
$ErrorActionPreference = 'Stop'
$diagnosticWarnings = [System.Collections.Generic.List[string]]::new()
$suiteBase = [IO.Path]::GetFullPath($SuiteRoot)
$outputBase = [IO.Path]::GetFullPath($OutputDirectory)
New-Item -ItemType Directory -Path $outputBase -Force | Out-Null
$artifactNames = @(
    'Sistema\SistemaG6.exe','Sistema\Boot.dll','Sistema\Service.dll','Sistema\Infraestrutura.dll',
    'Sistema\Modulos\Modulo.Relatorios.dll','Sistema\DigisatServer.Application.Main.dll',
    'Sistema\DigisatServer.Distributed.Wcf.Main.dll','Servidor\ServidorG6.exe',
    'Servidor\DigisatServer.Application.Main.dll','Servidor\DigisatServer.Distributed.Wcf.Main.dll',
    'Servidor\DigisatServer.Distributed.Akka.Main.dll','Sincronizador\SincronizadorG6.exe',
    'Sincronizador\DigisatServer.Infrastructure.Data.Core.dll'
)
$artifacts = @(foreach ($relative in $artifactNames) {
    $path = Join-Path $suiteBase $relative
    if (Test-Path -LiteralPath $path -PathType Leaf) {
        $file = Get-Item -LiteralPath $path
        [ordered]@{relative_path=$relative;file_version=$file.VersionInfo.FileVersion;
            sha256=(Get-FileHash -LiteralPath $path -Algorithm SHA256).Hash.ToLowerInvariant();
            bytes=$file.Length;last_write_utc=$file.LastWriteTimeUtc.ToString('o')}
    }
})
$configurations = @(foreach ($relative in @('Sistema\ConfiguracaoClient.xml','Sincronizador\ConfiguracaoClient.xml')) {
    $path = Join-Path $suiteBase $relative
    if (-not (Test-Path -LiteralPath $path -PathType Leaf)) { continue }
    $endpointFields = @()
    $reader = $null
    try {
        $settings = [Xml.XmlReaderSettings]::new()
        $settings.DtdProcessing = [Xml.DtdProcessing]::Prohibit
        $settings.XmlResolver = $null
        $reader = [Xml.XmlReader]::Create($path, $settings)
        $document = [Xml.XmlDocument]::new()
        $document.XmlResolver = $null
        $document.Load($reader)
        $endpointFields = @($document.SelectNodes('//*') | Where-Object {
            $_.LocalName -in @('Ip','Porta') -and $_.ChildNodes.Count -le 1
        } | ForEach-Object {
            # Only literal host/IP or numeric ports. Never emit URI credentials.
            $value = $_.InnerText.Trim()
            if ($value -match '^[a-zA-Z0-9_.:\-\[\]]{1,253}$') {
                [ordered]@{field=$_.LocalName;value=$value}
            }
        })
    } catch { $diagnosticWarnings.Add("Could not parse endpoint fields in $relative; raw configuration was not copied.") }
    finally { if ($reader) { $reader.Dispose() } }
    [ordered]@{relative_path=$relative;endpoint_fields=$endpointFields;
        sha256=(Get-FileHash -LiteralPath $path -Algorithm SHA256).Hash.ToLowerInvariant()}
})
$services = @()
try {
    $services = @(Get-Service | Where-Object {$_.Name -match 'Digisat|SuiteG6|Sincronizador'} | ForEach-Object {
        [ordered]@{name=$_.Name;status=$_.Status.ToString()}
    })
} catch { $diagnosticWarnings.Add('Service status unavailable.') }
$suiteProcesses = @(Get-Process | Where-Object {$_.ProcessName -in @('SistemaG6','ServidorG6','SincronizadorG6')})
$processes = @($suiteProcesses | ForEach-Object {
    $processPath = $null
    try { $processPath = $_.Path } catch { }
    [ordered]@{name=$_.ProcessName;pid=$_.Id;executable=$processPath}
})
$connections = @()
if ($suiteProcesses.Count -gt 0) {
    try {
        $processIds = @($suiteProcesses | ForEach-Object {$_.Id})
        $connections = @(Get-NetTCPConnection -ErrorAction Stop | Where-Object {
            $_.OwningProcess -in $processIds -and
            ($_.LocalPort -in @(12220,12221,12222) -or $_.RemotePort -in @(12220,12221,12222))
        } | ForEach-Object {
            [ordered]@{pid=$_.OwningProcess;local_address=$_.LocalAddress;local_port=$_.LocalPort;
                remote_address=$_.RemoteAddress;remote_port=$_.RemotePort;state=$_.State.ToString()}
        })
    } catch { $diagnosticWarnings.Add('TCP connection inventory unavailable; confirm endpoints manually.') }
}
$report = [ordered]@{
    schema_version=1;captured_utc=[DateTime]::UtcNow.ToString('o');machine=$Identificacao;
    logical_processors=[Environment]::ProcessorCount;read_only=$true;
    limitations=@('No database data, credentials or raw configuration contents are included.',
        'Connection snapshots do not identify a particular report request.',
        'IP/port fields are configured values, not proof of the report endpoint.',
        'No report was executed; record exact filters and company/user separately.');
    artifacts=$artifacts;configurations=$configurations;services=$services;
    processes=$processes;connections=$connections;warnings=@($diagnosticWarnings.ToArray())
}
$safeLabel = [Regex]::Replace($Identificacao, '[^a-zA-Z0-9_-]', '_')
if (-not $safeLabel) { $safeLabel = 'machine' }
$filename = 'sincronizacao-' + $safeLabel + '-' + [DateTime]::UtcNow.ToString('yyyyMMdd-HHmmss-fff') + '.json'
$destination = Join-Path $outputBase $filename
$report | ConvertTo-Json -Depth 8 | Set-Content -LiteralPath $destination -Encoding UTF8
Write-Output $destination
