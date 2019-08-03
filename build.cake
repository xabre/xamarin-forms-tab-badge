#addin nuget:?package=Cake.Git&version=0.21
#addin nuget:?package=Cake.FileHelpers&version=3.2.0

using Path = System.IO.Path;
using System.Xml.Linq;
using System.Xml;

var target = Argument("target", "Build");

var NuGetTargetDir = MakeAbsolute(Directory(".build/out/nuget"));
var BuildTargetDir = MakeAbsolute(Directory(".build/out/lib"));
var ProjectSources = MakeAbsolute(Directory("./Source"));
var NuspecFiles = new [] { ".build/Plugin.Badge.nuspec" };

string GetProjectDir(string projectName)
{
    return ProjectSources.Combine(projectName).CombineWithFilePath(projectName + ".csproj").FullPath;
}

void BuildProject(string projectName, string targetSubDir)
{
    Information("Building {0} ...", projectName);
    var project = GetProjectDir(projectName);
    var outputDir = BuildTargetDir.Combine(targetSubDir);
    MSBuild(project, settings => settings
            .SetConfiguration("Release")                                   
            .WithTarget("Build")
            .UseToolVersion(MSBuildToolVersion.VS2019)
            .SetMSBuildPlatform(MSBuildPlatform.x86)                        
            .WithProperty("OutDir", outputDir.FullPath));
}

Task("Restore")
    .Does(() =>
{	
    var solutions = GetFiles("./Source/*.sln");
    // Restore all NuGet packages.
    foreach(var solution in solutions)
    {
        Information("Restoring {0}", solution);
        NuGetRestore(solution);
    }
});

Task("Build")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .Does(() =>
{
    BuildProject("Plugin.Badge.Abstractions", "netstandard1.4");
    BuildProject("Plugin.Badge.Droid", "android");
    BuildProject("Plugin.Badge.iOS", "ios");
    BuildProject("Plugin.Badge.Mac", "mac");
    BuildProject("Plugin.Badge.UWP", "uwp");
    BuildProject("Plugin.Badge.WPF", "wpf");
});

Task("Clean").Does (() => 
{
    if (DirectoryExists (BuildTargetDir))
        DeleteDirectory (BuildTargetDir, true);

    CleanDirectories ("./**/bin");
    CleanDirectories ("./**/obj");
});

// ./build.ps1 -Target UpdateVersion -newVersion="2.0.1"       
Task("UpdateVersion")
   .Does(() => {
    var version = Argument<string>("newVersion", "");
    var cleanVersion = System.Text.RegularExpressions.Regex.Replace(version, @"[^\d\.].*$", "");

    if(string.IsNullOrEmpty(cleanVersion))
    {
        throw new ArgumentNullException(nameof(version));
    }
    
    ReplaceRegexInFiles("./**/AssemblyInfo.cs", "(?<=AssemblyVersion\\(\")(.+?)(?=\"\\))", cleanVersion);
    ReplaceRegexInFiles("./**/*.nuspec", "(?<=<version>)(.+?)(?=</version>)", cleanVersion);    
});

Task("Pack")
    .IsDependentOn("Build")
    .Does(() =>
    {    
        foreach(var nuspec in NuspecFiles)
        {
            NuGetPack(nuspec, new NuGetPackSettings()
            { 
                OutputDirectory = "./.build/nuget"
            });
        }        
    });

Task("Publish")
    .IsDependentOn("Pack")
    .Does(() =>
    {    
        var nupack = GetFiles(".build/nuget/*.nupkg").LastOrDefault();
        Information($"Pushing package: {nupack.FullPath}") ;       
        NuGetPush(nupack.FullPath, new NuGetPushSettings(){ Source = "https://nuget.org" });
    });

RunTarget(target);