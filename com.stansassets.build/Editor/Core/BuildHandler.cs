using JetBrains.Annotations;
using UnityEditor;

namespace StansAssets.Build.Editor
{
   [UsedImplicitly]
   static class BuildHandler
   {
      [InitializeOnLoadMethod, UsedImplicitly]
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
