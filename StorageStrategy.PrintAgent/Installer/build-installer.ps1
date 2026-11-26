<#
  Gera o instalador do agente de impressão (setup.exe).
  Saída: StorageStrategy_API\installer-dist\StorageStrategy-Impressora-Setup-<versao>.exe  (pasta fora do Git)

  Uso (na pasta StorageStrategy_API):
    .\StorageStrategy.PrintAgent\Installer\build-installer.ps1 -Version 1.0.0

  Requer Inno Setup 6:  winget install JRSoftware.InnoSetup
#>
param(
    [string]$Version = "1.0.0"
)

$ErrorActionPreference = 'Stop'

$projectDir = Split-Path -Parent $PSScriptRoot
$apiDir = Split-Path -Parent $projectDir
$distDir = Join-Path $apiDir 'installer-dist'
$publishDir = Join-Path $distDir 'publish'

$iscc = @(
    "${env:ProgramFiles(x86)}\Inno Setup 6\ISCC.exe",
    "$env:ProgramFiles\Inno Setup 6\ISCC.exe",
    "$env:LOCALAPPDATA\Programs\Inno Setup 6\ISCC.exe"
) | Where-Object { Test-Path $_ } | Select-Object -First 1

if (-not $iscc) {
    throw "Inno Setup 6 não encontrado. Instale com: winget install JRSoftware.InnoSetup"
}

Write-Host "1/2 Publicando o agente $Version..." -ForegroundColor Cyan
if (Test-Path $publishDir) { Remove-Item $publishDir -Recurse -Force }
dotnet publish $projectDir -c Release -r win-x64 --self-contained `
    -p:PublishSingleFile=true -p:Version=$Version -o $publishDir
if ($LASTEXITCODE -ne 0) { throw "dotnet publish falhou" }

Write-Host "2/2 Gerando o instalador..." -ForegroundColor Cyan
& $iscc "/DAppVersion=$Version" "/DPublishDir=$publishDir" "/O$distDir" (Join-Path $PSScriptRoot 'PrintAgent.iss')
if ($LASTEXITCODE -ne 0) { throw "ISCC falhou" }

$setup = Join-Path $distDir "StorageStrategy-Impressora-Setup-$Version.exe"
Write-Host "Instalador gerado: $setup" -ForegroundColor Green
