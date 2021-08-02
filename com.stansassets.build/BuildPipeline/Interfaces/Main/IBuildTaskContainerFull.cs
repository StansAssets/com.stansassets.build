using System.Collections.Generic;
using UnityEditor.Build;

namespace StansAssets.Build.Pipeline
{
    /// <summary>
    /// Provides build steps.
    /// </summary>
    public interface IBuildTasksContainerFull : IBuildTasksContainer
    {
        /// <summary>
        /// Pre process build steps. Order matters.
        /// </summary>
        IReadOnlyList<IOrderedCallback> UnityPreBuildTasks { get; }

        /// <summary>
        /// Post process build steps. Order matters.
        /// </summary>
        IReadOnlyList<IOrderedCallback> UnityPostBuildTasks { get; }

        /// <summary>
        /// Scene process build steps. Order matters.
        /// </summary>
        IReadOnlyList<IOrderedCallback> UnityProcessSceneTasks { get; }
    }
}
