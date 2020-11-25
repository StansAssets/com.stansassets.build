using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace StansAssets.Build.Editor
{
    static class BuildExecutorUtility
    {
        public static BuildPlayerOptions BuildPlayerOptions => GetBuildPlayerOptions(false, new BuildPlayerOptions());
        public static string DefaultBuildLocation => Path.Combine(Path.GetDirectoryName(Application.dataPath) ?? throw new UnityException("Application.dataPath is null!"),
                                                                  "Builds",
                                                                  EditorUserBuildSettings.activeBuildTarget.ToString());

        public static void CheckListenerPriorities(IEnumerable<IBuildExecutorListener> listenersToCheck)
        {
            Dictionary<int, List<IBuildExecutorListener>> prioritizedListeners = new Dictionary<int, List<IBuildExecutorListener>>();
            foreach (var listener in listenersToCheck)
            {
                if (prioritizedListeners.TryGetValue(listener.Priority, out var listeners) == false)
                {
                    listeners = new List<IBuildExecutorListener>();
                    prioritizedListeners[listener.Priority] = listeners;
                }
                listeners.Add(listener);
            }

            foreach (var pair in prioritizedListeners)
            {
                var priority = pair.Key;
                var listeners = pair.Value;
                if (listeners.Count > 1)
                {
                    Debug.LogWarning($"Undefined execution order for priority: {priority}, between listeners: " +
                        $"{string.Join(",", listeners.Select(l => l.GetType().Name))}! Please use unique IBuildExecutorListener.Priority.");
                }
            }
        }

        static BuildPlayerOptions GetBuildPlayerOptions(
            bool askForLocation = false,
            BuildPlayerOptions defaultOptions = new BuildPlayerOptions())
        {
            // Fake buildLocation path to prevent exception
            var locationPath = EditorUserBuildSettings.GetBuildLocation(EditorUserBuildSettings.activeBuildTarget);
            if (locationPath.Length == 0)
            {
                EditorUserBuildSettings.SetBuildLocation(EditorUserBuildSettings.activeBuildTarget, DefaultBuildLocation);
            }

            // Get static internal "GetBuildPlayerOptionsInternal" method
            var method = typeof(BuildPlayerWindow.DefaultBuildMethods).GetMethod(
                "GetBuildPlayerOptionsInternal",
                BindingFlags.NonPublic | BindingFlags.Static);

            // invoke internal method
            return (BuildPlayerOptions)method.Invoke(
                null,
                new object[] { askForLocation, defaultOptions});
        }
    }
}
