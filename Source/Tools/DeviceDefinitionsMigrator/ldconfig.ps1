[xml] $doc = Get-Content openXDA.exe.config
$connectionString = $doc.SelectSingleNode("/configuration/categorizedSettings/systemSettings/add[@name='ConnectionString']").value
cmd /C "DeviceDefinitionsMigrator ""$connectionString"" DeviceDefinitions.xml"