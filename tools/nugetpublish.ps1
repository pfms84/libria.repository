$apikey = Read-Host -Prompt 'Input your nuget api key'

Write-Host "Publishing nuget packages..."

Get-ChildItem -Path ".." -Recurse | 
Where-Object {$_.FullName -like "*src*" -and $_.FullName -like "*bin\Release*" -and $_.Name -like "*.nupkg"} | 
ForEach-Object {
	$packageName = $_.FullName

    Write-Host "Publishing file $($packageName)..."

	dotnet nuget push $packageName -k $apikey -s https://api.nuget.org/v3/index.json
}