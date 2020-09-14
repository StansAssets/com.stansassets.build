using UnityEditor;

namespace StansAssets.Build.Editor
{    
    /// <summary>
    /// Build configuration data
    /// </summary>
    public class BuildContext : IBuildStepContext
    {
        public BuildTarget TargetPlatform => m_TargetPlatform;
        
        public BuildPlayerOptions BuildPlayerOptions => m_BuildPlayerOptions;

        private BuildTarget m_TargetPlatform;
        
        private BuildPlayerOptions m_BuildPlayerOptions;
        
        public BuildContext(BuildTarget targetPlatform,BuildPlayerOptions buildPlayerOptions)
        {
            m_TargetPlatform = targetPlatform;
            m_BuildPlayerOptions = buildPlayerOptions;
        }
    }
}