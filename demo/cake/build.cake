// Install latest Cake.VulnerabilityScanner as a Cake Addin,recommended to use a specific version
#addin nuget:?package=Cake.CloudFormation 

var target = Argument("target", "ScanPackages"); 

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////



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


//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);

// To run it make sure you have the cake tool installed and run this `dotnet cake --nuget_loaddependencies=true`