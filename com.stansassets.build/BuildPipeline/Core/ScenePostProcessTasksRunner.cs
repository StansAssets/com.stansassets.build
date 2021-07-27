using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StansAssets.Build.Pipeline
{
    class ScenePostProcessTasksRunner
    {
        readonly List<Component> m_TempComponentsCollection = new List<Component>();

        public void Run(IBuildContext buildContext, IReadOnlyList<IScenePostProcessTask> postProcessSceneTasks)
        {
            var currentScene = SceneManager.GetActiveScene();
            var rootGameObjects = currentScene.GetRootGameObjects();
            var componentsMap = GetComponentsMap(rootGameObjects);

            foreach (var task in postProcessSceneTasks)
            {
                task.OnPostprocessScene(buildContext, currentScene);
                foreach (var pair in componentsMap)
                {
                    var gameObject = pair.Key;
                    var components = pair.Value;

                    if (gameObject == null)
                        continue;

                    task.OnPostprocessGameObject(buildContext, gameObject, components);
                }
            }
        }

        Dictionary<GameObject, List<Component>> GetComponentsMap(GameObject[] rootGameObjects)
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

        void FetchHierarchy(GameObject rootGameObject, Dictionary<GameObject, List<Component>> componentsMap)
        {
            m_TempComponentsCollection.Clear();
            rootGameObject.GetComponentsInChildren(m_TempComponentsCollection);

            for (var i = 0; i < m_TempComponentsCollection.Count; ++i)
            {
                var component = m_TempComponentsCollection[i];
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
