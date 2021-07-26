using UnityEditor;

namespace StansAssets.Build.Pipeline
{
    /// <summary>
    /// Build context provided by a package to define build steps.
    /// </summary>
    public interface IUserEditorBuildSettings
    {
        /// <summary>
        /// Currently selected build target.
        /// </summary>
        BuildTarget BuildTarget { get; }
    }
}
