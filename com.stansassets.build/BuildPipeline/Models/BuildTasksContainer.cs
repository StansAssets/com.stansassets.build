using System.Collections.Generic;
using UnityEditor.Build;

namespace StansAssets.Build.Pipeline
{
    /// <summary>
    /// Provides build tasks. Order matters.
    /// </summary>
    public class BuildTasksContainer : IBuildTasksContainerFull
    {
        readonly List<IPreprocessBuildWithReport> m_UnityPreProcessBuildTasks;
        readonly List<IPostprocessBuildWithReport> m_UnityPostprocessBuildTasks;
        readonly List<IProcessSceneWithReport> m_UnityProcessSceneTasks;

        readonly List<IBuildTask> m_PreBuildSteps;
        readonly List<IBuildTask> m_PostBuildSteps;
        readonly List<IScenePostProcessTask> m_ScenePostProcessStepsTasks;

        public IReadOnlyList<IBuildTask> PreBuildTasks => m_PreBuildTasks;
        public IReadOnlyList<IBuildTask> PostBuildTasks => m_PostBuildTasks;
        public IReadOnlyList<IScenePostProcessTask> ScenePostProcessTasks => m_ScenePostProcessTasks;

        public IReadOnlyList<IOrderedCallback> UnityPreBuildTasks => m_UnityPreProcessBuildTasks;
        public IReadOnlyList<IOrderedCallback> UnityPostBuildTasks => m_UnityPostprocessBuildTasks;
        public IReadOnlyList<IOrderedCallback> UnityProcessSceneTasks => m_UnityProcessSceneTasks;

        public BuildTasksContainer()
        {
            m_UnityPreProcessBuildTasks = new List<IPreprocessBuildWithReport>();
            m_UnityPostprocessBuildTasks = new List<IPostprocessBuildWithReport>();
            m_UnityProcessSceneTasks = new List<IProcessSceneWithReport>();

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
        /// Registers a pre process build task.
        /// </summary>
        /// <param name="task">Task to be registered.</param>
        public void AddPreProcessBuildTask(IPreprocessBuildWithReport task)
        {
            m_UnityPreProcessBuildTasks.Add(task);
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
        /// Registers a post process build step.
        /// </summary>
        /// <param name="task">Step to be registered.</param>
        public void AddPostProcessBuildTask(IPostprocessBuildWithReport task)
        {
            m_UnityPostprocessBuildTasks.Add(task);
        }

        /// <summary>
        /// Registers a post process scene step.
        /// </summary>
        /// <param name="task">Task to be registered.</param>
        public void AddScenePostProcessTask(IScenePostProcessTask task)
        {
            m_ScenePostProcessTasks.Add(task);
        }

        /// <summary>
        /// Registers a process scene step.
        /// </summary>
        /// <param name="task">Step to be registered.</param>
        public void AddProcessSceneTask(IProcessSceneWithReport task)
        {
            m_UnityProcessSceneTasks.Add(task);
        }
    }
}
