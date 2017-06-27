@echo off
".nuget/nuget.exe" install FAKE -Version 4.61.3

"FAKE.4.61.3/tools/FAKE.exe" build.fsx %*