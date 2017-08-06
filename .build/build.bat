@echo off
".nuget/nuget.exe" install FAKE -Version 4.63.0

"FAKE.4.63.0/tools/FAKE.exe" build.fsx %*