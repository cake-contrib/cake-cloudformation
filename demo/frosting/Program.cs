using Cake.CloudFormation;
using Cake.Frosting;
using System.Threading;
using System.Threading.Tasks;

namespace Build
{
    class Program
    {
        public static int Main(string[] args)
        {
            return new CakeHost()
                .Run(args);
        }
    }

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

    [TaskName("CF Command")]
    public sealed class CloudFormationCommand : FrostingTask<FrostingContext>
    {
        public override void Run(FrostingContext context)
        {
            context.CloudFormationCommand("describe-stacks", new BaseArguments
            {
               ArgumentCustomization= new System.Collections.Generic.Dictionary<string, string> { { "--stack-name", "my-dev-stack" } }
            });
        }
    }
    [TaskName("Default")]
    [IsDependentOn(typeof(CloudFormationDeploy))]
    [IsDependentOn(typeof(CloudFormationCommand))]
    public class DefaultTask : FrostingTask
    {
    }

}
