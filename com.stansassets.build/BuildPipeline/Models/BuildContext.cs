using UnityEditor;
using UnityEditor.Build.Reporting;

namespace StansAssets.Build.Pipeline
{
    class BuildContext : IBuildContext
    {
        readonly BuildReport m_BuildReport;
        public BuildTarget TargetPlatform => m_BuildReport.summary.platform;
        public BuildTargetGroup PlatformGroup => m_BuildReport.summary.platformGroup;
        public BuildOptions Options => m_BuildReport.summary.options;

        public BuildContext(BuildReport buildReport)
        {
            m_BuildReport = buildReport;
        }
    }
}
