using UnityEditor;

namespace StansAssets.Build.Pipeline
{
    /// <summary>
    /// Build configuration data
    /// </summary>
    class BuildContext : IBuildContext
    {
        BuildPlayerOptions m_BuildPlayerOptions;

        public BuildContext(BuildPlayerOptions buildPlayerOptions, BuildSettings buildSettings)
        {
            m_BuildPlayerOptions = buildPlayerOptions;
            BuildSettings = buildSettings;

            TargetPlatform = EditorUserBuildSettings.activeBuildTarget;
        }

        public void SetScenes(string[] scenes)
        {
            m_BuildPlayerOptions.scenes = scenes;
        }

        public BuildTarget TargetPlatform { get; }
        public BuildPlayerOptions BuildPlayerOptions => m_BuildPlayerOptions;
        public BuildSettings BuildSettings { get; }
    }
}
