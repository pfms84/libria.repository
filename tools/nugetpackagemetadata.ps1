$ErrorActionPreference = "Stop"

$major = 1
$minor = 0
$patch = '0-alpha4'

$packageVersion = "$($major).$($minor).$($patch)";

Write-Host "Updating project versions..."

$packageVersionText = "<Version>$($packageVersion)</Version>"

Get-ChildItem -Path ".." -Recurse | 
Where-Object {$_.FullName -like "*src*" -and $_.Name -like "*.csproj"} | 
ForEach-Object {

    $csprojFilePath = $_.FullName

    Write-Host "Changing versions in file $($csprojFilePath)..."
    $csprojContent = Get-Content $csprojFilePath -Raw -Encoding UTF8

    $csprojContent = $csprojContent -replace '<Version>[0-9a-zA-Z.-]+</Version>', $packageVersionText

    $csprojContent.Trim() | Set-Content $csprojFilePath -Encoding UTF8
}