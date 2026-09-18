#!/bin/bash
set -euo pipefail

# Accept the existing password options as well as the Linux image's standard option.
if [[ -z "${MSSQL_SA_PASSWORD:-}" ]]; then
    if [[ -n "${sa_password:-}" && "$sa_password" != "_" ]]; then
        export MSSQL_SA_PASSWORD="$sa_password"
    elif [[ -f "$sa_password_path" ]]; then
        export MSSQL_SA_PASSWORD="$(cat "$sa_password_path")"
    else
        echo "Set MSSQL_SA_PASSWORD, sa_password, or mount $sa_password_path." >&2
        exit 1
    fi
fi

export SQLCMDPASSWORD="$MSSQL_SA_PASSWORD"
sqlcmd=(/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -C -b -x)
rm -f /tmp/openxda-ready

/opt/mssql/bin/sqlservr &
sql_pid=$!
trap 'kill -TERM "$sql_pid" 2>/dev/null || true; wait "$sql_pid" || true' EXIT
trap 'exit 143' TERM
trap 'exit 130' INT

echo "Waiting for SQL Server"
ready=false
for ((attempt = 0; attempt < 60; attempt++)); do
    if ! kill -0 "$sql_pid" 2>/dev/null; then
        echo "SQL Server exited before initialization." >&2
        exit 1
    fi
    if "${sqlcmd[@]}" -l 2 -Q 'SELECT 1' > /dev/null 2>&1; then
        ready=true
        break
    fi
    sleep 2
done
if [[ "$ready" != true ]]; then
    echo "Timed out waiting for SQL Server." >&2
    exit 1
fi

if [[ ! -f /var/opt/mssql/openxda-initialized ]]; then
    "${sqlcmd[@]}" -Q 'CREATE DATABASE [openXDA]'
    for file in /scripts/sql/*.sql; do
        case "${file##*/}" in
            "03 - EnableEventDataSqlClr.sql"|"04 - EnableHistorianSqlClr.sql")
                echo "Skipping CLR setup: ${file##*/}"
                continue
                ;;
        esac
        echo "Running ${file##*/}"
        "${sqlcmd[@]}" -d openXDA -i "$file"
    done

    # Escape SQL identifiers and string literals independently.
    login="${openXDAUser//]/]]}"
    password="${openXDAPassword//\'/\'\'}"
    "${sqlcmd[@]}" -d openXDA -Q "CREATE LOGIN [$login] WITH PASSWORD = '$password'; CREATE USER [$login] FOR LOGIN [$login]; ALTER ROLE [db_owner] ADD MEMBER [$login];"
    touch /var/opt/mssql/openxda-initialized
fi

if [[ -n "$onStartSQL" ]]; then
    "${sqlcmd[@]}" -Q "$onStartSQL"
fi
if [[ -n "$onStartSQLFile" ]]; then
    "${sqlcmd[@]}" -d openXDA -i "$onStartSQLFile"
fi

touch /tmp/openxda-ready
echo "openXDA SQL initialization complete (CLR excluded)."
wait "$sql_pid"
