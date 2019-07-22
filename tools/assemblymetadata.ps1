$ErrorActionPreference = "Stop"

$major = 1
$minor = 0
$patch = 1

$assemblyVersion = "$($major).$($minor).$($patch)" 
$assemblyFileVersion = "$($major).$($minor).$($patch).19001"
$assemblyInformationalVersion = "$($major).$($minor).$($patch)"

Write-Host "Updating project versions..."

$assemblyVersionText = "<AssemblyVersion>$($assemblyVersion)</AssemblyVersion>"
$assemblyFileVersionText = "<FileVersion>$($assemblyFileVersion)</FileVersion>"
$assemblyInformationalVersionText = "<VersionPrefix>$($assemblyInformationalVersion)</VersionPrefix>"

Get-ChildItem -Path ".." -Recurse | 
Where-Object {$_.FullName -like "*src*" -and $_.Name -like "*.csproj"} | 
ForEach-Object {

    $csprojFilePath = $_.FullName

    Write-Host "Changing versions in file $($csprojFilePath)..."
    $csprojContent = Get-Content $csprojFilePath -Raw -Encoding UTF8

    $csprojContent = $csprojContent -replace '<AssemblyVersion>[0-9.]+</AssemblyVersion>', $assemblyVersionText
    $csprojContent = $csprojContent -replace '<FileVersion>[0-9.]+</FileVersion>', $assemblyFileVersionText 
    $csprojContent = $csprojContent -replace '<VersionPrefix>[0-9a-zA-Z.-]+</VersionPrefix>', $assemblyInformationalVersionText

    $csprojContent.Trim() | Set-Content $csprojFilePath -Encoding UTF8
}