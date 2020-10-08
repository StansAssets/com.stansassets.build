using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace StansAssets.Build.Editor
{
    public static class BuildContextUtility
    {
        public static BuildPlayerOptions BuildPlayerOptions => GetBuildPlayerOptions(false, new BuildPlayerOptions());
        public static BuildSettings BuildSettings => new BuildSettings();
        public static string DefaultBuildLocation => $"{Path.GetDirectoryName(Application.dataPath)}\\Builds\\{EditorUserBuildSettings.activeBuildTarget.ToString()}";

        static BuildPlayerOptions GetBuildPlayerOptions(
            bool askForLocation = false,
            BuildPlayerOptions defaultOptions = new BuildPlayerOptions())
        {
            // Fake buildLocation path to prevent exception
            string locationPath = EditorUserBuildSettings.GetBuildLocation(EditorUserBuildSettings.activeBuildTarget);
            if (locationPath.Length == 0)
            {
                EditorUserBuildSettings.SetBuildLocation(EditorUserBuildSettings.activeBuildTarget, DefaultBuildLocation);
            }

            // Get static internal "GetBuildPlayerOptionsInternal" method
            MethodInfo method = typeof(BuildPlayerWindow.DefaultBuildMethods).GetMethod(
                "GetBuildPlayerOptionsInternal",
                BindingFlags.NonPublic | BindingFlags.Static);

            // invoke internal method
            return (BuildPlayerOptions)method.Invoke(
                null,
                new object[] { askForLocation, defaultOptions});
        }
    }
}
