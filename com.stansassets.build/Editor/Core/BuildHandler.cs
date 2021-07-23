using UnityEditor;

namespace StansAssets.Build.Editor
{
    static class BuildHandler
    {
        [InitializeOnLoadMethod]
        static void Initialize()
        {
            BuildPlayerWindow.RegisterBuildPlayerHandler(RegisterBuildPlayer);
        }

        static void RegisterBuildPlayer(BuildPlayerOptions options)
        {
            BuildExecutor.Settings.Reset();

#if SCENE_MANAGEMENT_ENABLED
         SceneManagement.Build.BuildScenesPreprocessor.SetupBuildOptions(ref options);
#endif
            var buildContext = new BuildContext(options, BuildExecutor.Settings);
            BuildExecutor.Build(buildContext);
        }
    }
}
