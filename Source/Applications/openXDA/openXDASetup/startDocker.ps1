
[Environment]::SetEnvironmentVariable("CompanyName", "$env:CompanyName", "Machine")
[Environment]::SetEnvironmentVariable("CompanyAcronym", "$env:CompanyAcronym", "Machine")
[Environment]::SetEnvironmentVariable("ConnectionString", "$env:ConnectionString", "Machine")
[Environment]::SetEnvironmentVariable("NodeID", "$env:NodeID", "Machine")

$ServiceName = 'OpenXDA';
$ConfigFile = 'C:/Program Files/OpenXDA/OpenXDA.StatusLog.txt'
Write-Host "Starting $ServiceName" 
net start $ServiceName
Write-Host "Started $ServiceName"

get-content "$ConfigFile" -tail 1 -wait
