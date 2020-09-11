using StansAssets.Build.Editor;
using UnityEditor;
using UnityEngine;

static class BuildHandler
{
   [InitializeOnLoadMethod]
   private static void Initialize()
   {
      BuildPlayerWindow.RegisterGetBuildPlayerOptionsHandler(RegisterPlayerOptions);
      BuildPlayerWindow.RegisterBuildPlayerHandler(RegisterBuildPlayer);
   }

   private static BuildPlayerOptions RegisterPlayerOptions(BuildPlayerOptions options)
   {
      return options;
   }

   private static void RegisterBuildPlayer(BuildPlayerOptions options)
   {   
      //Example code
      BuildContext buildContext = new BuildContext
      {
         TargetPlatform = RuntimePlatform.Android,
      };

      BuildExecutor.Build(buildContext);
   }
}
