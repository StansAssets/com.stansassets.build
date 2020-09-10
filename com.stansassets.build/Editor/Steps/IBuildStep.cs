namespace StansAssets.Build.Editor
{
    public interface IBuildStep
    {
        event Action<ExecuteFinishedArgs> OnExecuteFinished;

        void Execute(BuildContext buildContext);

        int Priority { get; }
    }
}
