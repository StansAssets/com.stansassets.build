using System.Collections.Generic;

namespace StansAssets.Build.Editor
{
    public static class BuildExecutor
    {
        private static List<IBuildStep> s_Steps = new List<IBuildStep>();
        private static List<IBuildTask> s_Task = new List<IBuildTask>();

        public static void RegisterStep(IBuildStep step)
        {
            s_Steps.Add(step);
        }

        public static void RemoveStep(IBuildStep step)
        {
            s_Steps.Remove(step);
        }

        private static void ClearSteps()
        {
            s_Steps.Clear();
        }
        
        public static void RegisterScenePostprocessTask(IBuildTask buildTask)
        {
            s_Task.Add(buildTask);
        }

        public static void RemoveScenePostprocessTask(IBuildTask buildTask)
        {
            s_Task.Remove(buildTask);
        }

        private static void ClearTasks()
        {
            s_Task.Clear();
        }

        public static void Build(BuildContext buildContext, BuildMetadata metadata)
        {
            CreateBuildStep();
            SortSteps();
            SortTasks();
            RunSteps(buildContext);
            RunTasks(buildContext);
        }
        
        private static void SortSteps()
        {
            s_Steps.Sort((x, y) => x.Priority.CompareTo(y.Priority));
        }

        private static void SortTasks()
        {
            s_Task.Sort((x, y) => x.Priority.CompareTo(y.Priority));
        }

        private static void CreateBuildStep()
        {
            s_Steps.Add(new UnityPlayerBuildStep());
        }

        private static void RunSteps(BuildContext buildContext)
        {
            foreach (var step in s_Steps)
            {
                step.Execute(buildContext);
            }
        }

        private static void RunTasks(BuildContext buildContext)
        {
            foreach (var task in s_Task)
            {
                task.OnPostprocessScene(buildContext.TargetPlatform);
            }
        }
    }
}