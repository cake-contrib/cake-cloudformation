// Install latest Cake.VulnerabilityScanner as a Cake Addin,recommended to use a specific version
#addin nuget:?package=Cake.CloudFormation&Version=0.1.0-preview00003 

var target = Argument("target", "CF-Deploy"); 

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////



Task("CF-Deploy") 
    .Does( () =>
{
    CloudFormationDeploy(new DeployArguments
            {
                TemplateFile = "template.yaml",
                StackName =  "my-dev-stack",
                ParameterOverrides = new System.Collections.Generic.Dictionary<string, string> { { "TopicName", "my-cake-topic"},{ "TopicName1", "my-cake-topic1"} },
                Capabilities= new System.Collections.Generic.List<string> { { "CAPABILITY_IAM" }, { "CAPABILITY_NAMED_IAM" },{"CAPABILITY_AUTO_EXPAND" } },
                Tags = new System.Collections.Generic.Dictionary<string, string> { { "owner", "ci-team"},{ "owner2", "ci-team2"} },
                NoExecuteChangeset=true,
            });
});


//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);

// To run it make sure you have the cake tool installed and run this `dotnet cake --nuget_loaddependencies=true`