using System;

namespace StansAssets.Git
{
    class Git : BaseCommandProcessor
    {
        public AddCommands Add { get; }
        public WorkingCopy WorkingCopy { get; }
        public CommitCommands Commit { get; }
        public PushCommands Push { get; }
        public BranchCommands Branch { get; }

        public Git(Func<string, string> requestsProcessor) : base(requestsProcessor)
        {
            Add = new AddCommands(SendRequest);
            WorkingCopy = new WorkingCopy(SendRequest);
            Commit = new CommitCommands(SendRequest);
            Push = new PushCommands(SendRequest);
            Branch = new BranchCommands(SendRequest);
        }

        public string Command(string command) => SendRequest(command);
    }
}
