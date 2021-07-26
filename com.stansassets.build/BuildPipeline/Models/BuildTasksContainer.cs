using System.Collections.Generic;

namespace StansAssets.Build.Pipeline
{
    /// <summary>
    /// Provides build steps. Order matters.
    /// </summary>
    public class BuildTasksContainer : IBuildTasksContainer
    {
        readonly List<IBuildTask> m_PreBuildSteps;
        readonly List<IBuildTask> m_PostBuildSteps;
        readonly List<IScenePostProcessTask> m_ScenePostProcessStepsTasks;

        public IReadOnlyList<IBuildTask> PreBuildTasks => m_PreBuildSteps;
        public IReadOnlyList<IBuildTask> PostBuildTasks => m_PostBuildSteps;
        public IReadOnlyList<IScenePostProcessTask> ScenePostProcessStepsTasks => m_ScenePostProcessStepsTasks;

        public BuildTasksContainer()
        {
            m_PreBuildSteps = new List<IBuildTask>();
            m_PostBuildSteps = new List<IBuildTask>();
            m_ScenePostProcessStepsTasks = new List<IScenePostProcessTask>();
        }

        /// <summary>
        /// Registers a pre process build step.
        /// </summary>
        /// <param name="step">Step to be registered.</param>
        public void AddPreProcessTask(IBuildTask step)
        {
            m_PreBuildSteps.Add(step);
        }

        /// <summary>
        /// Registers a post process build step.
        /// </summary>
        /// <param name="step">Step to be registered.</param>
        public void AddPostProcessTask(IBuildTask step)
        {
            m_PostBuildSteps.Add(step);
        }

        /// <summary>
        /// Registers a post process scene step.
        /// </summary>
        /// <param name="task">Step to be registered.</param>
        public void AddScenePostProcessTask(IScenePostProcessTask task)
        {
            m_ScenePostProcessStepsTasks.Add(task);
        }
    }
}
