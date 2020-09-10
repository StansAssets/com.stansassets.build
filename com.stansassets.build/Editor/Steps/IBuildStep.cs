namespace StansAssets.Build.Editor
{
    public interface IBuildStep
    {
        bool Execute(BuildContext buildContext);

        int Priority { get; }
        string GetResultMessage();
    }
}
