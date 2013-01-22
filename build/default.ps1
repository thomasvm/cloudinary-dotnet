properties {
    $mode = "Debug"
}

$config = New-Object PSObject -property @{ 
    version="0.0.5";
    releaseNotes=@"
No new features, simply pointing out that there now is an official Cloudinary package: CloudinaryDotNet
"@
}

task default -depends Build

task GenerateAssemblyInfo  {
    $fullVersion = "$($config.version).$(Get-GitCommitNumber)"
    $informationalVersion = "$($fullVersion).$(Get-GitCommitHash)"

    Generate-Assembly-Info `
            -clsCompliant "false" `
            -product      "Cloudinary" `
            -copyright    "Copyright © Thomas Van Machelen $((Get-Date).Year)" `
            -version      "$fullVersion" `
            -informationalVersion "$informationalVersion" `
            -file         "..\src\CommonAssemblyInfo.cs" `
}

task Build -depends GenerateAssemblyInfo {
    Exec { msbuild /p:Configuration=$mode ../src/Cloudinary.sln }
}

task Test -depends Build {
    Exec { .\nunit\nunit-console.exe ..\src\Cloudinary.Tests\bin\$mode\Cloudinary.Tests.dll }
    Remove-Item TestResult.xml
}

task Package {
    # Force release build
    $mode = "Release"
    Invoke-Task Test

    # Create pkg
    If(Test-Path pkg) {
        Remove-Item -Recurse pkg
    }
    New-Item -type directory pkg
    New-Item -type directory pkg\lib
    Copy-Item ..\src\Cloudinary\bin\$mode\Cloudinary.dll pkg\lib

    SetNuSpecVersionInfo

    Exec {
        ..\src\.nuget\nuget.exe pack pkg\Cloudinary.nuspec /BasePath pkg
    }

    Remove-Item -Recurse pkg
}

task Push -depends Package {
    Exec { ..\src\.nuget\nuget.exe push "Cloudinary.$($config.version).nupkg" }
}

function SetNuSpecVersionInfo {
    (Get-Content "Cloudinary.nuspec") | Foreach-Object {
        $_ -replace 'PKG_VERSION', $config.version `
           -replace 'PKG_RELEASENOTES', $config.releaseNotes
        } | Set-Content "pkg\Cloudinary.nuspec"
}

function Get-GitCommitHash
{
    $gitLog = git log --oneline -1
    return $gitLog.Split(' ')[0]
}

function Get-GitCommitNumber
{
    $count = git log --oneline | wc -l
    return $count.Trim()
}

function Generate-Assembly-Info
{
param(
    [string]$clsCompliant = "true",
    [string]$company, 
    [string]$product, 
    [string]$copyright, 
    [string]$version,
    [string]$informationalVersion,
    [string]$file = $(throw "file is a required parameter.")
)
  $temp = "$file`_temp"

  $asmInfo = "using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: CLSCompliantAttribute($clsCompliant)]
[assembly: ComVisibleAttribute(false)]
[assembly: AssemblyCompanyAttribute(""$company"")]
[assembly: AssemblyProductAttribute(""$product"")]
[assembly: AssemblyCopyrightAttribute(""$copyright"")]
[assembly: AssemblyVersionAttribute(""$version"")]
[assembly: AssemblyInformationalVersionAttribute(""$informationalVersion"")]
[assembly: AssemblyFileVersionAttribute(""$version"")]"

    $dir = [System.IO.Path]::GetDirectoryName($file)
    if ((Test-Path $dir) -eq $FALSE)
    {
        Write-Host "Creating directory $dir"
        New-Item -type Directory -Name $dir
    }

    Write-Host "Generating assembly info file: $file"
    # Only write when changed; by doing so the build isn't triggered each time
    If (Test-Path $file) {
        out-file -filePath $temp -encoding UTF8 -inputObject $asmInfo

        $old = Get-Content $file
        $new = Get-Content $temp

        $result = Compare-Object $old $new

        If ($result -eq $null) {
            Remove-Item $temp
        } Else {
            Move-Item $temp $file -Force
        }
    } Else {
        out-file -filePath $file -encoding UTF8 -inputObject $asmInfo
    }
}

