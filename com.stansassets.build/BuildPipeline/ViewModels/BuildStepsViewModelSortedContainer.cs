using System.Collections.Generic;

namespace StansAssets.Build.Pipeline
{
    public class BuildStepsViewModelSortedContainer : IBuildStepsViewModelContainer
    {
        readonly List<BuildStepViewModel> m_PreBuildSteps = new List<BuildStepViewModel>();
        readonly List<BuildStepViewModel> m_PostBuildSteps = new List<BuildStepViewModel>();
        readonly List<BuildStepViewModel> m_SceneProcessSteps = new List<BuildStepViewModel>();

        public IReadOnlyList<BuildStepViewModel> PreBuildSteps => m_PreBuildSteps;
        public IReadOnlyList<BuildStepViewModel> PostBuildSteps => m_PostBuildSteps;
        public IReadOnlyList<BuildStepViewModel> SceneProcessSteps => m_SceneProcessSteps;

        public void AddPreBuildStep(BuildStepViewModel step)
        {
            m_PreBuildSteps.AddSorted(step);
        }

        public void AddPostBuildStep(BuildStepViewModel step)
        {
            m_PostBuildSteps.AddSorted(step);
        }

        public void AddSceneProcessStep(BuildStepViewModel step)
        {
            m_SceneProcessSteps.AddSorted(step);
        }
    }
}
