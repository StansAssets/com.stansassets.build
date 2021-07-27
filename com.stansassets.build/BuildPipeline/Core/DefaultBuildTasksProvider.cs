using System;
using StansAssets.Foundation;

namespace StansAssets.Build.Pipeline
{
    class DefaultBuildTasksProvider : IBuildTasksProvider
    {
        public IBuildTasksContainer GetBuildSteps(IUserEditorBuildSettings buildSettings)
        {
            var tasksContainer = new BuildTasksContainer();
            var buildTasks = ReflectionUtility.FindImplementationsOf<IBuildTask>();
            foreach (var taskType in buildTasks)
            {
                //TODO check if type has empty constructor and throw appropriate exception if it's not
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
                        throw new InvalidOperationException($"Unknown task type: {task.GetType().FullName}");
                }
            }

            var scenePostProcessSteps = ReflectionUtility.FindImplementationsOf<IScenePostProcessTask>();
            foreach (var stepType in scenePostProcessSteps)
            {
                var buildStep = Activator.CreateInstance(stepType) as IScenePostProcessTask;
                tasksContainer.AddScenePostProcessTask(buildStep);
            }

            return tasksContainer;
        }
    }
}
