using System.Collections.Generic;

namespace StansAssets.Build.Pipeline
{
    public interface IBuildStepsViewModelContainer
    {
        IReadOnlyList<BuildStepViewModel> PreBuildSteps { get; } 
        IReadOnlyList<BuildStepViewModel> PostBuildSteps { get; } 
        IReadOnlyList<BuildStepViewModel> SceneProcessSteps { get; }
    }
}
