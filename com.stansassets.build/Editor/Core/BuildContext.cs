using UnityEditor;

namespace StansAssets.Build.Editor
{
    /// <summary>
    /// Build configuration data
    /// </summary>
    public class BuildContext : IBuildContext
    {
        public BuildContext(BuildPlayerOptions buildPlayerOptions, string buildAlias = null)
        {
            BuildPlayerOptions = buildPlayerOptions;
            BuildAlias = buildAlias;

            TargetPlatform = EditorUserBuildSettings.activeBuildTarget;
        }

        public BuildTarget TargetPlatform { get; }
        public BuildPlayerOptions BuildPlayerOptions  { get; }
        public string BuildAlias { get; }
    }
}