using System;

namespace StansAssets.Git
{ 
    class WorkingCopy : BaseCommandProcessor
    {
        public WorkingCopy(Func<string, string> requestsProcessor) : base(requestsProcessor) { }

        public bool HasChanges => !string.IsNullOrEmpty(SendRequest("status --ignore-submodules=dirty -s").Trim());
    }
}
