using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace StansAssets.Build.Editor
{
    public class UnityPlayerBuildStep : IBuildStep
    {
        private string m_resultMessage;

        private int m_Priority;

        public int Priority => m_Priority;

        public bool Execute(BuildContext buildContext)
        {
            return BuildProject();
        }

        public string GetResultMessage()
        {
            return m_resultMessage;
        }

        private bool BuildProject()
        {
            bool isSuccess = true;

            BuildPlayerOptions defaultBuildPlayerOptions = new BuildPlayerOptions();
            BuildPlayerOptions currentBuildPlayerOptions =
                BuildPlayerWindow.DefaultBuildMethods.GetBuildPlayerOptions(defaultBuildPlayerOptions);

            BuildReport report = BuildPipeline.BuildPlayer(currentBuildPlayerOptions);

            BuildSummary summary = report.summary;

            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
            }
            else if (summary.result == BuildResult.Failed)
            {
                Debug.Log("Build failed");
                isSuccess = false;
            }

            return isSuccess;
        }
    }
}
