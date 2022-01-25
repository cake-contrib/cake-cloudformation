using Cake.Common;
using Cake.Common.Diagnostics;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.CloudFormation
{
    internal class CommandRunner<T> where T : BaseArguments, new()
    {
        private readonly string _command;

        public CommandRunner(string command)
        {
            _command = command;
        }
        public int Run(ICakeContext context, T arguments)
        {
            var builder = new ProcessArgumentBuilder();
            builder.Append("cloudformation");
            builder.Append(_command);
            builder = arguments.GetProcessArguments(builder);
            context.Information($"Running command : `aws {builder.RenderSafe()}`");
            return context.StartProcess("aws", new ProcessSettings { Arguments = builder}); ;
        }
    }
}
