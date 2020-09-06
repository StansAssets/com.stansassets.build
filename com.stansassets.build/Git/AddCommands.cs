using System;

namespace StansAssets.Git
{
    class AddCommands : BaseCommandProcessor
    {
        public AddCommands(Func<string, string> requestsProcessor) : base(requestsProcessor) { }

        public string Files(string filter) => SendRequest($"add {filter}");
    }
}
