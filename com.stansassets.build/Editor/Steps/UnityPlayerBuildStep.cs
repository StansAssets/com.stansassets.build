using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StansAssets.Build.Editor
{
    class UnityPlayerBuildStep : IBuildStep
    {
        static List<IBuildTask> s_Tasks = new List<IBuildTask>();

        IBuildContext m_BuildContext;
        Action<BuildStepResultArgs> m_OnCompleteCallback = delegate { };

        public UnityPlayerBuildStep(List<IBuildTask> tasks)
        {
            s_Tasks.Clear();
            s_Tasks.AddRange(tasks);
        }

        public void Execute(IBuildContext buildContext, Action<BuildStepResultArgs> onComplete = null)
        {
            m_OnCompleteCallback = onComplete;
            m_BuildContext = buildContext;

            SetContext();
            BuildProject();
        }

        void BuildProject()
        {
            var report = BuildPipeline.BuildPlayer(m_BuildContext.BuildPlayerOptions);
            var summary = report.summary;
            var resultArgs = new BuildStepResultArgs
            {
                Step = this,
                IsSuccess = summary.result == BuildResult.Succeeded,
                ResultMessage = "UnityPlayerBuildStep finished with: " + summary.result,
            };

            m_OnCompleteCallback?.Invoke(resultArgs);
            m_OnCompleteCallback = null;
        }

        void SetContext()
        {
            for (int i = 0; i < s_Tasks.Count; i++)
            {
                s_Tasks[i].SetContext(m_BuildContext);
            }
        }

        [PostProcessScene(1)]
        static void RunTasks()
        {
            if(Application.isPlaying || !s_Tasks.Any())
                return;

            var currentScene = SceneManager.GetActiveScene();
            var rootGameObjects = currentScene.GetRootGameObjects();
            var componentsMap = GetComponentsMap(rootGameObjects);

            foreach (var task in s_Tasks)
            {
                task.OnPostprocessScene(currentScene);
                foreach (var pair in componentsMap)
                {
                    var gameObject = pair.Key;
                    var components = pair.Value;

                    if (gameObject == null)
                        continue;

                    task.OnPostprocessGameObject(gameObject, components);
                }
            }
        }

        static Dictionary<GameObject, List<Component>> GetComponentsMap(GameObject[] rootGameObjects)
        {
            var componentsMap = new Dictionary<GameObject, List<Component>>();
            for (int i = 0; i < rootGameObjects.Length; i++)
            {
                if(rootGameObjects[i] == null)
                    continue;

                FetchHierarchy(rootGameObjects[i], componentsMap);
            }
            return componentsMap;
        }

        static readonly List<Component> s_TempComponentsCollection = new List<Component>();
        static void FetchHierarchy(GameObject rootGameObject, Dictionary<GameObject, List<Component>> componentsMap)
        {
            s_TempComponentsCollection.Clear();
            rootGameObject.GetComponentsInChildren<Component>(s_TempComponentsCollection);

            for (var i = 0; i < s_TempComponentsCollection.Count; ++i)
            {
                var component = s_TempComponentsCollection[i];
                if(component == null)
                    continue;

                if (componentsMap.TryGetValue(component.gameObject, out var components) == false)
                {
                    components = new List<Component>();
                    componentsMap[component.gameObject] = components;
                }
                components.Add(component);
            }
        }

        public int Priority => 0;
        public string Name => "UnityPlayerBuildStep";
    }
}
