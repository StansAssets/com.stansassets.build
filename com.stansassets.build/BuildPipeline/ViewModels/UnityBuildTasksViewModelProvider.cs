using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Callbacks;
using UnityEngine;

namespace StansAssets.Build.Pipeline
{
    public class UnityBuildTasksViewModelProvider : IBuildStepsViewModelProvider
    {
        public IBuildStepsViewModelContainer GetBuildSteps()
        {
            var stepsContainer = new BuildStepsViewModelSortedContainer();

            CollectPreProcessTasks(stepsContainer);

            return stepsContainer;
        }

        static void CollectPreProcessTasks(BuildStepsViewModelSortedContainer sortedContainer)
        {
            var preBuildStepsWithReport = CollectUnityBuildStepsWithReport<IPreprocessBuildWithReport>();
            var sceneProcessStepsWithReport = CollectUnityBuildStepsWithReport<IProcessSceneWithReport>();
            var postBuildStepsWithReport = CollectUnityBuildStepsWithReport<IPostprocessBuildWithReport>();

            preBuildStepsWithReport.ForEach(sortedContainer.AddPreBuildStep);
            postBuildStepsWithReport.ForEach(sortedContainer.AddPostBuildStep);
            sceneProcessStepsWithReport.ForEach(sortedContainer.AddSceneProcessStep);

            var sceneProcessStepsWithAttributes = CollectUnityBuildStepsFromAttributes<PostProcessSceneAttribute>();
            var postBuildStepsWithAttributes = CollectUnityBuildStepsFromAttributes<PostProcessBuildAttribute>();

            sceneProcessStepsWithAttributes.ForEach(sortedContainer.AddSceneProcessStep);
            postBuildStepsWithAttributes.ForEach(sortedContainer.AddPostBuildStep);
        }

        static List<BuildStepViewModel> CollectUnityBuildStepsWithReport<T>() where T : class, IOrderedCallback
        {
            var steps = new List<BuildStepViewModel>();
            var stepTypes = ReflectionUtils.FindImplementationsOf<T>(true);
            foreach (var stepType in stepTypes)
            {
                if (!ReflectionUtils.HasDefaultConstructor(stepType))
                {
                    Debug.LogError($"The class {stepType.FullName} has no parameterless constructor and will be ignored");
                    continue;
                }

                if (Activator.CreateInstance(stepType) is T step)
                {
                    steps.Add(new BuildStepViewModel(step.GetType().FullName, step.callbackOrder));
                }
            }

            return steps;
        }

        static List<BuildStepViewModel> CollectUnityBuildStepsFromAttributes<T>() where T : CallbackOrderAttribute
        {
            var steps = new List<BuildStepViewModel>();
            var methods = ReflectionUtils.FindMethodsWithAttributes<T>(true);
            foreach (var methodInfo in methods)
            {
                var attrData = CustomAttributeData.GetCustomAttributes(methodInfo).First(attr => attr.AttributeType == typeof(T));
                var callbackOrder = attrData.ConstructorArguments.Count > 0
                    ? (int)attrData.ConstructorArguments[0].Value
                    : 0;

                if (methodInfo.DeclaringType != null)
                {
                    steps.Add(new BuildStepViewModel(methodInfo.DeclaringType?.FullName, callbackOrder));
                }
            }

            return steps;
        }
    }
}
