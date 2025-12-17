#This script was not tested, please use ps1 version if there are issues.

#!/usr/bin/env bash
set -euo pipefail

########################################
# Builds and pushes Docker images for FoodClub
# Mirrors k8s/buildAndPushDockerImages.ps1
########################################

echo "Building and pushing images for FoodClub..."

# Frontend needs this for private npm packages
if [[ -z "${GITHUB_NODE_AUTH_TOKEN:-}" ]]; then
  echo "WARNING: GITHUB_NODE_AUTH_TOKEN is not set. Frontend build will fail if private npm packages are required." >&2
fi

# Determine repository root based on script location.
# Script is in the 'k8s' folder, so the repo root is its parent directory.
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ROOT_DIR="$(cd "${SCRIPT_DIR}/.." && pwd)"
cd "${ROOT_DIR}"

########################################
# Frontend: ghcr.io/prvacy/foodclub-ui-next:latest
########################################
FRONTEND_LOCAL_TAG="foodclub-ui-next:latest"
FRONTEND_REMOTE_TAG="ghcr.io/prvacy/foodclub-ui-next:latest"
FRONTEND_CONTEXT="src/foodclub-ui-next"
FRONTEND_DOCKERFILE="Dockerfile"   # or Dockerfile.dev if you prefer

echo
echo "[Frontend] Building ${FRONTEND_LOCAL_TAG} from ${FRONTEND_CONTEXT}"

docker build \
  -f "${FRONTEND_CONTEXT}/${FRONTEND_DOCKERFILE}" \
  --secret id=GITHUB_NODE_AUTH_TOKEN,env=GITHUB_NODE_AUTH_TOKEN \
  --build-arg NEXT_PUBLIC_USERS_API_BASE_URL="http://localhost:30002" \
  --build-arg NEXT_PUBLIC_EVENTS_API_BASE_URL="http://localhost:30001" \
  -t "${FRONTEND_LOCAL_TAG}" \
  "${FRONTEND_CONTEXT}"

echo "[Frontend] Tagging ${FRONTEND_LOCAL_TAG} -> ${FRONTEND_REMOTE_TAG}"
docker tag "${FRONTEND_LOCAL_TAG}" "${FRONTEND_REMOTE_TAG}"

echo "[Frontend] Pushing ${FRONTEND_REMOTE_TAG}"
docker push "${FRONTEND_REMOTE_TAG}"

########################################
# Events API: ghcr.io/prvacy/foodclub-events-api:latest
########################################
EVENTS_LOCAL_TAG="events-api:latest"
EVENTS_REMOTE_TAG="ghcr.io/prvacy/foodclub-events-api:latest"
EVENTS_CONTEXT="src/EventsMicroservice"
EVENTS_DOCKERFILE="Dockerfile"

echo
echo "[Events API] Building ${EVENTS_LOCAL_TAG} from ${EVENTS_CONTEXT}"

docker build \
  -f "${EVENTS_CONTEXT}/${EVENTS_DOCKERFILE}" \
  -t "${EVENTS_LOCAL_TAG}" \
  "${EVENTS_CONTEXT}"

echo "[Events API] Tagging ${EVENTS_LOCAL_TAG} -> ${EVENTS_REMOTE_TAG}"
docker tag "${EVENTS_LOCAL_TAG}" "${EVENTS_REMOTE_TAG}"

echo "[Events API] Pushing ${EVENTS_REMOTE_TAG}"
docker push "${EVENTS_REMOTE_TAG}"

########################################
# Users API: ghcr.io/prvacy/foodclub-users-api:latest
########################################
USERS_LOCAL_TAG="users-api:latest"
USERS_REMOTE_TAG="ghcr.io/prvacy/foodclub-users-api:latest"
USERS_CONTEXT="src/UsersMicroservice"
USERS_DOCKERFILE="Dockerfile"

echo
echo "[Users API] Building ${USERS_LOCAL_TAG} from ${USERS_CONTEXT}"

docker build \
  -f "${USERS_CONTEXT}/${USERS_DOCKERFILE}" \
  -t "${USERS_LOCAL_TAG}" \
  "${USERS_CONTEXT}"

echo "[Users API] Tagging ${USERS_LOCAL_TAG} -> ${USERS_REMOTE_TAG}"
docker tag "${USERS_LOCAL_TAG}" "${USERS_REMOTE_TAG}"

echo "[Users API] Pushing ${USERS_REMOTE_TAG}"
docker push "${USERS_REMOTE_TAG}"

########################################
# Chat API: ghcr.io/prvacy/foodclub-chat-api:latest
########################################
CHAT_LOCAL_TAG="chat-api:latest"
CHAT_REMOTE_TAG="ghcr.io/prvacy/foodclub-chat-api:latest"
CHAT_CONTEXT="src/ChatMicroservice"
CHAT_DOCKERFILE="Dockerfile"

echo
echo "[Chat API] Building ${CHAT_LOCAL_TAG} from ${CHAT_CONTEXT}"

docker build \
  -f "${CHAT_CONTEXT}/${CHAT_DOCKERFILE}" \
  -t "${CHAT_LOCAL_TAG}" \
  "${CHAT_CONTEXT}"

echo "[Chat API] Tagging ${CHAT_LOCAL_TAG} -> ${CHAT_REMOTE_TAG}"
docker tag "${CHAT_LOCAL_TAG}" "${CHAT_REMOTE_TAG}"

echo "[Chat API] Pushing ${CHAT_REMOTE_TAG}"
docker push "${CHAT_REMOTE_TAG}"

echo
echo "All images built and pushed successfully."


