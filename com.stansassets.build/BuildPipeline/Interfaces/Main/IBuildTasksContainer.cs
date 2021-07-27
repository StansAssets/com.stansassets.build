using System.Collections.Generic;

namespace StansAssets.Build.Pipeline
{
    /// <summary>
    /// Provides build steps.
    /// </summary>
    public interface IBuildTasksContainer
    {
        /// <summary>
        /// Pre process build steps. Order matters.
        /// </summary>
        IReadOnlyList<IBuildTask> PreBuildTasks { get; }

        /// <summary>
        /// Post process build steps. Order matters.
        /// </summary>
        IReadOnlyList<IBuildTask> PostBuildTasks { get; }

        /// <summary>
        /// Scene process build steps. Order matters.
        /// </summary>
        IReadOnlyList<IScenePostProcessTask> ScenePostProcessTasks { get; }
    }
}
