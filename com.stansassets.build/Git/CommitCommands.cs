using System;
using UnityEngine;

namespace StansAssets.Git
{
    class CommitCommands : BaseCommandProcessor
    {
        public CommitCommands(Func<string, string> requestsProcessor) : base(requestsProcessor) { }

        public string WithMessage(string message) => SendRequest($@"commit -m ""{message}""");
        public string Amend() => SendRequest("commit --amend");
        public string Hash => SendRequest("rev-parse HEAD").Trim();
        public string ShortHash => SendRequest("rev-parse --short HEAD").Trim();
        
        public string Message => SendRequest("show -s --format=%s").Trim();
        public double UnixTimestamp
        {
            get
            {
                var time = SendRequest("show -s --format=%ct").Trim();
                if (string.IsNullOrEmpty(time))
                    return 0;
                
                return Convert.ToDouble(time);
            }
        }
        /*
        public string Message => SendRequest("git show -s --format=%s").Trim();
        public string Time => SendRequest("show -s --pretty=%at").Trim(); // + "| xargs -I{} date -d @{} +%Y/%m/%d_%H:%M:%S" for formater git must run in from other terminal as child: https://github.com/git/git/blob/1f0fc1db8599f87520494ca4f0e3c1b6fabdf997/Documentation/pretty-formats.txt
            */
    }
}
