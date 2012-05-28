properties {
    $mode = "Debug"
}

task default -depends Build

task Build {
    Exec { msbuild /p:Configuration=$mode ../src/Cloudinary.sln }
}

task Test -depends Build {
    Exec { .\nunit\nunit-console.exe ..\src\Cloudinary.Tests\bin\$mode\Cloudinary.Tests.dll /framework=4.0 }
}

