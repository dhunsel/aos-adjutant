#!/usr/bin/env bash
set -euo pipefail

export PGHOST PGPORT PGDATABASE PGUSER PGPASSWORD

for f in /migrations/*.sql; do
  echo "Applying $(basename "$f")"
  psql -v ON_ERROR_STOP=1 -f "$f" >/dev/null
done

echo "Migrations complete."
