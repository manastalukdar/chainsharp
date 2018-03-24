#addin "Cake.Powershell"

var target = Argument("target", "Default");

Task("Default")
  .Does(() =>
{
  Information("Hello World!");
});

RunTarget(target);

var settings = new DotNetCoreBuildSettings
{
    Configuration = "Release"
};

DotNetCoreBuild("chainsharp.sln", settings);