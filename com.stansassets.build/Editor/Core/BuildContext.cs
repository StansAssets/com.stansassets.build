using UnityEditor;

namespace StansAssets.Build.Editor
{
    /// <summary>
    /// Build configuration data
    /// </summary>
    public class BuildContext : IBuildContext
    {
        private BuildPlayerOptions m_BuildPlayerOptions;
        public BuildContext(BuildPlayerOptions buildPlayerOptions, string buildAlias = null)
        {
            m_BuildPlayerOptions = buildPlayerOptions;
            BuildAlias = buildAlias;

            TargetPlatform = EditorUserBuildSettings.activeBuildTarget;
        }

        public BuildTarget TargetPlatform { get; }
        public BuildPlayerOptions BuildPlayerOptions => m_BuildPlayerOptions;
        public string BuildAlias { get; }

        public void SetScenes(string[] scenes)
        {
            m_BuildPlayerOptions.scenes = scenes;
        }
    }
}