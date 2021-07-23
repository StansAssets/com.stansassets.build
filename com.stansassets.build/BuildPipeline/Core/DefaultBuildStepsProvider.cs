using System;
using StansAssets.Foundation;

namespace StansAssets.Build.Pipeline
{
    class DefaultBuildStepsProvider : IBuildStepsProvider
    {
        public IBuildStepsContainer GetBuildSteps(IUserEditorBuildSettings buildSettings)
        {
            var buildStepsContainer = new BuildStepsContainer();

            var preProcessSteps = ReflectionUtility.FindImplementationsOf<IBuildPreProcessStep>();
            foreach (var stepType in preProcessSteps)
            {
                var buildStep = Activator.CreateInstance(stepType) as IBuildPreProcessStep;
                buildStepsContainer.RegisterBuildPreProcessStep(buildStep);
            }

            var postProcessSteps = ReflectionUtility.FindImplementationsOf<IBuildPostProcessStep>();
            foreach (var stepType in postProcessSteps)
            {
                var buildStep = Activator.CreateInstance(stepType) as IBuildPostProcessStep;
                buildStepsContainer.RegisterBuildPostProcessStep(buildStep);
            }

            var scenePostProcessSteps = ReflectionUtility.FindImplementationsOf<IScenePostProcessStep>();
            foreach (var stepType in scenePostProcessSteps)
            {
                var buildStep = Activator.CreateInstance(stepType) as IScenePostProcessStep;
                buildStepsContainer.RegisterScenePostProcessStep(buildStep);
            }

            return buildStepsContainer;
        }
    }
}
