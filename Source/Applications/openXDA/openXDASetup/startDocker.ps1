
[Environment]::SetEnvironmentVariable("CompanyName", "$env:CompanyName", "Machine")
[Environment]::SetEnvironmentVariable("CompanyAcronym", "$env:CompanyAcronym", "Machine")
[Environment]::SetEnvironmentVariable("ConnectionString", "$env:ConnectionString", "Machine")
[Environment]::SetEnvironmentVariable("NodeID", "$env:NodeID", "Machine")


Write-Host "Starting OpenXDA"
$ServiceName = 'OpenXDA'; 
Restart-Service $ServiceName
Write-Host "Started OpenXDA"

get-content 'C:/Program Files/OpenXDA/OpenXDA.StatusLog.txt' -tail 1 -wait
