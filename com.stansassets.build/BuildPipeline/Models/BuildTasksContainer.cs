using System.Collections.Generic;

namespace StansAssets.Build.Pipeline
{
    /// <summary>
    /// Provides build tasks. Order matters.
    /// </summary>
    public class BuildTasksContainer : IBuildTasksContainer
    {
        readonly List<IBuildTask> m_PreBuildSteps;
        readonly List<IBuildTask> m_PostBuildSteps;
        readonly List<IScenePostProcessTask> m_ScenePostProcessStepsTasks;

        public IReadOnlyList<IBuildTask> PreBuildTasks => m_PreBuildTasks;
        public IReadOnlyList<IBuildTask> PostBuildTasks => m_PostBuildTasks;
        public IReadOnlyList<IScenePostProcessTask> ScenePostProcessTasks => m_ScenePostProcessTasks;

        public BuildTasksContainer()
        {

            m_PreBuildSteps = new List<IBuildTask>();
            m_PostBuildSteps = new List<IBuildTask>();
            m_ScenePostProcessStepsTasks = new List<IScenePostProcessTask>();
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
        /// Registers a post process build step.
        /// </summary>
        /// <param name="task">Task to be registered.</param>
        public void AddPostProcessTask(IBuildTask task)
        {
            m_PostBuildTasks.Add(task);
        }

        /// <summary>
        /// Registers a post process scene step.
        /// </summary>
        /// <param name="task">Task to be registered.</param>
        public void AddScenePostProcessTask(IScenePostProcessTask task)
        {
            m_ScenePostProcessTasks.Add(task);
        }
    }
}
