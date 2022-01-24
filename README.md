# Cake.CloudForamtion | cakebuild

 A CakeBuild plugin to run AWS CloudFormation CLI commands,
https://docs.aws.amazon.com/cli/latest/reference/cloudformation/index.html#cli-aws-cloudformation
 
 
## Usage 

The plugin could be used as cakebuild script plugin or cake frosting task. 

### Cake script 
 
 1. Add the plugin to your cake script : 
  ```
  #addin nuget:?package=Cake.CloudFormation&version=[specify the version here]
  ```
 
  https://github.com/RapidFailure/cake-dotnet-vulnerability-scanner/blob/6b69ae9b3f34ebddf0b483c01cbfa9973bc0694d/demo/cake/build.cake#L2
 
2. Add a task 

```
Task("ScanPackages") 
    .Does( async () =>
{
            var ossIndexToken = Environment.GetEnvironmentVariable("OSS_INDEX_TOKEN");
            await  ScanPackagesAsync(new ScanPackagesSettings
            {
                Ecosystem="nuget",
                FailOnVulnerability=false,
                OssIndexBaseUrl="https://ossindex.sonatype.org/",
                OssIndexToken=ossIndexToken,
                SolutionFile="../../Cake.VulnerabilityScanner.sln",
                Verbosity= Microsoft.Extensions.Logging.LogLevel.Debug
            }, System.Threading.CancellationToken.None);
});
```
https://github.com/RapidFailure/cake-dotnet-vulnerability-scanner/blob/6b69ae9b3f34ebddf0b483c01cbfa9973bc0694d/demo/cake/build.cake#L12-L26

### Cake Frosting
1. Install the pacakge 

```
dotnet add package Cake.CloudFormation --version 0.3.0
```
2. add the task as following 

```
    [TaskName("CF Deploy")]
    public sealed class CloudFormationDeploy :  FrostingTask<FrostingContext>
    {
        public override void Run(FrostingContext context)
        {
            context.CloudFormationDeploy(new DeployArguments
            {
                TemplateFile = "template.yaml",
                StackName =  "my-dev-stack",
                ParameterOverrides = new System.Collections.Generic.Dictionary<string, string> { { "TopicName", "my-cake-topic"} },
                Capabilities= new System.Collections.Generic.List<string> { { "CAPABILITY_IAM" }, { "CAPABILITY_NAMED_IAM" },{"CAPABILITY_AUTO_EXPAND" } },
                NoExecuteChangeset=true,
            });
        }
    }
```
https://github.com/RapidFailure/cake-dotnet-vulnerability-scanner/blob/2bbf524dd0b39af05256452502128f340c61a5a4/demo/frosting/Program.cs#L18-L35


## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.



