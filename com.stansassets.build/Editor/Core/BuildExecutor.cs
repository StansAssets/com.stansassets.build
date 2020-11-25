using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

namespace StansAssets.Build.Editor
{
    /// <summary>
    /// Run registered steps and tasks with build on result
    /// </summary>
    [InitializeOnLoad]
    public static class BuildExecutor
    {
        static readonly List<IBuildStep> s_Steps = new List<IBuildStep>();
        static readonly List<IBuildTask> s_Tasks = new List<IBuildTask>();

        static IBuildStep s_CurrentStep;

        static IBuildContext s_BuildContext;

        static BuildExecutor()
        {
            Settings = new BuildSettings();
          //  RegisterListeners(new BuildContext(BuildExecutorUtility.BuildPlayerOptions, Settings));
        }

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
        public static void Build(IBuildContext buildContext)
        {
            s_BuildContext = buildContext;

            RegisterListeners(buildContext);

            RegisterUnityPlayerBuildStep();
            SortTasks();
            SortSteps();


            RunNextStep();
        }

        /// <summary>
        /// Getting all types that implement an interface IBuildExecutorListener and run Register method
        /// </summary>
        /// <param name="buildContext">Data class with necessary parameters for build execution</param>
        internal static void RegisterListeners(IBuildContext buildContext)
        {
            var buildExecutorType = typeof(IBuildExecutorListener);
            var scriptsWithBuildExecutorListener = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => buildExecutorType.IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract);

            var listeners = new List<IBuildExecutorListener>();
            foreach (var buildExecutorListener in scriptsWithBuildExecutorListener)
            {
                if (Activator.CreateInstance(buildExecutorListener) is IBuildExecutorListener listener && listener.Active)
                {
                    listeners.Add(listener);
                }
            }

            BuildExecutorUtility.CheckListenerPriorities(listeners);

            listeners.Sort((a,b) => a.Priority.CompareTo(b.Priority));

            foreach (var listener in listeners)
            {
                listener.Register(buildContext);
            }
        }

        static void SortSteps()
        {
            s_Steps.Sort((x, y) => x.Priority.CompareTo(y.Priority));
        }

        static void SortTasks()
        {
            s_Tasks.Sort((x, y) => x.Priority.CompareTo(y.Priority));
        }

        static void RegisterUnityPlayerBuildStep()
        {
            RegisterStep(new UnityPlayerBuildStep(s_Tasks));
        }

        static void RunNextStep()
        {
            if (s_Steps.Count == 0)
            {
                OnStepsCompleted();
                return;
            }

            s_CurrentStep = s_Steps[0];

            s_CurrentStep.Execute(s_BuildContext,OnStepCompleted);
        }

        static void OnStepCompleted(BuildStepResultArgs stepExecuteResultArgs)
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

        static void ReleaseCurrentAndRunNextStep()
        {
            RemoveCurrentStep();
            RunNextStep();
        }

        static void OnStepFailed(BuildStepResultArgs stepExecuteResultArgs)
        {
            Debug.LogError("Build Executor : " + stepExecuteResultArgs.ResultMessage);
            ClearSteps();
        }

        static void OnStepsCompleted()
        {
            OnBuildFinished();
            ClearSteps();
            ClearTasks();
        }

        static void RemoveCurrentStep()
        {
            s_Steps.Remove(s_CurrentStep);
        }

        static void ClearTasks()
        {
            s_Tasks.Clear();
        }

        static void ClearSteps()
        {
            s_Steps.Clear();
        }

        static void OnBuildFinished()
        {
            foreach (var task in s_Tasks)
            {
                task.OnBuildFinished();
            }
        }

        /// <summary>
        /// Please use this property to pass data during build pipeline. Check <see cref="BuildSettings.AddData"/>.
        /// </summary>
        public static BuildSettings Settings { get; }

        internal static IReadOnlyCollection<IBuildStep> Steps => s_Steps;
        internal static IReadOnlyCollection<IBuildTask> Tasks => s_Tasks;
    }
}
