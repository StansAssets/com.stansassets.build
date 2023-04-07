using StansAssets.Build.Pipeline;

namespace StansAssets.Build.Pipeline
{
    public class DefaultBuildTasksViewModelProvider : IBuildStepsViewModelProvider
    {
        public IBuildStepsViewModelContainer GetBuildSteps()
        {
            var provider = new DefaultBuildTasksProvider();
            var container = new BuildStepsViewModelSortedContainer();
            var callbackOrder = BuildProcessor.GetCallbackOrder();

            var buildSteps = provider.GetBuildTasks(new UserEditorBuildSettings());

            foreach (var preBuildStep in buildSteps.PreBuildTasks)
            {
                var step = new BuildStepViewModel(preBuildStep.GetType().FullName, callbackOrder);
                container.AddPreBuildStep(step);
            }

            foreach (var sceneProcessStep in buildSteps.ScenePostProcessTasks)
            {
                var step = new BuildStepViewModel(sceneProcessStep.GetType().FullName, callbackOrder);
                container.AddSceneProcessStep(step);
            }

            foreach (var postBuildStep in buildSteps.PostBuildTasks)
            {
                var step = new BuildStepViewModel(postBuildStep.GetType().FullName, callbackOrder);
                container.AddSceneProcessStep(step);
            }

            return container;
        }
    }
}
