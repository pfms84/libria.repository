Write-Host "Publishing nuget packages to local repo..."

Get-ChildItem -Path ".." -Recurse | 
Where-Object {$_.FullName -like "*src*" -and $_.FullName -like "*bin\Release*" -and $_.Name -like "*.nupkg"} | 
ForEach-Object {
	$packageName = $_.FullName

    Write-Host "Publishing file $($packageName)..."

	dotnet nuget push $packageName -s c:\nuget_repo
}