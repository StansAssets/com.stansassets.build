using System.Linq;
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
            BuildSystemSettings.Instance.SettingsTab = new SettingsTab();
            AddTab("Settings", BuildSystemSettings.Instance.SettingsTab);
            AddTab("About", new AboutTab());

            SetupSettingsTab();
        }

        void SetupSettingsTab()
        {
            BuildSystemSettings.Instance.SettingsTab.UpdateBuildEntitiesCallback += UpdateBuildEntities;
            UpdateBuildEntities();
        }

        void UpdateBuildEntities()
        {
            BuildExecutor.RegisterListeners(new BuildContext(default, default));
            var steps = BuildExecutor.Steps;
            var tasks = BuildExecutor.Tasks;

            BuildSystemSettings.Instance.SettingsTab.SetBuildEntities(steps.ToList().ConvertAll(s => new BuildStepEntity() { Name = s.Name}),
                                                                      tasks.ToList().ConvertAll(s => new BuildTaskEntity() { Name = s.Name}));
        }

        public static GUIContent WindowTitle => new GUIContent(BuildSystemPackage.DisplayName);
    }
}
