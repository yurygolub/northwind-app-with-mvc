param(
    [Parameter(Mandatory=$true)]
    [string] $Project,
    [string] $OS = 'win',
    [bool] $SingleFile = $true,
    [bool] $SelfContained = $true
)

if (!(Get-Command -ErrorAction Ignore -Type Application dotnet))
{
    Write-Warning 'dotnet was not found'
    Write-Host 'you have to install .NET SDK to build this application'
    Write-Host 'you can download it here https://dotnet.microsoft.com/en-us/download/dotnet/'
    return
}

$ErrorActionPreference = 'Stop'

if(Test-Path -Path $Project -PathType Leaf)
{
    $projectName = [System.IO.Directory]::GetParent($Project).Name
}
elseif(Test-Path -Path $Project -PathType Container)
{
    $projectName = (Get-Item $Project).Name
}
else
{
    Write-Warning "sprecified project '$Project' does not exist"
    return
}

$projects = 'NorthwindApiApp', 'NorthwindMvcClient'
if ($projects -notcontains $projectName)
{
    Write-Warning "'$Project' is not supported"
    Write-Host "Supported projects: $projects"
    return
}

$opSysems = 'win', 'linux'
if ($opSysems -notcontains $OS)
{
    Write-Warning "'$OS' is not supported"
    Write-Host "Available operating systems: $opSysems"
    return
}

$baseOutPath = 'publish'

$outPath = [System.IO.Path]::Combine($baseOutPath, $projectName, $OS)

if ($SingleFile)
{
    dotnet publish $projectName --configuration Release --output $outPath --os $OS --self-contained $SelfContained --property:DebugType=None --property:DebugSymbols=false --property:SatelliteResourceLanguages=ru --property:IncludeNativeLibrariesForSelfExtract=true --property:IsTransformWebConfigDisabled=true --property:GenerateDocumentationFile=false --property:PublishSingleFile=$SingleFile
}
else
{
    dotnet publish $projectName --configuration Release --output $outPath --os $OS --self-contained $SelfContained --property:DebugType=None --property:DebugSymbols=false --property:SatelliteResourceLanguages=ru
}
