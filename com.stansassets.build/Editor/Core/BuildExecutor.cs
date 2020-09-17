using System;
using System.Collections.Generic;
using UnityEngine;

namespace StansAssets.Build.Editor
{
    /// <summary>
    /// Run registered steps and tasks with build on result
    /// </summary>
    public static class BuildExecutor
    {
        private static List<IBuildStep> s_Steps = new List<IBuildStep>();
        private static List<IBuildTask> s_Tasks = new List<IBuildTask>();

        private static IBuildStep s_CurrentStep;

        private static IBuildContext s_BuildContext;

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
            s_Tasks.Add(buildTask);
        }

        /// <summary>
        /// Run build process with included steps and tasks
        /// </summary>
        /// <param name="buildContext">Data class with necessary parameters for build execution</param>
        public static void Build(BuildContext buildContext)
        {
            s_BuildContext = buildContext;

            RegisterAllIBuildExecutorListener(buildContext);

            SortTasks();
            SortSteps();

            RegisterUnityPlayerBuildStep();

            RunNextStep();
        }

        /// <summary>
        /// Getting all types that implement an interface IBuildExecutorListener and run Register method
        /// </summary>
        /// <param name="buildContext">Data class with necessary parameters for build execution</param>
        internal static void RegisterAllIBuildExecutorListener(BuildContext buildContext)
        {
            var buildExecutorType = typeof(IBuildExecutorListener);
            var scriptsWithBuildExecutorListener = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => buildExecutorType.IsAssignableFrom(p) && !buildExecutorType.IsInterface && !buildExecutorType.IsAbstract);

            foreach (var buildexecutorListener in scriptsWithBuildExecutorListener)
            {
                if (Activator.CreateInstance(buildexecutorListener) is IBuildExecutorListener listener && listener.Active)
                {
                    listener.Register(buildContext);
                }
            }
        }

        private static void SortSteps()
        {
            s_Steps.Sort((x, y) => x.Priority.CompareTo(y.Priority));
        }

        private static void SortTasks()
        {
            s_Tasks.Sort((x, y) => x.Priority.CompareTo(y.Priority));
        }

        private static void RegisterUnityPlayerBuildStep()
        {
            RegisterStep(new UnityPlayerBuildStep(s_Tasks));
        }

        private static void RunNextStep()
        {
            if (s_Steps.Count == 0)
            {
                OnStepsCompleted();
                return;
            }

            s_CurrentStep = s_Steps[0];

            s_CurrentStep.Execute(s_BuildContext,OnStepCompleted);
        }

        private static void OnStepCompleted(BuildStepResultArgs stepExecuteResultArgs)
        {
            if (stepExecuteResultArgs.IsSuccess)
            {
                ReleaseCurrentAndRunNextStep();
            }
            else
            {
                OnStepFailed(stepExecuteResultArgs);
            }
        }

        private static void ReleaseCurrentAndRunNextStep()
        {
            RemoveCurrentStep();
            RunNextStep();
        }

        private static void OnStepFailed(BuildStepResultArgs stepExecuteResultArgs)
        {
            Debug.LogError("Build Executor : " + stepExecuteResultArgs.ResultMessage);
            ClearSteps();
        }

        private static void OnStepsCompleted()
        {
            OnBuildFinished();
            ClearSteps();
            ClearTasks();
        }

        private static void RemoveCurrentStep()
        {
            s_Steps.Remove(s_CurrentStep);
        }

        private static void ClearTasks()
        {
            s_Tasks.Clear();
        }

        private static void ClearSteps()
        {
            s_Steps.Clear();
        }

        private static void OnBuildFinished()
        {
            foreach (var task in s_Tasks)
            {
                task.OnBuildFinished();
            }
        }
    }
}