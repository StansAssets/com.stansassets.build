using StansAssets.Build.Editor;
using StansAssets.Foundation;
using StansAssets.Plugins;
using UnityEngine;

namespace StansAssets.Build.Pipeline
{
    class BuildPipelineSettings : PackageScriptableSettingsSingleton<BuildPipelineSettings>
    {
        public static readonly string WindowTabsPath = $"{BuildSystemPackage.RootPath}/BuildPipeline/UserInterface";

        public override string PackageName => BuildSystemPackage.PackageName;

        [SerializeField]
        string m_CustomBuildStepsProviderTypeName;

        // TODO use it from UI to update custom build provider name
        public void SetBuildProviderTypeName(string typeName)
        {
            m_CustomBuildStepsProviderTypeName = typeName;
        }

        public static IBuildTasksProvider DefineBuildStepsProvider()
        {
            IBuildTasksProvider customProvider = null;
            var customBuildStepsProviderTypeName = Instance.m_CustomBuildStepsProviderTypeName;
            if (!string.IsNullOrEmpty(customBuildStepsProviderTypeName))
            {
                var provider = ReflectionUtility.CreateInstance(customBuildStepsProviderTypeName);
                if (provider != null && provider is IBuildTasksProvider buildStepsProvider)
                {
                    customProvider = buildStepsProvider;
                }
                else
                {
                    Debug.LogError($"Wasn't able to instantiate custom build steps provider with type name: {customBuildStepsProviderTypeName}");
                }
            }

            return customProvider ?? new DefaultBuildTasksProvider();
        }
    }
}
