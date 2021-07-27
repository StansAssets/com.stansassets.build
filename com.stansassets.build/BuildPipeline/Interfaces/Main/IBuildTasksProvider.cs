namespace StansAssets.Build.Pipeline
{
    /// <summary>
    /// Provides steps
    /// </summary>
    public interface IBuildTasksProvider
    {
        IBuildTasksContainer GetBuildSteps(IUserEditorBuildSettings buildSettings);
    }
}
