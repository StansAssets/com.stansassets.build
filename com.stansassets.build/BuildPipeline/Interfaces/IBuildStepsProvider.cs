namespace StansAssets.Build.Pipeline
{
    /// <summary>
    /// Provides steps
    /// </summary>
    public interface IBuildStepsProvider
    {
        IBuildStepsContainer GetBuildSteps(IUserEditorBuildSettings buildSettings);
    }
}
