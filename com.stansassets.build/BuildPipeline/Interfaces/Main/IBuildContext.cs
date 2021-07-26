using UnityEditor;

namespace StansAssets.Build.Pipeline
{
    /// <summary>
    /// Provide information about the build context.
    /// </summary>
    public interface IBuildContext
    {
        /// <summary>
        /// The platform that the build was created for.
        /// </summary>
        BuildTarget TargetPlatform { get; }

        /// <summary>
        /// The platform group the build was created for.
        /// </summary>
        BuildTargetGroup PlatformGroup { get; }

        /// <summary>
        /// The BuildOptions used for the build, as passed to BuildPipeline.BuildPlayer.
        /// </summary>
        BuildOptions Options { get; }
    }
}
