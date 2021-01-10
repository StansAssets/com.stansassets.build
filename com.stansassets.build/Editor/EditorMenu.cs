using StansAssets.Plugins.Editor;
using UnityEditor;

namespace StansAssets.Build.Editor
{
    static class EditorMenu
    {
        [MenuItem(PluginsDevKitPackage.RootMenu + "/" + BuildSystemPackage.DisplayName + "/Test Build Increment")]
        static void TestBuildIncrement()
        {
            var buildMetadata = Build.Metadata;
            var incrementBuildNumberEnable = BuildSystemSettings.Instance.AutomatedBuildNumberIncrement &&
                                             StanAssetsPackages.IsGoogleDocConnectorProInstalled;
            if (incrementBuildNumberEnable)
            {
                IncrementBuildNumber.Increment(buildMetadata, EditorUserBuildSettings.activeBuildTarget);
            }
        }
    }
}
