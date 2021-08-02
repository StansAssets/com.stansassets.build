namespace StansAssets.Build.Pipeline
{
    /// <summary>
    /// Provides steps
    /// </summary>
    public interface IBuildTasksProvider
    {
        IBuildTasksContainerFull GetBuildSteps(IUserEditorBuildSettings buildSettings);
    }
}
