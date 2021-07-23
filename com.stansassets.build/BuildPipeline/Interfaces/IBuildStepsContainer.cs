using System.Collections.Generic;

namespace StansAssets.Build.Pipeline
{
    /// <summary>
    /// Provides build steps.
    /// </summary>
    public interface IBuildStepsContainer
    {
        /// <summary>
        /// Pre process build steps. Order matters.
        /// </summary>
        IReadOnlyList<IBuildPreProcessStep> PreBuildSteps { get; }

        /// <summary>
        /// Post process build steps. Order matters.
        /// </summary>
        IReadOnlyList<IBuildPostProcessStep> PostBuildSteps { get; }

        /// <summary>
        /// Scene process build steps. Order matters.
        /// </summary>
        IReadOnlyList<IScenePostProcessStep> ScenePostProcessStepsTasks { get; }
    }
}
