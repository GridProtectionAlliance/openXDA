
$sa_password = $env:sa_password
$openXDAUser = $env:openXDAUser
$openXDAPassword = $env:openXDAPassword

# start the service
Write-Host "Starting SQL Server"
$SqlServiceName = 'SQL Server (SQLEXPRESS)'; 

start-service $SqlServiceName

Write-Host "Started SQL Server."

if($sa_password -eq "_") {
    if (Test-Path $env:sa_password_path) {
        $sa_password = Get-Content -Raw $secretPath
    }
    else {
        Write-Host "WARN: Using default SA password, secret file not found at: $secretPath"
        
    }
}

if($sa_password -ne "_")
{
    Write-Host "Changing SA login credentials"
    $sqlcmd = "ALTER LOGIN sa with password=" +"'" + $sa_password + "'" + ";ALTER LOGIN sa ENABLE;"
    sqlcmd -S "localhost\SQLEXPRESS" -Q $sqlcmd
}

Write-Host "Generating openXDA login credentials"
$sqlcmd = "CREATE LOGIN " + $openXDAUser + " with password=" +"'" + $openXDAPassword + "'" + "; CREATE USER " + $openXDAUser + " FOR LOGIN " + $openXDAUser +";  "
sqlcmd -S "localhost\SQLEXPRESS" -Q $sqlcmd
$sqlcmd = "Use openXDA; EXEC sp_AddUser '" + $openXDAUser + "', '" + $openXDAUser + "', 'db_owner';"
sqlcmd -S "localhost\SQLEXPRESS" -Q $sqlcmd

$lastCheck = (Get-Date).AddSeconds(-2) 
while ($true) 
{ 
    Get-EventLog -LogName Application -Source "MSSQL*" -After $lastCheck | Select-Object TimeGenerated, EntryType, Message	 
    $lastCheck = Get-Date 
    Start-Sleep -Seconds 2 
}