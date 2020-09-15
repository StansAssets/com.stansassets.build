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
        private int m_Priority;

        public int Priority => m_Priority;

        private static List<IBuildTask> s_Tasks;
        
        private IBuildStepContext m_BuildContext;

        private event Action<BuildStepResultArgs> m_OnCompleteCallback;

        public UnityPlayerBuildStep(List<IBuildTask> tasks,int priority)
        {
            s_Tasks = tasks;
            m_Priority = priority;
        }
        
        public void Execute(IBuildStepContext buildContext, Action<BuildStepResultArgs> onComplete = null)
        {
            m_OnCompleteCallback = onComplete;

            m_BuildContext = buildContext;

            SetTasksContext();
            RunTasks();
                
            BuildProject();
        }

        private void BuildProject()
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

        private void SetTasksContext()
        {
            for (int i = 0; i < s_Tasks.Count; i++)
            {
                s_Tasks[i].SetContext(m_BuildContext);
            }
        }
        
        [PostProcessScene(1)]
        private static void RunTasks()
        {    
            if(Application.isPlaying)
                return;
            
            Scene currentScene = SceneManager.GetActiveScene();
            GameObject[] rootGameObjects = currentScene.GetRootGameObjects();

            List<GameObject> currentSceneObjects = GetAllObjects(rootGameObjects);

            for (int i = 0; i < s_Tasks.Count; i++)
            {
                for (int j = 0; j < currentSceneObjects.Count; j++)
                {
                    List<Component> allObjectComponents = GetAllComponents(currentSceneObjects[j]);
                    s_Tasks[i].OnPostprocessGameObject(currentSceneObjects[j], allObjectComponents);
                }
            }
        }

        private static List<GameObject> GetAllObjects(GameObject[] rootGameObjects)
        {
            List<GameObject> allObjects = new List<GameObject>();
            
            for (int i = 0; i < rootGameObjects.Length; i++)
            {
                var componentsInChildren =
                    rootGameObjects[i].GetComponentsInChildren<Transform>();

                for (int j = 0; j < componentsInChildren.Length; j++)
                {
                    allObjects.Add(componentsInChildren[j].gameObject);
                }
            }

            return allObjects;
        }
        
        private static List<Component> GetAllComponents(GameObject rootObject)
        {
            List<Component> allComponents = rootObject.GetComponents(typeof(MonoBehaviour)).ToList();
            
            return allComponents;
        }
    }
}
