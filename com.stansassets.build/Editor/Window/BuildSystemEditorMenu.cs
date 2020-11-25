using StansAssets.Plugins.Editor;
using UnityEditor;

namespace StansAssets.Build.Editor
{
    static class BuildSystemEditorMenu
    {
        [MenuItem(PluginsDevKitPackage.RootMenu + "/" + BuildSystemPackage.DisplayName + "/Settings", false, 0)]
        public static void OpenSettingsTest()
        {
            var headerContent = BuildSystemSettingsWindow.WindowTitle;
            BuildSystemSettingsWindow.ShowTowardsInspector(headerContent.text, headerContent.image);
        }
    }
}
