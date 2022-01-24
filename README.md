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
  
 
2. Add a task 

```
Task("CF Deploy") 
    .Does( async () =>
{
    CloudFormationDeploy(new DeployArguments
            {
                TemplateFile = "template.yaml",
                StackName =  "my-dev-stack",
                ParameterOverrides = new System.Collections.Generic.Dictionary<string, string> { { "TopicName", "my-cake-topic"} },
                Capabilities= new System.Collections.Generic.List<string> { { "CAPABILITY_IAM" }, { "CAPABILITY_NAMED_IAM" },{"CAPABILITY_AUTO_EXPAND" } },
                NoExecuteChangeset=true,
            });
});
``` 

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


## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.



