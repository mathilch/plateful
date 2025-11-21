#!/usr/bin/env pwsh
param(
    # npm version bump type: patch | minor | major
    [ValidateSet("patch", "minor", "major")]
    [string]$VersionType = "patch"
)

$ErrorActionPreference = "Stop"

Write-Host "=== Build & Publish events-api-sdk ===" -ForegroundColor Cyan

# Go to script directory (repo root)
if ($PSScriptRoot) {
    Set-Location $PSScriptRoot
}

Write-Host "Step 1: Make sure Events service is running at http://localhost:5121 (HTTP, no TLS)" -ForegroundColor Yellow
$answer = Read-Host "Is the service running? (y/n)"
if ($answer -notin @("y", "Y", "yes", "Yes")) {
    Write-Error "Please start the Events service and rerun the script."
    exit 1
}

Write-Host "Step 2: Running Orval with config .\orval.config.js" -ForegroundColor Cyan
orval --config .\orval.config.js

Write-Host "Step 3: Checking GITHUB_NODE_AUTH_TOKEN environment variable" -ForegroundColor Cyan
if (-not $env:GITHUB_NODE_AUTH_TOKEN) {
    Write-Error "GITHUB_NODE_AUTH_TOKEN is not set. Please set it before running:"
    Write-Host '  $env:GITHUB_NODE_AUTH_TOKEN = "<your_personal_access_token>"' -ForegroundColor Yellow
    exit 1
}

Write-Host "Step 4: Changing directory to ./src" -ForegroundColor Cyan
if (-not (Test-Path "./src")) {
    Write-Error "Folder ./src not found. Make sure you run this from the repo root."
    exit 1
}
Set-Location ./src

Write-Host "Step 5: Bumping npm version ($VersionType)" -ForegroundColor Cyan
npm version $VersionType

Write-Host "Step 6: Building SDK (npm run build)" -ForegroundColor Cyan
npm run build

Write-Host "Step 7: Publishing package (npm publish)" -ForegroundColor Cyan
npm publish

Write-Host "✅ Done! SDK built and published successfully." -ForegroundColor Green
