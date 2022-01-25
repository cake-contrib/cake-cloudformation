using Cake.Core;
using Cake.Core.Annotations;

namespace Cake.CloudFormation
{
    [CakeAliasCategory("CloudFormaiton")]
    public static class Aliases
    {
        /// <summary>
        /// Runs `aws cloudformatin deploy ...`
        /// </summary>
        /// <param name="context"></param>
        /// <param name="arguments"></param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="CakeException"></exception>
        [CakeMethodAlias]
        public static void CloudFormationDeploy(this ICakeContext context, DeployArguments arguments)
        {
            if (context == null)
            {
                throw new System.ArgumentNullException(nameof(context));
            }

            if (arguments == null)
            {
                throw new System.ArgumentNullException(nameof(arguments));
            }

            var exitCode = new DeployCommandRunner().Run(context, arguments);
            if (exitCode != 0)
                throw new CakeException(exitCode); 
        }

        /// <summary>
        /// Runs any cloudformatin command 
        /// Can be used to run any of CF commands listed here https://docs.aws.amazon.com/cli/latest/reference/cloudformation/index.html#cli-aws-cloudformation
        /// </summary>
        /// <param name="context"></param>
        /// <param name="command"></param>
        /// <param name="arguments"></param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="CakeException"></exception>
        [CakeMethodAlias]
        public static void CloudFormationCommand(this ICakeContext context,string command, BaseArguments arguments)
        {
            if (context == null)
            {
                throw new System.ArgumentNullException(nameof(context));
            }

            if (arguments == null)
            {
                throw new System.ArgumentNullException(nameof(arguments));
            }

            var exitCode = new CommandRunner<BaseArguments>(command).Run(context, arguments);
            if (exitCode != 0)
                throw new CakeException(exitCode);
        }
    }
}
