properties {
    $mode = "Debug"
}

$config = New-Object PSObject -property @{ version="0.0.1";releaseNotes="Initial release" }

task default -depends Build

task Build {
    Exec { msbuild /p:Configuration=$mode ../src/Cloudinary.sln }
}

task Test -depends Build {
    Exec { .\nunit\nunit-console.exe ..\src\Cloudinary.Tests\bin\$mode\Cloudinary.Tests.dll }
    Remove-Item TestResult.xml
}

task Package {
    # Force release build
    $mode = "Release"
    Invoke-Task Build
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

function SetNuSpecVersionInfo {
    (Get-Content "Cloudinary.nuspec") | Foreach-Object {
        $_ -replace 'PKG_VERSION', $config.version `
           -replace 'PKG_RELEASENOTES', $config.releaseNotes
        } | Set-Content "pkg\Cloudinary.nuspec"
}

