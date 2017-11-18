@echo off
".nuget/nuget.exe" install FAKE -Version 4.63.2

"FAKE.4.63.2/tools/FAKE.exe" build.fsx %*