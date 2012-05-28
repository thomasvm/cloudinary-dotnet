properties {
    $mode = "Debug"
}

task default -depends Build

task Build {
    Exec { msbuild /p:Configuration=$mode ../src/Cloudinary.sln }
}

