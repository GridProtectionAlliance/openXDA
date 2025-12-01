
FROM mcr.microsoft.com/windows/servercore:ltsc2022

ENV sa_password="_" \
    sa_password_path="C:\ProgramData\Docker\secrets\sa-password" \
    openXDAUser="openXDA" \
    openXDAPassword="openXDA1" \
    onStartSQL="" \
    onStartSQLFile=""

ADD https://go.microsoft.com/fwlink/p/?linkid=2216019&clcid=0x409&culture=en-us&country=us c:/setup/
RUN C:/setup/SQL2025-SSEI-Expr.exe /Q /Action=Install /IAcceptSQLServerLicenseTerms

RUN POWERSHELL Set-itemproperty -path ('HKLM:\software\microsoft\microsoft sql server\mssql17.SQLEXPRESS\mssqlserver\supersocketnetlib\tcp\ipall') -name tcpdynamicports -value '' ; \
    Set-itemproperty -path ('HKLM:\software\microsoft\microsoft sql server\mssql17.SQLEXPRESS\mssqlserver\supersocketnetlib\tcp\ipall') -name tcpdynamicports -value '' ; \
    Set-itemproperty -path ('HKLM:\software\microsoft\microsoft sql server\mssql17.SQLEXPRESS\mssqlserver\supersocketnetlib\tcp\ipall') -name tcpport -value 1433 ; \
    Set-itemproperty -path ('HKLM:\software\microsoft\microsoft sql server\mssql17.SQLEXPRESS\mssqlserver') -name LoginMode -value 2;

RUN POWERSHELL SQLPS.EXE
RUN POWERSHELL Import-Module SQLPS; \
    $wmi = New-Object 'Microsoft.SqlServer.Management.Smo.Wmi.ManagedComputer' localhost; \
    $tcp = $wmi.ServerInstances['SQLEXPRESS'].ServerProtocols['Tcp']; \
    $tcp.IsEnabled = $true; \
    $tcp.Alter(); \
    Restart-Service -Name 'SQL Server (SQLEXPRESS)' -Force;

COPY ./startDocker.ps1 c:/scripts/
COPY ./*.sql c:/scripts/sql/
RUN POWERSHELL ls 'c:/scripts/sql/'
# Generate openXDA Database
RUN POWERSHELL Start-SERVICE -Name 'SQL Server (SQLEXPRESS)'; \
    sqlcmd -S 'localhost\SQLEXPRESS' -Q 'CREATE DATABASE [openXDA]'

RUN POWERSHELL Start-SERVICE -Name 'SQL Server (SQLEXPRESS)'; \
foreach ($file in Get-ChildItem 'c:/scripts/sql/') { \
    sqlcmd -S 'localhost\SQLEXPRESS' -d openXDA -i $file.FullName \
}

EXPOSE 1433/tcp
CMD ["powershell", "C:/scripts/startDocker.ps1"]
