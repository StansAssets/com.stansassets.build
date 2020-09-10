using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace StansAssets.Build.Editor
{
    public class UnityPlayerBuildStep : IBuildStep
    {
        private int m_Priority;

        public int Priority => m_Priority;

        public bool Execute(BuildContext buildContext)
        {
            return BuildProject();
        }
        
        private bool BuildProject()
        {
            bool isSuccess = true;

            BuildPlayerOptions defaultBuildPlayerOptions = new BuildPlayerOptions();
            BuildPlayerOptions currentBuildPlayerOptions =
                BuildPlayerWindow.DefaultBuildMethods.GetBuildPlayerOptions(defaultBuildPlayerOptions);

           // currentBuildPlayerOptions.locationPathName = "";
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
