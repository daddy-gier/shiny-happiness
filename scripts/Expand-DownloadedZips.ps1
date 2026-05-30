<#
.SYNOPSIS
    Expand every .zip in this repo (downloaded GitHub archives) into D:\git repos.

.DESCRIPTION
    Mirrors the behavior of "pull all my gits": for each *.zip in the source folder,
    extract it into the destination. If a folder of the same name already exists, it is
    skipped (use -Force to overwrite).

.PARAMETER Source
    Folder containing .zip archives. Defaults to the repo root (parent of this script).

.PARAMETER Destination
    Target folder. Defaults to "D:\git repos".

.PARAMETER Force
    Overwrite an existing destination folder for that archive.

.EXAMPLE
    .\Expand-DownloadedZips.ps1
    .\Expand-DownloadedZips.ps1 -Destination "D:\git repos" -Force
#>
[CmdletBinding()]
param(
    [string]$Source,
    [string]$Destination = "D:\git repos",
    [switch]$Force
)

$ErrorActionPreference = "Stop"

if (-not $Source) {
    $Source = Split-Path -Parent $PSScriptRoot
}

if (-not (Test-Path -LiteralPath $Source)) {
    throw "Source folder not found: $Source"
}
if (-not (Test-Path -LiteralPath $Destination)) {
    New-Item -ItemType Directory -Path $Destination -Force | Out-Null
}

$zips = Get-ChildItem -LiteralPath $Source -Filter *.zip -File
if (-not $zips) {
    Write-Host "No .zip files found in $Source" -ForegroundColor Yellow
    return
}

$extracted = 0
$skipped   = 0
$failed    = @()

foreach ($zip in $zips) {
    $name   = [IO.Path]::GetFileNameWithoutExtension($zip.Name)
    $outDir = Join-Path $Destination $name

    if ((Test-Path -LiteralPath $outDir) -and -not $Force) {
        Write-Host "[skip]    $($zip.Name) -> $outDir already exists" -ForegroundColor DarkGray
        $skipped++
        continue
    }

    try {
        if (Test-Path -LiteralPath $outDir) {
            Remove-Item -LiteralPath $outDir -Recurse -Force
        }
        Write-Host "[extract] $($zip.Name) -> $outDir" -ForegroundColor Green
        Expand-Archive -LiteralPath $zip.FullName -DestinationPath $outDir -Force
        $extracted++
    } catch {
        Write-Warning "FAILED $($zip.Name): $_"
        $failed += $zip.Name
    }
}

Write-Host ""
Write-Host "Done. extracted=$extracted skipped=$skipped failed=$($failed.Count)" -ForegroundColor Cyan
if ($failed.Count -gt 0) { exit 1 }
