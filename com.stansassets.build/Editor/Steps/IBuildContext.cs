using UnityEditor;

namespace StansAssets.Build.Editor
{
    /// <summary>
    /// Build parameters
    /// </summary>
    public interface IBuildContext
    {
        /// <summary>
        /// Platform to build
        /// </summary>
        BuildTarget TargetPlatform { get; }

        /// <summary>
        /// Build settings
        /// </summary>
        BuildPlayerOptions BuildPlayerOptions { get; }

        /// <summary>
        /// Key value
        /// </summary>
        string BuildAlias { get; }


        /// <summary>
        /// Sets scene paths for a build. Will affect build only in case if called before UnityPlayerBuildStep (has priority lower than 0)
        /// </summary>
        /// <param name="scenes">Array with scenes path</param>
        void SetScenes(string[] scenes);
    }
}
