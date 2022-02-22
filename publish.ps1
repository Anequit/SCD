dotnet publish .\src\SCD.sln -c Release -p:PublishProfile=win-x64 -v q --nologo
dotnet publish .\src\SCD.sln -c Release -p:PublishProfile=win-x86 -v q --nologo
dotnet publish .\src\SCD.sln -c Release -p:PublishProfile=win-arm64 -v q --nologo
dotnet publish .\src\SCD.sln -c Release -p:PublishProfile=win-arm -v q --nologo
dotnet publish .\src\SCD.sln -c Release -p:PublishProfile=linux-x64 -v q --nologo
dotnet publish .\src\SCD.sln -c Release -p:PublishProfile=osx-x64 -v q --nologo
