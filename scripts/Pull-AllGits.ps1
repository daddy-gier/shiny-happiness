<#
.SYNOPSIS
    Clone or update every GitHub repo you own/collaborate on into a target folder.

.DESCRIPTION
    For each repo returned by `gh repo list`:
      - If the folder already exists with a .git directory, run `git pull --ff-only`.
      - Otherwise, clone it fresh.

    Requires the GitHub CLI (`gh`) authenticated as the target user, and `git`.

.PARAMETER Destination
    Target root folder. Defaults to "D:\git repos".

.PARAMETER Owner
    Optional GitHub owner/org to filter to. Defaults to the authenticated user's repos.

.PARAMETER Limit
    Max repos to enumerate. Defaults to 1000.

.PARAMETER IncludeArchived
    Include archived repos (skipped by default).

.EXAMPLE
    .\Pull-AllGits.ps1
    .\Pull-AllGits.ps1 -Destination "D:\git repos" -Owner my-org
#>
[CmdletBinding()]
param(
    [string]$Destination = "D:\git repos",
    [string]$Owner,
    [int]$Limit = 1000,
    [switch]$IncludeArchived
)

$ErrorActionPreference = "Stop"

function Require-Cmd($name) {
    if (-not (Get-Command $name -ErrorAction SilentlyContinue)) {
        throw "$name is required but was not found in PATH."
    }
}

Require-Cmd git
Require-Cmd gh

if (-not (Test-Path -LiteralPath $Destination)) {
    New-Item -ItemType Directory -Path $Destination -Force | Out-Null
}

$ghArgs = @("repo", "list", "--limit", $Limit, "--json", "nameWithOwner,isArchived,sshUrl")
if ($Owner) { $ghArgs += $Owner }

$json = & gh @ghArgs
if ($LASTEXITCODE -ne 0) { throw "gh repo list failed." }

$repos = $json | ConvertFrom-Json
if (-not $IncludeArchived) {
    $repos = $repos | Where-Object { -not $_.isArchived }
}

$total   = $repos.Count
$cloned  = 0
$updated = 0
$failed  = @()

Write-Host "Found $total repos. Target: $Destination" -ForegroundColor Cyan

foreach ($r in $repos) {
    $name    = ($r.nameWithOwner -split "/")[-1]
    $repoDir = Join-Path $Destination $name

    try {
        if (Test-Path -LiteralPath (Join-Path $repoDir ".git")) {
            Write-Host "[pull]  $($r.nameWithOwner)" -ForegroundColor Yellow
            Push-Location -LiteralPath $repoDir
            try {
                & git fetch --all --prune | Out-Null
                & git pull --ff-only
                if ($LASTEXITCODE -ne 0) { throw "git pull failed in $repoDir" }
                $updated++
            } finally { Pop-Location }
        } else {
            Write-Host "[clone] $($r.nameWithOwner)" -ForegroundColor Green
            & gh repo clone $r.nameWithOwner $repoDir
            if ($LASTEXITCODE -ne 0) { throw "gh repo clone failed for $($r.nameWithOwner)" }
            $cloned++
        }
    } catch {
        Write-Warning "FAILED $($r.nameWithOwner): $_"
        $failed += $r.nameWithOwner
    }
}

Write-Host ""
Write-Host "Done. cloned=$cloned updated=$updated failed=$($failed.Count) total=$total" -ForegroundColor Cyan
if ($failed.Count -gt 0) {
    Write-Host "Failures:" -ForegroundColor Red
    $failed | ForEach-Object { Write-Host "  $_" }
    exit 1
}
