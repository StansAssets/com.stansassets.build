using StansAssets.Foundation;
using System;

namespace StansAssets.Build.Pipeline
{
    class DefaultBuildTasksProvider : IBuildTasksProvider
    {
        public IBuildTasksContainer GetBuildSteps(IUserEditorBuildSettings buildSettings)
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
    }
}
