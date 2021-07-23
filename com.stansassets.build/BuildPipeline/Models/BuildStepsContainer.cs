using System.Collections.Generic;

namespace StansAssets.Build.Pipeline
{
    /// <summary>
    /// Provides build steps. Order matters.
    /// </summary>
    class BuildStepsContainer : IBuildStepsContainer
    {
        readonly List<IBuildPreProcessStep> m_PreBuildSteps;
        readonly List<IBuildPostProcessStep> m_PostBuildSteps;
        readonly List<IScenePostProcessStep> m_ScenePostProcessStepsTasks;

        public IReadOnlyList<IBuildPreProcessStep> PreBuildSteps => m_PreBuildSteps;
        public IReadOnlyList<IBuildPostProcessStep> PostBuildSteps => m_PostBuildSteps;
        public IReadOnlyList<IScenePostProcessStep> ScenePostProcessStepsTasks => m_ScenePostProcessStepsTasks;

        public BuildStepsContainer()
        {
            m_PreBuildSteps = new List<IBuildPreProcessStep>();
            m_PostBuildSteps = new List<IBuildPostProcessStep>();
            m_ScenePostProcessStepsTasks = new List<IScenePostProcessStep>();
        }

        /// <summary>
        /// Registers a pre process build step.
        /// </summary>
        /// <param name="step">Step to be registered.</param>
        public void RegisterBuildPreProcessStep(IBuildPreProcessStep step)
        {
            m_PreBuildSteps.Add(step);
        }

        /// <summary>
        /// Registers a post process build step.
        /// </summary>
        /// <param name="step">Step to be registered.</param>
        public void RegisterBuildPostProcessStep(IBuildPostProcessStep step)
        {
            m_PostBuildSteps.Add(step);
        }

        /// <summary>
        /// Registers a post process scene step.
        /// </summary>
        /// <param name="step">Step to be registered.</param>
        public void RegisterScenePostProcessStep(IScenePostProcessStep step)
        {
            m_ScenePostProcessStepsTasks.Add(step);
        }
    }
}
