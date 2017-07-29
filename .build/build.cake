#addin "Cake.Xamarin"

using Path = System.IO.Path;

var target = Argument("target", "Build");

var NuGetTargetDir =  Path.Combine("out" ,"nuget");
var BuildTargetDir = Path.Combine(Environment.CurrentDirectory, "out" ,"lib");
var ProjectSources = Path.Combine("..", "Source");
var NuspecFiles = new []{"Plugin.Badge.nuspec"};

string GetProjectDir(string projectName)
{
    return Path.Combine(ProjectSources, projectName, projectName + ".csproj");
}

void BuildProject(string projectName, string targetSubDir)
{
    Information("Building {0} ...", projectName);
    var project = GetProjectDir(projectName);
    var outputDir = Path.Combine(BuildTargetDir, targetSubDir);
    MSBuild(project, settings => settings
            .SetConfiguration("Release")           
            .WithTarget("Build")
            .WithProperty("outputPath", outputDir));
}

Task("Restore")
    .Does(() =>
{	
    var solutions = GetFiles("../Source/*.sln");
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
    BuildProject("Plugin.Badge.Abstractions", "pcl");
    BuildProject("Plugin.Badge.Droid", "android");
    BuildProject("Plugin.Badge.iOS", "ios");
});

Task ("Clean").Does (() => 
{
    if (DirectoryExists (BuildTargetDir))
        DeleteDirectory (BuildTargetDir, true);

    CleanDirectories ("./**/bin");
    CleanDirectories ("./**/obj");
});


RunTarget(target);