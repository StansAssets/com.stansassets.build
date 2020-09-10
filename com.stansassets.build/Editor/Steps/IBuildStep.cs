using System;

namespace StansAssets.Build.Editor
{
    public interface IBuildStep
    {
        event Action<ExecuteFinishedArgs> OnExecuteFinished;

        void Execute(BuildContext buildContext, Action onComplete = null);

        int Priority { get; }
    }
}
