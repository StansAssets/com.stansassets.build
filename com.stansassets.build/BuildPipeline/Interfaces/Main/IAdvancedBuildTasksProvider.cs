namespace StansAssets.Build.Pipeline
{
    public interface IAdvancedBuildTasksProvider
    {
        IBuildTasksContainer GetBuildTasks(IUserEditorBuildSettings buildSettings, IBuildTasksContainer defaultTasksContainer);
    }
}
