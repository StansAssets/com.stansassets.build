using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace StansAssets.Build.Editor
{
    class UnityPlayerBuildStep : IBuildStep
    {
        private int m_Priority;

        public int Priority => m_Priority;

        public bool Execute(BuildContext buildContext)
        {
            return BuildProject(buildContext);
        }

        private bool BuildProject(BuildContext buildContext)
        {
            bool isSuccess = true;

            BuildPlayerOptions defaultBuildPlayerOptions = new BuildPlayerOptions();
            BuildPlayerOptions currentBuildPlayerOptions =
                BuildPlayerWindow.DefaultBuildMethods.GetBuildPlayerOptions(defaultBuildPlayerOptions);

            currentBuildPlayerOptions.target = buildContext.TargetPlatform;

            BuildReport report = BuildPipeline.BuildPlayer(currentBuildPlayerOptions);

            BuildSummary summary = report.summary;

            if (summary.result == BuildResult.Failed)
            {
                isSuccess = false;
            }

            return isSuccess;
        }
    }
}
