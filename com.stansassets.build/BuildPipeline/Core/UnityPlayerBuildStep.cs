using System;
using System.Collections.Generic;
using System.Linq;
using StansAssets.Foundation.Models;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StansAssets.Build.Pipeline
{
    class UnityPlayerBuildStep : IBuildStep
    {
        static readonly List<IScenePostProcessStep> s_ScenePostProcessSteps = new List<IScenePostProcessStep>();
        static IBuildContext s_BuildContext;

        Action<Result> m_OnCompleteCallback;

        public string Name => nameof(UnityPlayerBuildStep);

        public UnityPlayerBuildStep(IEnumerable<IScenePostProcessStep> tasks)
        {
            s_ScenePostProcessSteps.Clear();
            s_ScenePostProcessSteps.AddRange(tasks);
        }

        public void Run(IBuildContext buildContext, Action<Result> onComplete)
        {
            m_OnCompleteCallback = onComplete;
            s_BuildContext = buildContext;
            BuildProject();
        }

        void BuildProject()
        {
            var report = BuildPipeline.BuildPlayer(s_BuildContext.BuildPlayerOptions);
            var summary = report.summary;

            if (summary.result != BuildResult.Succeeded)
            {
                var error = new Error((int)summary.result, $"UnityPlayerBuildStep finished with {summary.result} result");
                m_OnCompleteCallback.Invoke(new Result(error));
            }
            else
            {
                m_OnCompleteCallback.Invoke(new Result());
            }
        }

        [PostProcessScene(1)]
        static void RunTasks()
        {
            if (Application.isPlaying || !s_ScenePostProcessSteps.Any())
                return;

            var currentScene = SceneManager.GetActiveScene();
            var rootGameObjects = currentScene.GetRootGameObjects();
            var componentsMap = GetComponentsMap(rootGameObjects);

            foreach (var task in s_ScenePostProcessSteps)
            {
                task.OnPostprocessScene(s_BuildContext, currentScene);
                foreach (var pair in componentsMap)
                {
                    var gameObject = pair.Key;
                    var components = pair.Value;

                    if (gameObject == null)
                        continue;

                    task.OnPostprocessGameObject(s_BuildContext, gameObject, components);
                }
            }
        }

        static Dictionary<GameObject, List<Component>> GetComponentsMap(GameObject[] rootGameObjects)
        {
            var componentsMap = new Dictionary<GameObject, List<Component>>();
            for (int i = 0; i < rootGameObjects.Length; i++)
            {
                if (rootGameObjects[i] == null)
                    continue;

                FetchHierarchy(rootGameObjects[i], componentsMap);
            }

            return componentsMap;
        }

        static readonly List<Component> s_TempComponentsCollection = new List<Component>();

        static void FetchHierarchy(GameObject rootGameObject, Dictionary<GameObject, List<Component>> componentsMap)
        {
            s_TempComponentsCollection.Clear();
            rootGameObject.GetComponentsInChildren(s_TempComponentsCollection);

            for (var i = 0; i < s_TempComponentsCollection.Count; ++i)
            {
                var component = s_TempComponentsCollection[i];
                if (component == null)
                    continue;

                if (componentsMap.TryGetValue(component.gameObject, out var components) == false)
                {
                    components = new List<Component>();
                    componentsMap[component.gameObject] = components;
                }

                components.Add(component);
            }
        }
    }
}
