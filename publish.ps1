$PublishPath = ".\src\SCD.Avalonia\bin\Publish"

mkdir -Force $PublishPath

dotnet publish .\SCD.sln -c Release -p:PublishProfile=win-x64 -v q --nologo
Compress-Archive -Force -Path $PublishPath\win-x64 -DestinationPath $PublishPath\win-x64
Remove-Item -Path $PublishPath\win-x64 -Force -Recurse -ErrorAction SilentlyContinue

dotnet publish .\SCD.sln -c Release -p:PublishProfile=win-x86 -v q --nologo
Compress-Archive -Force -Path $PublishPath\win-x86 -DestinationPath $PublishPath\win-x86
Remove-Item -Path $PublishPath\win-x86 -Force -Recurse -ErrorAction SilentlyContinue

dotnet publish .\SCD.sln -c Release -p:PublishProfile=win-arm64 -v q --nologo
Compress-Archive -Force -Path $PublishPath\win-arm64 -DestinationPath $PublishPath\win-arm64
Remove-Item -Path $PublishPath\win-arm64 -Force -Recurse -ErrorAction SilentlyContinue

dotnet publish .\SCD.sln -c Release -p:PublishProfile=win-arm -v q --nologo
Compress-Archive -Force -Path $PublishPath\win-arm -DestinationPath $PublishPath\win-arm
Remove-Item -Path $PublishPath\win-arm -Force -Recurse -ErrorAction SilentlyContinue

dotnet publish .\SCD.sln -c Release -p:PublishProfile=linux-x64 -v q --nologo
Compress-Archive -Force -Path $PublishPath\linux-x64 -DestinationPath $PublishPath\linux-x64
Remove-Item -Path $PublishPath\linux-x64 -Force -Recurse -ErrorAction SilentlyContinue

dotnet publish .\SCD.sln -c Release -p:PublishProfile=osx-x64 -v q --nologo
Compress-Archive -Force -Path $PublishPath\osx-x64 -DestinationPath $PublishPath\osx-x64
Remove-Item -Path $PublishPath\osx-x64 -Force -Recurse -ErrorAction SilentlyContinue

explorer.exe $PublishPath