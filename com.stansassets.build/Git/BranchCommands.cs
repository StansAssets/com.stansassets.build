using System;

namespace StansAssets.Git
{
    class BranchCommands : BaseCommandProcessor
    {
        public BranchCommands(Func<string, string> requestsProcessor) : base(requestsProcessor) { }

        public string Name => SendRequest("rev-parse --abbrev-ref HEAD").Trim();
    }
}
