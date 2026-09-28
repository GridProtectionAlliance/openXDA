FROM mcr.microsoft.com/mssql/server:2025-latest

ENV MSSQL_PID="Express" \
    sa_password_path="/run/secrets/sa-password" \
    openXDAUser="openXDA" \
    openXDAPassword="openXDA1" \
    onStartSQL="" \
    onStartSQLFile=""

COPY ./startDocker.sh /scripts/startDocker.sh
COPY ./*.sql /scripts/sql/

EXPOSE 1433/tcp
HEALTHCHECK --interval=5s --timeout=5s --start-period=120s --retries=12 \
    CMD test -f /tmp/openxda-ready
ENTRYPOINT ["/bin/bash", "/scripts/startDocker.sh"]
