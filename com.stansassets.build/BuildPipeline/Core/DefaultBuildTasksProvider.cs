using StansAssets.Foundation;
using System;
using UnityEditor.Build;

namespace StansAssets.Build.Pipeline
{
    class DefaultBuildTasksProvider : IBuildTasksProvider
    {
        public IBuildTasksContainerFull GetBuildSteps(IUserEditorBuildSettings buildSettings)
        {
            var tasksContainer = new BuildTasksContainer();

            CollectBuildTasks(tasksContainer);
            CollectScenePostProcessTasks(tasksContainer);

            CollectUnityBuildTasks<IPreprocessBuildWithReport>(tasksContainer.AddPreProcessBuildTask);
            CollectUnityBuildTasks<IPostprocessBuildWithReport>(tasksContainer.AddPostProcessBuildTask);
            CollectUnityBuildTasks<IProcessSceneWithReport>(tasksContainer.AddProcessSceneTask);

            return tasksContainer;
        }

        /// <summary>
        /// Collects 'Pre' and 'Post' build tasks of type <see cref="IBuildTask"/>
        /// </summary>
        static void CollectBuildTasks(BuildTasksContainer tasksContainer)
        {
            var buildTasks = ReflectionUtility.FindImplementationsOf<IBuildTask>();

            foreach (var taskType in buildTasks)
            {
                if (ReflectionUtility.HasDefaultConstructor(taskType))
                {
                    var task = Activator.CreateInstance(taskType) as IBuildTask;
                    switch (task)
                    {
                        case IPreProcessTask _:
                        case IAsyncPreProcessTask _:
                            tasksContainer.AddPreProcessTask(task);
                            break;
                        case IPostProcessTask _:
                        case IAsyncPostProcessTask _:
                            tasksContainer.AddPostProcessTask(task);
                            break;
                        default:
                            throw new InvalidOperationException($"Unknown task type: {taskType.FullName}");
                    }
                }
            }
        }

        static void CollectScenePostProcessTasks(BuildTasksContainer tasksContainer)
        {
            var scenePostProcessSteps = ReflectionUtility.FindImplementationsOf<IScenePostProcessTask>();
            foreach (var stepType in scenePostProcessSteps)
            {
                if (ReflectionUtility.HasDefaultConstructor(stepType))
                {
                    var buildStep = Activator.CreateInstance(stepType) as IScenePostProcessTask;
                    tasksContainer.AddScenePostProcessTask(buildStep);
                }
            }
        }

        static void CollectUnityBuildTasks<T>(Action<T> addBuildTask)
            where T : class, IOrderedCallback
        {
            var taskTypes = ReflectionUtility.FindImplementationsOf<T>(ignoreBuiltIn: true);
            foreach (var taskType in taskTypes)
            {
                if (ReflectionUtility.HasDefaultConstructor(taskType))
                {
                    addBuildTask(Activator.CreateInstance(taskType) as T);
                }
            }
        }
    }
}
