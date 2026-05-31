#!/usr/bin/env bash
set -euo pipefail

MIG_NAME=$1

cd "$(dirname "${BASH_SOURCE[0]}")/.."
REPO_ROOT=$(pwd)
API_DIR="$REPO_ROOT/backend/src/AosAdjutant.Api"
MIG_DIR="$API_DIR/Migrations/Sql"

PREV_MIG=$(dotnet ef migrations list --project $API_DIR --no-build --no-connect --json \
  | jq -r '.[-1].id // "0"')

dotnet ef migrations add "$MIG_NAME" --project $API_DIR >/dev/null

dotnet build $API_DIR >/dev/null

NEW_MIG=$(dotnet ef migrations list --project $API_DIR --no-build --no-connect --json \
  | jq -r '.[-1].id')

dotnet ef migrations script "$PREV_MIG" "$NEW_MIG" \
  --idempotent \
  --project $API_DIR \
  --output "$MIG_DIR/${NEW_MIG}.sql" >/dev/null

echo "Migration files $NEW_MIG added in $API_DIR/Migrations and $MIG_DIR"
