using System;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StansAssets.Build.Pipeline
{
    class BuildProcessor : IPreprocessBuildWithReport, IProcessSceneWithReport, IPostprocessBuildWithReport
    {
        const int k_CallbackOrder = 0;

        static IBuildTasksContainer s_BuildTasks;
        static IBuildContext s_BuildContext;

        public int callbackOrder => k_CallbackOrder;

        public void OnPreprocessBuild(BuildReport report)
        {
            s_BuildContext = new BuildContext(report);
            s_BuildTasks = GenerateBuildTasksContainer();
            foreach (var task in s_BuildTasks.PreBuildTasks)
            {
                RunBuildTask(s_BuildContext, task);
            }
        }

        public void OnProcessScene(Scene scene, BuildReport report)
        {
            if (Application.isPlaying)
                return;

            var postProcessSceneTasks = s_BuildTasks.ScenePostProcessTasks;
            if (postProcessSceneTasks.Count == 0)
                return;

            var sceneTasksRunner = new ScenePostProcessTasksRunner();
            sceneTasksRunner.Run(s_BuildContext, scene, postProcessSceneTasks);
        }

        public void OnPostprocessBuild(BuildReport report)
        {
            foreach (var task in s_BuildTasks.PostBuildTasks)
            {
                RunBuildTask(s_BuildContext, task);
            }
        }

        public static int GetCallbackOrder() => k_CallbackOrder;

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

        public static IBuildTasksContainer GenerateBuildTasksContainer()
        {
            var tasksProvider = CreateBuildTasksProvider();
            return tasksProvider.GetBuildTasks(new UserEditorBuildSettings());
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
