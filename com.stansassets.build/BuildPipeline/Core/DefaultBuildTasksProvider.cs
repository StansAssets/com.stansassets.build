using StansAssets.Foundation;
using System;
using UnityEngine;

namespace StansAssets.Build.Pipeline
{
    class DefaultBuildTasksProvider : IBuildTasksProvider
    {
        public IBuildTasksContainer GetBuildTasks(IUserEditorBuildSettings buildSettings)
        {
            var tasksContainer = new BuildTasksContainer();

            CollectBuildTasks(tasksContainer);
            CollectScenePostProcessTasks(tasksContainer);

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
                if (!HasDefaultConstructor(taskType))
                    continue;

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

        static void CollectScenePostProcessTasks(BuildTasksContainer tasksContainer)
        {
            var scenePostProcessTasks = ReflectionUtility.FindImplementationsOf<IScenePostProcessTask>();
            foreach (var taskType in scenePostProcessTasks)
            {
                if (!HasDefaultConstructor(taskType))
                    continue;

                var buildStep = Activator.CreateInstance(taskType) as IScenePostProcessTask;
                tasksContainer.AddScenePostProcessTask(buildStep);
            }
        }

        static bool HasDefaultConstructor(Type taskType)
        {
            var hasConstructor = ReflectionUtils.HasDefaultConstructor(taskType);
            if (!hasConstructor)
            {
                Debug.LogError($"The task {taskType.FullName} has no parameterless constructor and will be ignored");
            }

            return hasConstructor;
        }
    }
}
