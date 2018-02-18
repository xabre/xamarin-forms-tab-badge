#addin "Cake.Xamarin"
#addin nuget:?package=Cake.Git

using Path = System.IO.Path;
using System.Xml.Linq;
using System.Xml;

var target = Argument("target", "Build");

var NuGetTargetDir = MakeAbsolute(Directory(".build/out/nuget"));
var BuildTargetDir = MakeAbsolute(Directory(".build/out/lib"));
var ProjectSources = MakeAbsolute(Directory("./Source"));
var NuspecFiles = new []{".build/Plugin.Badge.nuspec"};

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
            .SetMSBuildPlatform(MSBuildPlatform.x86)                        
            .WithProperty("OutDir", outputDir.FullPath));
}

// string NuVersionGet (string specFile)
// {
//     var doc = System.Xml.Linq.XDocument.Load(specFile);
//     var versionElements = doc.Descendants(XName.Get("version", doc.Root.Name.NamespaceName));
//     return versionElements.First().Value;
// }

// void NuVersionSet (string specFile, string version)
// {
//     var xmlDocument = System.Xml.Linq.XDocument.Load(specFile);
//     var nsmgr = new XmlNamespaceManager(new XmlNameTable());
//     nsmgr.AddNamespace("ns", "http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd");
//     var node = xmlDocument.Document.SelectSingleNode("//ns:version", nsmgr);
//     node.InnerText = version;
//     xmlDocument.Save(specFile);
// }

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
});

Task("Clean").Does (() => 
{
    if (DirectoryExists (BuildTargetDir))
        DeleteDirectory (BuildTargetDir, true);

    CleanDirectories ("./**/bin");
    CleanDirectories ("./**/obj");
});

Task("Version")
   .Does(() => {
    var version = Argument<string>("ver", "");
    var cleanVersion = System.Text.RegularExpressions.Regex.Replace(version, @"[^\d\.].*$", "");

    if(string.IsNullOrEmpty(cleanVersion))
    {
        throw new ArgumentNullException(nameof(version));
    }
    
    // ReplaceRegexInFiles("./your/AssemblyInfo.cs", "(?<=AssemblyVersion\\(\")(.+?)(?=\"\\))", cleanVersion);
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
    .IsDependentOn("Build")
    .Does(() =>
    {    
        var nupack = GetFiles("./.build/nuget/*.nuspec").FirstOrDefault();        
        NuGetPush(nupack.FullPath);
    });

RunTarget(target);