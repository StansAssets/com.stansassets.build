using System;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Callbacks;
using UnityEngine;

namespace StansAssets.Build.Pipeline
{
    class BuildProcessor : IPreprocessBuildWithReport
    {
        static IBuildTasksContainer s_BuildTasks;
        static IBuildContext s_BuildContext;

        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            s_BuildContext = new BuildContext(report);
            s_BuildTasks = GenerateBuildStepsContainer();
            foreach (var task in s_BuildTasks.PreBuildTasks)
            {
                RunBuildTask(s_BuildContext, task);
            }
        }

        [PostProcessBuild(0)]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
        {
            foreach (var task in s_BuildTasks.PostBuildTasks)
            {
                RunBuildTask(s_BuildContext, task);
            }
        }

        static void RunBuildTask(IBuildContext buildContext, IBuildTask task)
        {
            switch (task)
            {
                case IAsyncBuildTask asyncBuildTask:
                    //TODO run asyncBuildTask
                    break;
                case ISyncBuildTask buildTask:
                    buildTask.Run(buildContext);
                    break;
                default:
                    throw new InvalidOperationException($"Unknown task type: {task.GetType().FullName}");
            }
        }

        [PostProcessScene(0)]
        public static void OnPostProcessScene()
        {
            if (Application.isPlaying)
                return;

            var postProcessSceneTasks = s_BuildTasks.ScenePostProcessTasks;
            if (postProcessSceneTasks.Count == 0)
                return;

            var sceneTasksRunner = new ScenePostProcessTasksRunner();
            sceneTasksRunner.Run(s_BuildContext, postProcessSceneTasks);
        }

        public static IBuildTasksContainer GenerateBuildStepsContainer()
        {
            var stepsProvider = CreateBuildTasksProvider();
            return stepsProvider.GetBuildSteps(new UserEditorBuildSettings());
        }

        public static string GetProviderName()
        {
            return CreateBuildTasksProvider().GetType().Name;
        }

        static IBuildTasksProvider CreateBuildTasksProvider()
        {
            return new DefaultBuildTasksProvider();
        }
    }
}
