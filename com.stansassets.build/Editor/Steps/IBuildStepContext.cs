using UnityEditor;

namespace StansAssets.Build.Editor
{
    /// <summary>
    /// Build parameters
    /// </summary>
    public interface IBuildStepContext
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
        string BuildAlias { get; set; }
    }
}
