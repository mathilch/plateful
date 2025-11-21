#!/usr/bin/env pwsh
param(
    # npm version bump type: patch | minor | major
    [ValidateSet("patch", "minor", "major")]
    [string]$VersionType = "patch",

    # How Orval should get the OpenAPI spec:
    # Http      -> temporarily change orval.config.js to use HTTP URL
    # Download  -> download swagger.json to openapi.json, keep config unchanged
    [ValidateSet("Http", "Download")]
    [string]$OrvalMode = "Download"
)

$ErrorActionPreference = "Stop"

Write-Host "=== Build & Publish events-api-sdk ===" -ForegroundColor Cyan

# Go to script directory (repo root)
if ($PSScriptRoot) {
    Set-Location $PSScriptRoot
}

$swaggerUrl  = "http://localhost:5121/swagger/v1/swagger.json"
$configPath  = "./orval.config.js"   # adjust to .ts if needed
$localSpec   = "./openapi.json"      # repo root; matches '../../../openapi.json' from the config

Write-Host "Step 1: Make sure Events service is running at $swaggerUrl (HTTP, no TLS)" -ForegroundColor Yellow
$answer = Read-Host "Is the service running? (y/n)"
if ($answer -notin @("y", "Y", "yes", "Yes")) {
    Write-Error "Please start the Events service and rerun the script."
    exit 1
}

if (-not (Test-Path $configPath)) {
    Write-Error "Config file '$configPath' not found. Adjust configPath in the script if needed."
    exit 1
}

switch ($OrvalMode) {
    'Http' {
        Write-Host "Step 2: Using HTTP URL in Orval config temporarily and running Orval" -ForegroundColor Cyan

        # Read original config so we can restore it
        $originalConfig = Get-Content $configPath -Raw

        # Make a backup just in case
        $backupPath = "$configPath.bak"
        Set-Content -Path $backupPath -Value $originalConfig

        # Strings to replace
        $localLine = "      target: '../../../openapi.json',"
        $httpLine  = "      target: 'http://localhost:5121/swagger/v1/swagger.json',"

        # Replace the target line
        $updatedConfig = $originalConfig -replace [Regex]::Escape($localLine), $httpLine

        if ($updatedConfig -eq $originalConfig) {
            Write-Warning "Did not find the expected local target line in $configPath. Orval may still use the original value."
        }

        try {
            Set-Content -Path $configPath -Value $updatedConfig
            orval --config $configPath
        }
        finally {
            # Restore original config
            Set-Content -Path $configPath -Value $originalConfig
            Write-Host "Orval config restored to original state." -ForegroundColor DarkGray
        }
    }

    'Download' {
        Write-Host "Step 2: Downloading OpenAPI spec to $localSpec and running Orval with unchanged config" -ForegroundColor Cyan

        Write-Host "Fetching $swaggerUrl ..." -ForegroundColor DarkCyan
        Invoke-WebRequest -Uri $swaggerUrl -OutFile $localSpec

        Write-Host "Running Orval with $configPath (input.target should point to '../../../openapi.json')" -ForegroundColor DarkCyan
        orval --config $configPath
    }
}

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
