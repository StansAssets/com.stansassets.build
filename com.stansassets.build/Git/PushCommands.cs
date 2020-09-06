using System;

namespace StansAssets.Git
{
    class PushCommands : BaseCommandProcessor
    {
        public PushCommands(Func<string, string> requestsProcessor) : base(requestsProcessor) { }

        public string Default() => SendRequest("push");
    }
}
