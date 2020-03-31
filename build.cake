var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var artifactsDir = (DirectoryPath)Directory("./artifacts");

Task("Clean")
    .Does(() =>
{
    CleanDirectories(new[] { "./src/TestLibrary/bin", "./src/TestLibrary/obj", artifactsDir });
});

Task("Restore")
    .IsDependentOn("Clean")
    .Does(() =>
{
    DotNetCoreRestore("./src/TestLibrary/TestLibrary.csproj");
});

Task("Build")
    .IsDependentOn("Restore")
    .Does(() =>
{
    DotNetCoreBuild("./src/TestLibrary/TestLibrary.csproj", new DotNetCoreBuildSettings {
        Configuration = configuration,
        NoRestore = true,
    });
});

Task("Pack")
    .IsDependentOn("Build")
    .Does(() =>
{
    DotNetCorePack("./src/TestLibrary/TestLibrary.csproj", new DotNetCorePackSettings {
        Configuration = configuration,
        NoBuild = true,
        OutputDirectory = artifactsDir.Combine("nuget")
    });
});

Task("Default")
    .IsDependentOn("Pack");

RunTarget(target);