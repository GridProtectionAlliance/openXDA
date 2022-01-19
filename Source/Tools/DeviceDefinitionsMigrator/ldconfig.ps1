param([string]$action)
[xml] $doc = Get-Content openXDA.exe.config
$connectionString = $doc.SelectSingleNode("/configuration/categorizedSettings/systemSettings/add[@name='ConnectionString']").value
cmd /C "DeviceDefinitionsMigrator $action ""$connectionString"" DeviceDefinitions.xml"