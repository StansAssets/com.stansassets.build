namespace StansAssets.Build.Pipeline
{
    /// <summary>
    /// Provides steps
    /// </summary>
    public interface IBuildTasksProvider
    {
        IBuildTasksContainer GetBuildTasks(IUserEditorBuildSettings buildSettings);
    }
}
