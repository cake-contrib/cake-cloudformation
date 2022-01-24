
using Cake.Common;
using Cake.Common.Diagnostics;
using Cake.Common.IO;
using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNetCore;
using Cake.Common.Tools.DotNetCore.Build;
using Cake.Common.Tools.DotNetCore.MSBuild;
using Cake.Common.Tools.DotNetCore.NuGet.Push;
using Cake.Common.Tools.DotNetCore.Pack;
using Cake.Common.Tools.DotNetCore.Test;
using Cake.Common.Tools.GitVersion;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Frosting;
using System;
using System.Collections.Generic;
public class Program
{
    public static int Main(string[] args)
    {
        return new CakeHost()
            .UseContext<BuildContext>()
            .UseWorkingDirectory("..")
            .InstallTools()
            .Run(args);
    }
}



public static class ToolsInstaller
{
    public static CakeHost InstallTools(this CakeHost host)
    {
        host.SetToolPath($"./caketools");
        host.InstallTool(new Uri("nuget:?package=GitVersion.Tool&version=5.8.1"));
        host.InstallTool(new Uri("nuget:?package=NuGet.CommandLine&version=5.11.0"));


        return host;
    }
}

public class BuildContext : FrostingContext
{
    public string MsBuildConfiguration { get; internal set; }
    public string SolutionFile { get; internal set; }
    public string? ApplicationVersion { get; internal set; }
    public DirectoryPath ArtifactsFolder { get; internal set; }
    public DirectoryPath PublishFolder { get; internal set; }

    public BuildContext(ICakeContext context)
        : base(context)
    {
        MsBuildConfiguration = context.Argument("configuration", "Release");
        SolutionFile = "cake-cloudformation.sln";
        ArtifactsFolder = "artifacts";
        PublishFolder = "publish";
    }
}

[TaskName("Clean")]
public sealed class CleanTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.Log.Information("Cleaning............");
        context.DotNetClean("");
    }
}

[TaskName("Version")]
public sealed class VersionTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        try
        {
            CalculateVersion();
        }
        catch (Exception ex)
        {
            context.Warning($"Error Calculating GitVersion, Retry is in progress, Exception:{ex.Message}");
            CalculateVersion();
        }

        void CalculateVersion()
        {
            var version = context.GitVersion(new GitVersionSettings
            {
                Verbosity = GitVersionVerbosity.Normal,
                NoFetch = true
            });
            Console.WriteLine($"Version : {version.NuGetVersionV2}");
            context.ApplicationVersion = version.NuGetVersionV2;
        }
    }
}

[TaskName("Build")]
public sealed class BuildTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.Log.Information("building............");
        context.DotNetBuild(context.SolutionFile, new DotNetCoreBuildSettings
        {
            Configuration = context.MsBuildConfiguration
        });
    }
}

[TaskName("Test")]
public sealed class TestTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.Log.Information("building............");
        context.DotNetTest(context.SolutionFile, new DotNetCoreTestSettings
        {
            Configuration = context.MsBuildConfiguration
        });

    }
}

 

[TaskName("Package")]
public sealed class PackageTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        string repoUrl = context.EnvironmentVariable("REPO_URL", "http://temp.org");
        
        var msBuildSettings = new DotNetCoreMSBuildSettings();
        msBuildSettings.Properties.Add("PackageVersion", new List<string> { context.ApplicationVersion });
        context.DotNetPack("src/Cake.CloudFormation/Cake.CloudFormation.csproj", new DotNetCorePackSettings
        {
            OutputDirectory = context.ArtifactsFolder,
            MSBuildSettings = msBuildSettings,
            Configuration = context.MsBuildConfiguration,
            Verbosity = DotNetCoreVerbosity.Normal
        });
    }
}

[TaskName("NugetPush")]
public sealed class NugetPushTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        var packges = context.GetFiles(context.ArtifactsFolder + "/*.nupkg");

        string feedUrl = context.Environment.GetEnvironmentVariable("FEED_URL");
        string feedApiKey = context.Environment.GetEnvironmentVariable("FEED_API_KEY"); ;
        if (feedApiKey != null && feedUrl != null)
            foreach (var packge in packges)
            {

                context.DotNetNuGetPush(packge.FullPath, new DotNetCoreNuGetPushSettings
                {
                    Source = feedUrl,
                    ApiKey = feedApiKey
                });
            }
        else
            context.Warning($" feed information not complete.. pushing to pacakge manage is skipped");
    }
}

[TaskName("Default")]
[IsDependentOn(typeof(CleanTask))]
[IsDependentOn(typeof(BuildTask))]
[IsDependentOn(typeof(TestTask))] 
public class DefaultTask : FrostingTask
{
}

[TaskName("Deploy")]
[IsDependentOn(typeof(VersionTask))]
[IsDependentOn(typeof(PackageTask))]
[IsDependentOn(typeof(NugetPushTask))]
public class DeployTask : FrostingTask
{
}