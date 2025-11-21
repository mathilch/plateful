#!/usr/bin/env bash
set -e

echo "=== Step 1: Ensure events service is running at http://localhost:5121 ==="
read -p "Press ENTER to continue when the service is running..."

echo "=== Step 2: Running Orval code generation ==="
orval --config ./orval.config.js

echo "=== Step 3: Checking GitHub token ==="
if [ -z "$GITHUB_NODE_AUTH_TOKEN" ]; then
  echo "ERROR: GITHUB_NODE_AUTH_TOKEN is not set."
  echo "Please export it before running this script:"
  echo "export GITHUB_NODE_AUTH_TOKEN=your_token_here"
  exit 1
fi

echo "=== Step 4: Switching to /src directory ==="
cd src

echo "=== Step 5: Bumping version (patch) ==="
npm version patch

echo "=== Step 6: Building SDK ==="
npm run build

echo "=== Step 7: Publishing ==="
npm publish

echo "=== Done! Package built and published. ==="
