﻿using System;
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
        public int Priority => 0;
        public string Name => "UnityPlayerBuildStep";

        static List<IBuildTask> s_Tasks;
        IBuildContext m_BuildContext;
        Action<BuildStepResultArgs> m_OnCompleteCallback = delegate { };

        public UnityPlayerBuildStep(List<IBuildTask> tasks)
        {
            s_Tasks = tasks;
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
            BuildReport report = BuildPipeline.BuildPlayer(m_BuildContext.BuildPlayerOptions);

            BuildSummary summary = report.summary;

            BuildStepResultArgs resultArgs = new BuildStepResultArgs
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
            if(Application.isPlaying)
                return;

            Scene currentScene = SceneManager.GetActiveScene();
            GameObject[] rootGameObjects = currentScene.GetRootGameObjects();
            Dictionary<GameObject, List<Component>> componentsMap = FetchComponentsMap(rootGameObjects);

            for (int i = 0; i < s_Tasks.Count; i++)
            {
                s_Tasks[i].OnPostprocessScene(currentScene);
                foreach (var pair in componentsMap)
                {
                    var gameObject = pair.Key;
                    var components = pair.Value;

                    if (gameObject == null)
                        continue;

                    s_Tasks[i].OnPostprocessGameObject(gameObject, components);
                }
            }

        }

        static Dictionary<GameObject, List<Component>> FetchComponentsMap(GameObject[] rootGameObjects)
        {
            var componentsMap = new Dictionary<GameObject, List<Component>>();
            for (int i = 0; i < rootGameObjects.Length; i++)
            {
                if(rootGameObjects[i] == null)
                    continue;

                StoreObjectsAndComponents(rootGameObjects[i], componentsMap);
            }
            return componentsMap;
        }

        static readonly List<Transform> s_TempTransformsCollection = new List<Transform>();
        static readonly List<Component> s_TempComponentsCollection = new List<Component>();
        static void StoreObjectsAndComponents(GameObject rootGameObject, Dictionary<GameObject, List<Component>> componentsMap)
        {
            s_TempTransformsCollection.Clear();
            s_TempComponentsCollection.Clear();
            rootGameObject.GetComponentsInChildren<Transform>(s_TempTransformsCollection);
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
    }
}
