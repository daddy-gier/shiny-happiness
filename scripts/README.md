# scripts/

Windows PowerShell helpers for collecting your repos into `D:\git repos`.

## Prerequisites
- `git` on PATH
- GitHub CLI (`gh`) installed and authenticated: `gh auth login`

## Pull every GitHub repo you own
```powershell
.\scripts\Pull-AllGits.ps1
```
Clones missing repos into `D:\git repos\<repo-name>`, and runs `git pull --ff-only` on any that already exist.

Options:
- `-Destination "E:\code"` to target a different folder
- `-Owner my-org` to restrict to a specific user/org
- `-IncludeArchived` to include archived repos

## Extract the .zip archives already in this repo
```powershell
.\scripts\Expand-DownloadedZips.ps1
```
Unzips every `*.zip` at the repo root into `D:\git repos\<archive-name>\`. Skips folders that already exist; pass `-Force` to overwrite.

## Typical first-time run
```powershell
.\scripts\Expand-DownloadedZips.ps1
.\scripts\Pull-AllGits.ps1
```
