using System.Collections.Generic;
using UnityEngine;

namespace StansAssets.Build.Pipeline
{
    class BuildRunner
    {
        IBuildContext m_BuildContext;

        IBuildStep m_CurrentStep;
        Queue<IBuildStep> m_StepsQueue;

        public void Build(IBuildContext buildContext, IBuildStepsContainer buildSteps)
        {
            m_BuildContext = buildContext;
            m_StepsQueue = new Queue<IBuildStep>();
            foreach (var buildStep in buildSteps.PreBuildSteps)
            {
                m_StepsQueue.Enqueue(buildStep);
            }

            m_StepsQueue.Enqueue(new UnityPlayerBuildStep(buildSteps.ScenePostProcessStepsTasks));

            foreach (var buildStep in buildSteps.PostBuildSteps)
            {
                m_StepsQueue.Enqueue(buildStep);
            }

            RunNextStep();
        }

        void RunNextStep()
        {
            if (m_StepsQueue.Count == 0)
            {
                // TODO print some report here. total time, steps, etc
                Debug.Log("Build Finished!");
                return;
            }

            var step = m_StepsQueue.Dequeue();
            step.Run(m_BuildContext, result =>
            {
                if (result.IsSucceeded)
                {
                    RunNextStep();
                }
                else
                {
                    Debug.LogError($"Build Failed {result.Error.Message}");
                }
            });
        }
    }
}
