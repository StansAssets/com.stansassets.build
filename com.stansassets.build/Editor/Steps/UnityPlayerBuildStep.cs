using System;
using UnityEditor;
using UnityEditor.Build.Reporting;

namespace StansAssets.Build.Editor
{
    class UnityPlayerBuildStep : IBuildStep
    {
        private int m_Priority;

        public int Priority => m_Priority;

        private event Action<BuildStepResultArgs> m_OnCompleteCallback;

        public void Execute(BuildContext buildContext, Action<BuildStepResultArgs> onComplete = null)
        {
            m_OnCompleteCallback = onComplete;
            
            BuildProject(buildContext);
        }

        private void BuildProject(BuildContext buildContext)
        {
            BuildPlayerOptions defaultBuildPlayerOptions = new BuildPlayerOptions();
            BuildPlayerOptions currentBuildPlayerOptions =
                BuildPlayerWindow.DefaultBuildMethods.GetBuildPlayerOptions(defaultBuildPlayerOptions);

            currentBuildPlayerOptions.target = buildContext.TargetPlatform;

            BuildReport report = BuildPipeline.BuildPlayer(currentBuildPlayerOptions);

            BuildSummary summary = report.summary;
            
            BuildStepResultArgs resultArgs = new BuildStepResultArgs
            {    
                Step = this,
                IsSuccess = summary.result == BuildResult.Succeeded,
                ResultMessage = "UnityPlayerBuildStep execution finish is " + summary.result,
            };
            
            m_OnCompleteCallback?.Invoke(resultArgs);
            m_OnCompleteCallback = null;
        }
    }
}
