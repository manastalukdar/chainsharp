
// Install addins.
#addin "Cake.Powershell"
#addin nuget:?package=Cake.Codecov

// Install tools.
#tool "nuget:https://api.nuget.org/v3/index.json?package=gitreleasemanager&version=0.7.0"
#tool "nuget:https://api.nuget.org/v3/index.json?package=GitVersion.CommandLine&version=3.6.2"
#tool "nuget:?package=OpenCover"
#tool "nuget:?package=xunit.runner.console"
#tool "nuget:?package=ReportGenerator"
#tool nuget:?package=Codecov

// Load other scripts.
// #load "./build/parameters.cake"

var artifactsDirectory = MakeAbsolute(Directory("./artifacts"));
var codeCoverageDirectory = MakeAbsolute(Directory("./artifacts/codecoverage"));
var codeCoverageFile = codeCoverageDirectory.CombineWithFilePath("coverage.xml");
var solutionPath = MakeAbsolute(new DirectoryPath("chainsharp.sln"));

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() => 
    {
        CleanDirectory(artifactsDirectory);
        CreateDirectory(codeCoverageDirectory);
        DotNetCoreClean(solutionPath.FullPath);
    });
    

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    DotNetCoreRestore("chainsharp.sln", new DotNetCoreRestoreSettings
    {
        Verbosity = DotNetCoreVerbosity.Minimal,
        Sources = new [] {
            "https://api.nuget.org/v3/index.json"
        },
        MSBuildSettings = null
    });
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    // Build the solution.
    DotNetCoreBuild(solutionPath.FullPath, new DotNetCoreBuildSettings()
    {
        ArgumentCustomization = args => args
            .Append("-p:DebugType=Full"),
        NoRestore = true
    });
});

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    var projects = GetFiles("./src/**/tests/**/*.csproj");
    foreach(var project in projects)
    {
        var filePath = new FilePath(solutionPath.FullPath);
        DotNetCoreTest(project.ToString(), new DotNetCoreTestSettings
        {
            Framework = "netcoreapp2.0",
            NoBuild = true,
            NoRestore = true,
            WorkingDirectory = filePath.GetDirectory()
        });
    }
});

Task("Calculate-Coverage")
    .IsDependentOn("Run-Unit-Tests")
    .Does(() =>
{
    if (IsRunningOnUnix())
    {
        Information("Running on Unix. Skipping task.");
        return;
    }

    var projects = GetFiles("../src/main/**/*.csproj");
    foreach(var project in projects)
    {
        TransformTextFile(project.FullPath, ">", "<")
            .WithToken("portable", ">full<")
            .Save(project.FullPath);
    }

    projects = GetFiles("./src/**/tests/**/*.csproj");
    foreach(var project in projects)
    {
        OpenCover(
                x => x.DotNetCoreTest(
                     project.FullPath,
                     new DotNetCoreTestSettings() //{ Configuration = "Debug" }
                     {
                        Framework = "netcoreapp2.0",
                        NoBuild = true,
                        NoRestore = true
                     }
                ),
                codeCoverageFile,
                new OpenCoverSettings()
                {
                    ArgumentCustomization = args => args
                        .Append("-threshold:100")
                        .Append("-returntargetcode")
                        .Append("-hideskipped:Filter;Attribute"),
                    Register = "user",
                    OldStyle = true,
                    MergeOutput = true
                }
                    .WithFilter("+[*]*")
                    .WithFilter("-[*tests*]*")
                    .ExcludeByAttribute("*.ExcludeFromCodeCoverage*")
            );
    }
});

Task("Generate-Report")
    .IsDependentOn("Calculate-Coverage")
    .Does(() =>
{
    if (IsRunningOnUnix())
    {
        Information("Running on Unix. Skipping task.");
        return;
    }

    ReportGenerator(codeCoverageFile, codeCoverageDirectory);
});

Task("Upload-Coverage")
    .IsDependentOn("Generate-Report")
    .Does(() =>
{
    if (IsRunningOnUnix())
    {
        Information("Running on Unix. Skipping task.");
        return;
    }

    Codecov(codeCoverageFile.ToString());
});

Task("Default")
    .IsDependentOn("Upload-Coverage")
    .Does(() =>
{
    Information("Done.");
});

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");

RunTarget(target);