using Cake.Core;
using Cake.Core.IO;
using System.Collections.Generic;
using System.Linq;

namespace Cake.CloudFormation
{
    //https://docs.aws.amazon.com/cli/latest/reference/cloudformation/deploy/index.html
    public class DeployArguments : BaseArguments
    {
        public string TemplateFile { get; set; }
        public string StackName { get; set; }
        public string S3Bucket { get; set; }
        public string S3Prefix { get; set; }
        public string KmsKeyId { get; set; }
        public string RoleArn { get; set; }
        public bool ForceUpload { get; set; }
        public bool NoExecuteChangeset { get; set; }
        public bool NoFailOnEmptyChangeset { get; set; }
        public List<string> Capabilities { get; set; }
        public List<string> NotificationArns { get; set; }
        public Dictionary<string, string> ParameterOverrides { get; set; }
        public Dictionary<string, string> Tags { get; set; }
        public override ProcessArgumentBuilder GetProcessArguments(ProcessArgumentBuilder builder)
        {
            base.GetProcessArguments(builder);

            if (TemplateFile != null)
                builder.AppendSwitch("--template-file", TemplateFile);

            if (StackName != null)
                builder.AppendSwitch("--stack-name", StackName);

            if (S3Bucket != null)
                builder.AppendSwitch("--s3-bucket", S3Bucket);

            if (S3Prefix != null)
                builder.AppendSwitch("--s3-prefix", S3Prefix);

            if (KmsKeyId != null)
                builder.AppendSwitch("--kms-key-id", KmsKeyId);

            if (ForceUpload)
                builder.Append("--force-upload");

            if (NoExecuteChangeset)
                builder.Append("--no-execute-changeset");

            if (NoFailOnEmptyChangeset)
                builder.Append("--no-fail-on-empty-changese");

            if (Capabilities != null)
                builder.AppendSwitch("--capabilities", ToCommandList(Capabilities));

            if (NotificationArns != null)
                builder.AppendSwitch("--notification-arns", ToCommandList(NotificationArns));

            if (ParameterOverrides != null)
                builder.AppendSwitch("--parameter-overrides", ToCommandKeyValuePairs(ParameterOverrides));

            if (Tags != null)
                builder.AppendSwitch("--tags", ToCommandKeyValuePairs(Tags));

            return builder;
        }
    }
}
