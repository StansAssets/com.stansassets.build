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

        SettingsTab m_SettingsTab;

        protected override void OnWindowEnable(VisualElement root)
        {
            m_SettingsTab = new SettingsTab();
            AddTab("Settings", m_SettingsTab);
            AddTab("About", new AboutTab());

            SetupSettingsTab(m_SettingsTab);
        }

        void SetupSettingsTab(SettingsTab settingsTab)
        {
            settingsTab.UpdateBuildEntitiesCallback += UpdateBuildEntities;
            UpdateBuildEntities();
        }

        void UpdateBuildEntities()
        {
            var steps = BuildExecutor.Steps;
            var tasks = BuildExecutor.Tasks;
            m_SettingsTab.SetBuildEntities(steps.ToList().ConvertAll(s => new BuildStepEntity() { Name = s.Name}),
                                                                      tasks.ToList().ConvertAll(s => new BuildTaskEntity() { Name = s.Name}));
        }

        public static GUIContent WindowTitle => new GUIContent(BuildSystemPackage.DisplayName);
    }
}
