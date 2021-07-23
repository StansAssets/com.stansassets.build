using System;
using System.Linq;
using StansAssets.Foundation;
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
            => PackageManagerUtility.GetPackageInfo(BuildSystemPackage.PackageName);

        protected override void OnWindowEnable(VisualElement root)
        {
            var tabsTypes = ReflectionUtility.FindImplementationsOf<IBuildSystemWindowTab>();
            foreach (var tabType in tabsTypes)
            {
                var tabInstance = Activator.CreateInstance(tabType) as IBuildSystemWindowTab;
                AddTab(tabInstance.Title, tabInstance.Tab);
            }

            AddTab("About", new AboutTab());

            // SetupSettingsTab(m_SettingsTab);
        }

        /*
        void SetupSettingsTab(SettingsTab settingsTab)
        {
            settingsTab.UpdateBuildEntitiesCallback += UpdateBuildEntities;
            UpdateBuildEntities();
        }*/

        void UpdateBuildEntities()
        {
            var steps = BuildExecutor.Steps;
            var tasks = BuildExecutor.Tasks;

            //  m_SettingsTab.SetBuildEntities(steps.ToList().ConvertAll(s => new BuildStepEntity() { Name = s.Name}),
          //  tasks.ToList().ConvertAll(s => new BuildTaskEntity() { Name = s.Name }));
        }

        public static GUIContent WindowTitle => new GUIContent(BuildSystemPackage.DisplayName);
    }
}
