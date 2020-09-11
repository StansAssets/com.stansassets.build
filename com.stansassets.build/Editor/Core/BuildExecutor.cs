using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace StansAssets.Build.Editor
{    
    /// <summary>
    /// Run registered steps and tasks with build on result
    /// </summary>
    public static class BuildExecutor
    {
        private static List<IBuildStep> s_Steps = new List<IBuildStep>();
        private static List<IBuildTask> s_Task = new List<IBuildTask>();

        private static IBuildStep s_CurrentStep;

        private static BuildContext s_BuildContext;
        
        /// <summary>
        /// Add IBuildStep object to build pipeline as a step
        /// </summary>
        /// <param name="step">Build step</param>
        public static void RegisterStep(IBuildStep step)
        {
            s_Steps.Add(step);
        }
        
        /// <summary>
        /// Add IBuildTask object to build pipeline as a task
        /// </summary>
        /// <param name="buildTask">Build task</param>
        public static void RegisterScenePostprocessTask(IBuildTask buildTask)
        {
            s_Task.Add(buildTask);
        }
        
        /// <summary>
        /// Run build process with included steps and tasks
        /// </summary>
        /// <param name="buildContext">Data class with necessary parameters for build execution</param>
        public static void Build(BuildContext buildContext)
        {
            s_BuildContext = buildContext;
            
            RegisterUnityPlayerBuildStep();
            
            SortTasks();
            SortSteps();

            RunTasks();
            RunNextStep();
        }
        
        private static void SortSteps()
        {
            s_Steps.Sort((x, y) => x.Priority.CompareTo(y.Priority));
        }

        private static void SortTasks()
        {
            s_Task.Sort((x, y) => x.Priority.CompareTo(y.Priority));
        }

        private static void RegisterUnityPlayerBuildStep()
        {    
            RegisterStep(new UnityPlayerBuildStep());
        }

        private static void RunNextStep()
        {
            if (s_Steps.Count == 0)
            {
                OnStepsCompleted();
                return;
            }

            s_CurrentStep = s_Steps[0];
            
            s_CurrentStep.Execute(s_BuildContext,OnBuildStepCompleted);
        }
        
        private static void OnBuildStepCompleted(BuildStepResultArgs stepExecuteResultArgs)
        {
            if (stepExecuteResultArgs.IsSuccess)
            {
                ReleaseCurrentAndRunNextStep();
            }
            else
            {
                OnStepCompleteFailed(stepExecuteResultArgs);
            }
        }

        private static void ReleaseCurrentAndRunNextStep()
        {
            RemoveCurrentStep();
            RunNextStep();
        }

        private static void OnStepCompleteFailed(BuildStepResultArgs stepExecuteResultArgs)
        {
            Debug.LogError("Build Executor : " + stepExecuteResultArgs.ResultMessage);
            ClearSteps();
        }
        
        private static void OnStepsCompleted()
        {
            ClearSteps();
            ClearTasks();
        }

        private static void RunTasks()
        {
            foreach (var task in s_Task)
            {
                task.OnPostprocessScene();
            }
        }
        
        private static void RemoveCurrentStep()
        {
            s_Steps.Remove(s_CurrentStep);
        }
        
        private static void ClearTasks()
        {
            s_Task.Clear();
        }
        
        private static void ClearSteps()
        {
            s_Steps.Clear();
        }
    }
}