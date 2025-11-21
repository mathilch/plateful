#!/usr/bin/env bash
set -e

# Default values
VERSION_TYPE="patch"
ORVAL_MODE="Http"

# Parse arguments
while [[ "$#" -gt 0 ]]; do
  case "$1" in
    --version-type)
      VERSION_TYPE="$2"
      shift 2;;
    --orval-mode)
      ORVAL_MODE="$2"
      shift 2;;
    *)
      echo "Unknown parameter: $1"
      exit 1;;
  esac
done

# Allowed version types
if [[ ! "$VERSION_TYPE" =~ ^(patch|minor|major)$ ]]; then
  echo "ERROR: --version-type must be patch, minor, or major"
  exit 1
fi

# Allowed Orval modes
if [[ ! "$ORVAL_MODE" =~ ^(Http|Download)$ ]]; then
  echo "ERROR: --orval-mode must be Http or Download"
  exit 1
fi

echo "=== Build & Publish events-api-sdk ==="
echo "Version type:  $VERSION_TYPE"
echo "Orval mode:    $ORVAL_MODE"

SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
cd "$SCRIPT_DIR"

SWAGGER_URL="http://localhost:5121/swagger/v1/swagger.json"
CONFIG_PATH="./orval.config.js"
LOCAL_SPEC="./openapi.json"
BACKUP_PATH="${CONFIG_PATH}.bak"

echo ""
echo "Step 1: Ensure events service is running at $SWAGGER_URL"
read -p "Is the service running? (y/n) " confirm
if [[ "$confirm" != "y" && "$confirm" != "Y" ]]; then
  echo "Please start the service first."
  exit 1
fi

if [[ "$ORVAL_MODE" == "Http" ]]; then
  echo ""
  echo "Step 2: Temporarily updating orval.config.js to use HTTP URL"

  if [[ ! -f "$CONFIG_PATH" ]]; then
    echo "ERROR: Config file not found: $CONFIG_PATH"
    exit 1
  fi

  cp "$CONFIG_PATH" "$BACKUP_PATH"

  LOCAL_LINE="      target: '../../../openapi.json',"
  HTTP_LINE="      target: 'http://localhost:5121/swagger/v1/swagger.json',"

  if grep -q "$LOCAL_LINE" "$CONFIG_PATH"; then
    sed -i.bak "s|$LOCAL_LINE|$HTTP_LINE|" "$CONFIG_PATH"
  else
    echo "WARNING: Expected line not found in config, Orval might not pick up URL."
  fi

  echo "Running Orval..."
  orval --config "$CONFIG_PATH"

  echo "Restoring original config..."
  mv "$BACKUP_PATH" "$CONFIG_PATH"

else
  echo ""
  echo "Step 2: Downloading OpenAPI spec to $LOCAL_SPEC and using unchanged config"
  
  echo "Downloading $SWAGGER_URL..."
  curl -sS "$SWAGGER_URL" -o "$LOCAL_SPEC"

  echo "Running Orval..."
  orval --config "$CONFIG_PATH"
fi

echo ""
echo "Step 3: Checking GITHUB_NODE_AUTH_TOKEN"
if [[ -z "$GITHUB_NODE_AUTH_TOKEN" ]]; then
  echo "ERROR: GITHUB_NODE_AUTH_TOKEN is not set."
  echo "Set it with:"
  echo '  export GITHUB_NODE_AUTH_TOKEN="your_token_here"'
  exit 1
fi

echo ""
echo "Step 4: Switching to ./src directory"
cd ./src || { echo "ERROR: ./src not found"; exit 1; }

echo ""
echo "Step 5: Bumping npm version: $VERSION_TYPE"
npm version "$VERSION_TYPE"

echo ""
echo "Step 6: Building SDK"
npm run build

echo ""
echo "Step 7: Publishing package"
npm publish

echo ""
echo "✅ Done! SDK built and published."
