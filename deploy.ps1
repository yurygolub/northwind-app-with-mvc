param(
    [Parameter(Mandatory=$true)]
    [string] $ArtifactsFolder,
    [Parameter(Mandatory=$true)]
    [string] $ExecutablePath,
    [string] $InstallLocation,
    [string] $ServiceName,
    [string] $ServiceDescription
)

if(!(Test-Path -Path $ArtifactsFolder -PathType Container))
{
    Write-Warning "specified folder '$ArtifactsFolder' does not exist"
    return
}

if(!(Test-Path -Path $ExecutablePath))
{
    Write-Warning "specified file '$ExecutablePath' does not exist"
    return
}

$Executable = (Get-Item $ExecutablePath).Name

if(!(Test-Path -Path $(Join-Path -Path $ArtifactsFolder -ChildPath $Executable)))
{
    Write-Warning "'$Executable' does not exist in $ArtifactsFolder"
    return
}

$appName = (Get-Item $ExecutablePath).BaseName

if (!$InstallLocation)
{
    $InstallLocation = Join-Path -Path 'C:\' -ChildPath $appName
}

if (!$ServiceName)
{
    $ServiceName = $appName
}

if (!$ServiceDescription)
{
    $ServiceDescription = $ServiceName
}

$service = Get-Service $ServiceName -ErrorAction Ignore
if ($service)
{
    if ($service.Status -eq 'Running')
    {
        Stop-Service $appName
    }

    Remove-Service $appName
}

if (Test-Path $InstallLocation)
{
    Get-ChildItem $InstallLocation -Exclude logs | Remove-Item -Recurse
}

New-Item $InstallLocation -Type Directory -Force

Copy-Item "$ArtifactsFolder/*" $InstallLocation -Recurse

$binaryPath = Join-Path -Path $InstallLocation -ChildPath $Executable

New-Service $ServiceName -BinaryPathName $binaryPath -StartupType Automatic -Description $ServiceDescription

Start-Service $ServiceName
