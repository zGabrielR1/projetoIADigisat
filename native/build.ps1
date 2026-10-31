param([string]$OutputDirectory = "$PSScriptRoot\..\.release-build\native-host", [string]$JsonAssembly = $env:INSIGHT_BUILD_NEWTONSOFT)
$ErrorActionPreference='Stop'
$compiler=Join-Path $env:WINDIR 'Microsoft.NET\Framework\v4.0.30319\csc.exe'
$json=if ($JsonAssembly) { $JsonAssembly } else { 'C:\Digisat\SuiteG6\Sistema\Newtonsoft.Json.dll' }
if (-not (Test-Path -LiteralPath $compiler)) { throw '.NET Framework C# compiler is required.' }
if (-not (Test-Path -LiteralPath $json)) { throw 'Installed SuiteG6 Newtonsoft.Json.dll not found.' }
New-Item -ItemType Directory -Path $OutputDirectory -Force | Out-Null
$target=Join-Path $OutputDirectory 'NativeReportHost.exe'
& $compiler /nologo /target:exe /platform:x86 "/out:$target" "/reference:$json" /reference:System.ServiceModel.dll /reference:System.Core.dll "/reference:System.Runtime.Serialization.dll" (Join-Path $PSScriptRoot 'NativeReportHost.cs')
if ($LASTEXITCODE -ne 0) { throw 'Native report host build failed.' }
Write-Output $target
