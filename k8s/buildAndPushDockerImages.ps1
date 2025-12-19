<#
.SYNOPSIS
    Builds and pushes Docker images for FoodClub frontend and Events API.

.DESCRIPTION
    This script should be run from the repository root directory.
    It builds and tags Docker images, then pushes them to GitHub Container Registry (ghcr.io).

.PREREQUISITES
    - Docker must be installed and running.
    - You must be authenticated to GitHub Container Registry (ghcr.io) before running this script. Otherwise, docker push will fail.

.HOW TO LOGIN TO GHCR
    The recommended way to login:
        1. Generate a Personal Access Token (PAT) with 'write:packages' scope from GitHub.
        2. Run this command, replacing <YOUR_PAT> and <YOUR_GITHUB_USERNAME> accordingly:

            echo <YOUR_PAT> | docker login ghcr.io -u <YOUR_GITHUB_USERNAME> --password-stdin

    This securely logs you in by passing the token via stdin, which is a best practice.
    See: https://docs.github.com/en/packages/working-with-a-github-packages-registry/working-with-the-container-registry

.USAGE
    Run from the repo root:
        ./k8s/buildAndPushDockerImages.ps1

    Optionally set your GitHub Node Auth Token (for private npm modules in frontend build):
        $env:GITHUB_NODE_AUTH_TOKEN = "<your_token>"
        ./k8s/buildAndPushDockerImages.ps1

.NOTES
    - If you get authentication errors when pushing images, check your ghcr login.
    - Frontend build may fail if your GITHUB_NODE_AUTH_TOKEN is missing and private npm packages are required.
#>

param(
    [string]$GithubNodeAuthToken = $env:GITHUB_NODE_AUTH_TOKEN
)

# Fail fast
$ErrorActionPreference = "Stop"

Write-Host "Building and pushing images for FoodClub..." -ForegroundColor Cyan

if (-not $GithubNodeAuthToken) {
    Write-Warning "GITHUB_NODE_AUTH_TOKEN is not set. Frontend build will fail if private npm packages are required."
}

# Determine repository root based on script location.
# Script is in the 'k8s' folder, so the repo root is its parent directory.
$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$root = Split-Path -Parent $scriptDir
Set-Location $root

# ---------------------------
# Frontend: ghcr.io/prvacy/foodclub-ui-next:latest
# ---------------------------
$frontendLocalTag  = "foodclub-ui-next:latest"
$frontendRemoteTag = "ghcr.io/prvacy/foodclub-ui-next:latest"
$frontendContext   = "src/foodclub-ui-next"
$frontendDockerfile = "Dockerfile"   # or "Dockerfile.dev" if you prefer

Write-Host "`n[Frontend] Building $frontendLocalTag from $frontendContext" -ForegroundColor Yellow

docker build `
  -f "$frontendContext/$frontendDockerfile" `
  --secret id=GITHUB_NODE_AUTH_TOKEN,env=GITHUB_NODE_AUTH_TOKEN `
  --build-arg NEXT_PUBLIC_USERS_API_BASE_URL="http://localhost:30002" `
  --build-arg NEXT_PUBLIC_EVENTS_API_BASE_URL="http://localhost:30001" `
  -t $frontendLocalTag `
  $frontendContext
if ($LASTEXITCODE -ne 0) {
    throw "Docker build failed for Frontend"
}

Write-Host "[Frontend] Tagging $frontendLocalTag -> $frontendRemoteTag"
docker tag $frontendLocalTag $frontendRemoteTag

Write-Host "[Frontend] Pushing $frontendRemoteTag"
docker push $frontendRemoteTag

# ---------------------------
# Events API: ghcr.io/prvacy/foodclub-events-api:latest
# ---------------------------
$eventsLocalTag  = "events-api:latest"
$eventsRemoteTag = "ghcr.io/prvacy/foodclub-events-api:latest"
$eventsContext   = "src/EventsMicroservice"
$eventsDockerfile = "Dockerfile"

Write-Host "`n[Events API] Building $eventsLocalTag from $eventsContext" -ForegroundColor Yellow

docker build `
  -f "$eventsContext/$eventsDockerfile" `
  -t $eventsLocalTag `
  $eventsContext
if ($LASTEXITCODE -ne 0) {
    throw "Docker build failed for Events API"
}

Write-Host "[Events API] Tagging $eventsLocalTag -> $eventsRemoteTag"
docker tag $eventsLocalTag $eventsRemoteTag

Write-Host "[Events API] Pushing $eventsRemoteTag"
docker push $eventsRemoteTag

# ---------------------------
# Users API: ghcr.io/prvacy/foodclub-users-api:latest
# ---------------------------
$usersLocalTag  = "users-api:latest"
$usersRemoteTag = "ghcr.io/prvacy/foodclub-users-api:latest"
$usersContext   = "src/UsersMicroservice"
$usersDockerfile = "Dockerfile"

Write-Host "`n[Users API] Building $usersLocalTag from $usersContext" -ForegroundColor Yellow

docker build `
  -f "$usersContext/$usersDockerfile" `
  -t $usersLocalTag `
  $usersContext
if ($LASTEXITCODE -ne 0) {
    throw "Docker build failed for Users API"
}

Write-Host "[Users API] Tagging $usersLocalTag -> $usersRemoteTag"
docker tag $usersLocalTag $usersRemoteTag

Write-Host "[Users API] Pushing $usersRemoteTag"
docker push $usersRemoteTag

# ---------------------------
# Chat API: ghcr.io/prvacy/foodclub-chat-api:latest
# ---------------------------
$chatLocalTag  = "chat-api:latest"
$chatRemoteTag = "ghcr.io/prvacy/foodclub-chat-api:latest"
$chatContext   = "src/ChatMicroservice"
$chatDockerfile = "Dockerfile"

Write-Host "`n[Chat API] Building $chatLocalTag from $chatContext" -ForegroundColor Yellow

docker build `
  -f "$chatContext/$chatDockerfile" `
  -t $chatLocalTag `
  $chatContext
if ($LASTEXITCODE -ne 0) {
    throw "Docker build failed for Chat API"
}

Write-Host "[Chat API] Tagging $chatLocalTag -> $chatRemoteTag"
docker tag $chatLocalTag $chatRemoteTag

Write-Host "[Chat API] Pushing $chatRemoteTag"
docker push $chatRemoteTag

Write-Host "`nAll images built and pushed successfully." -ForegroundColor Green