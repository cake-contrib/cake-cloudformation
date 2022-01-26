using Cake.Core;
using Cake.Core.IO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cake.CloudFormation
{
    public class BaseArguments
    {
        public string Profile { get; set; }

        public string ToCommandKeyValuePairs(Dictionary<string, string> keyValuePairs)
        {
            return string.Join(' ', keyValuePairs.Select(kv => $"{kv.Key}={kv.Value}").ToArray());
        }

        public string ToCommandList(List<string> list)
        {
            return string.Join(' ', list);
        }

        public Dictionary<string, string> ArgumentCustomization { get; set; }
        public virtual ProcessArgumentBuilder GetProcessArguments(ProcessArgumentBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));
            if (Profile != null)
                builder.AppendSwitch("--profile", Profile);

            if (ArgumentCustomization != null)
                foreach (var kv in ArgumentCustomization)
                    builder.AppendSwitch(kv.Key, kv.Value);

            return builder;
        }
    }
}
