using System.Collections.Generic;

namespace StansAssets.Build.Pipeline
{
    /// <summary>
    /// Provides build tasks.
    /// </summary>
    public interface IBuildTasksContainer
    {
        /// <summary>
        /// Pre process build tasks. Order matters.
        /// </summary>
        IReadOnlyList<IBuildTask> PreBuildTasks { get; }

        /// <summary>
        /// Post process build tasks. Order matters.
        /// </summary>
        IReadOnlyList<IBuildTask> PostBuildTasks { get; }

        /// <summary>
        /// Scene process build tasks. Order matters.
        /// </summary>
        IReadOnlyList<IScenePostProcessTask> ScenePostProcessTasks { get; }
    }
}
