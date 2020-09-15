using UnityEditor;

namespace StansAssets.Build.Editor
{
   static class BuildHandler
   {
      [InitializeOnLoadMethod]
      private static void Initialize()
      {
         BuildPlayerWindow.RegisterBuildPlayerHandler(RegisterBuildPlayer);
      }

      private static void RegisterBuildPlayer(BuildPlayerOptions options)
      {
         BuildContext buildContext = new BuildContext(options.target,options);

         BuildExecutor.Build(buildContext);
      }
   }
}
