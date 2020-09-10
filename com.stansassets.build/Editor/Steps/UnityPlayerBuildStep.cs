using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using System;

namespace StansAssets.Build.Editor
{
    public class UnityPlayerBuildStep : IBuildStep
    {
        private string m_resultMessage;

        private int m_Priority;

        public int Priority => m_Priority;

        public event Action<ExecuteFinishedArgs> OnExecuteFinished;

        public void Execute(BuildContext buildContext)
        {
             BuildProject();
        }

        private void BuildProject()
        {
            BuildPlayerOptions defaultBuildPlayerOptions = new BuildPlayerOptions();
            BuildPlayerOptions currentBuildPlayerOptions =
                BuildPlayerWindow.DefaultBuildMethods.GetBuildPlayerOptions(defaultBuildPlayerOptions);

            ExecuteFinishedArgs executeFinishedArgs = new ExecuteFinishedArgs();

            BuildReport report = BuildPipeline.BuildPlayer(currentBuildPlayerOptions);

            BuildSummary summary = report.summary;
            executeFinishedArgs.args = summary;

            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
            }
            else if (summary.result == BuildResult.Failed)
            {
                Debug.Log("Build failed");
            }
            OnExecuteFinished.Invoke(executeFinishedArgs);
        }
    }
}
