using StansAssets.Build.Editor;
using UnityEditor;

static class BuildHandler
{
   [InitializeOnLoadMethod]
   private static void Initialize()
   {
      BuildPlayerWindow.RegisterBuildPlayerHandler(RegisterBuildPlayer);
   }
   
   private static void RegisterBuildPlayer(BuildPlayerOptions options)
   {
      BuildContext buildContext = new BuildContext
      {
         TargetPlatform = options.target,
         BuildPlayerOptions = options,
      };

      BuildExecutor.Build(buildContext);
   }
}
