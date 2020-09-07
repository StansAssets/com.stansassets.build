using StansAssets.Foundation.Editor;
using StansAssets.Plugins.Editor;
using UnityEngine;
using UnityEngine.UIElements;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace StansAssets.Build.Editor
{
    class BuildSystemSettingsWindow : PackageSettingsWindow<BuildSystemSettingsWindow>
    {
        protected override PackageInfo GetPackageInfo()
            => PackageManagerUtility.GetPackageInfo(BuildSystemSettings.Instance.PackageName);

        protected override void OnWindowEnable(VisualElement root)
        {
            AddTab("Settings", new SettingsTab());
            AddTab("About", new AboutTab());
        }

        public static GUIContent WindowTitle => new GUIContent(BuildSystemPackage.DisplayName);
    }
}
