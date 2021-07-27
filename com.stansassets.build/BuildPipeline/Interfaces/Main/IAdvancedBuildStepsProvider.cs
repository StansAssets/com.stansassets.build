namespace StansAssets.Build.Pipeline
{
    public interface IAdvancedBuildStepsProvider
    {
        IBuildTasksContainer GetBuildSteps(IUserEditorBuildSettings buildSettings, IBuildTasksContainer defaultTasksContainer);
    }
}
