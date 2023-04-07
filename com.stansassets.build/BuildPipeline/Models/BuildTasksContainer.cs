using System.Collections.Generic;

namespace StansAssets.Build.Pipeline
{
    /// <summary>
    /// Provides build tasks. Order matters.
    /// </summary>
    public class BuildTasksContainer : IBuildTasksContainer
    {
        readonly List<IBuildTask> m_PreBuildTasks;
        readonly List<IBuildTask> m_PostBuildTasks;
        readonly List<IScenePostProcessTask> m_ScenePostProcessTasks;

        public IReadOnlyList<IBuildTask> PreBuildTasks => m_PreBuildTasks;
        public IReadOnlyList<IBuildTask> PostBuildTasks => m_PostBuildTasks;
        public IReadOnlyList<IScenePostProcessTask> ScenePostProcessTasks => m_ScenePostProcessTasks;

        public BuildTasksContainer()
        {

            m_PreBuildTasks = new List<IBuildTask>();
            m_PostBuildTasks = new List<IBuildTask>();
            m_ScenePostProcessTasks = new List<IScenePostProcessTask>();
        }

        /// <summary>
        /// Registers a pre process build task.
        /// </summary>
        /// <param name="task">Task to be registered.</param>
        public void AddPreProcessTask(IBuildTask task)
        {
            m_PreBuildTasks.Add(task);
        }

        /// <summary>
        /// Registers a post process build task.
        /// </summary>
        /// <param name="task">Task to be registered.</param>
        public void AddPostProcessTask(IBuildTask task)
        {
            m_PostBuildTasks.Add(task);
        }

        /// <summary>
        /// Registers a post process scene task.
        /// </summary>
        /// <param name="task">Task to be registered.</param>
        public void AddScenePostProcessTask(IScenePostProcessTask task)
        {
            m_ScenePostProcessTasks.Add(task);
        }
    }
}
