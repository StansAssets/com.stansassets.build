using UnityEditor;

namespace StansAssets.Build.Pipeline
{
    static class BuildHandler
    {
        [InitializeOnLoadMethod]
        static void Initialize()
        {
            BuildPlayerWindow.RegisterBuildPlayerHandler(RegisterBuildPlayer);
        }

        // Add BuildRunner Run with and without options

        static void RegisterBuildPlayer(BuildPlayerOptions options)
        {
#if SCENE_MANAGEMENT_ENABLED
            SceneManagement.Build.BuildScenesPreprocessor.SetupBuildOptions(ref options);
#endif
            var buildContext = new BuildContext(options, new BuildSettings());
            var buildRunner = new BuildRunner();
            var buildStepsContainer = GenerateBuildStepsContainer();
            buildRunner.Build(buildContext, buildStepsContainer);
        }

        public static IBuildStepsContainer GenerateBuildStepsContainer()
        {
            var stepsProvider = BuildPipelineSettings.DefineBuildStepsProvider();
            return stepsProvider.GetBuildSteps(new UserEditorBuildSettings());
        }

        public static string GetProviderName()
        {
            return  BuildPipelineSettings.DefineBuildStepsProvider().GetType().Name;
        }
    }
}
