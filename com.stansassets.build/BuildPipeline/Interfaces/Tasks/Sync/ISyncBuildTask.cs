namespace StansAssets.Build.Pipeline
{
    public interface ISyncBuildTask : IBuildTask
    {
        void Run(IBuildContext buildContext);
    }
}
