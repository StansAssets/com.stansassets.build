using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace StansAssets.Build.Editor
{
    static class BuildExecutorUtility
    {
        public static BuildPlayerOptions BuildPlayerOptions => GetBuildPlayerOptions(false, new BuildPlayerOptions());

        public static string DefaultBuildLocation
        {
            get
            {
                string projectPath = Path.GetDirectoryName(Application.dataPath);
                var buildLocation = CombinePaths(projectPath ?? throw new UnityException($"Project path is null: {Application.dataPath}"),
                    "Builds",
                    EditorUserBuildSettings.activeBuildTarget.ToString());

                if (!Directory.Exists(buildLocation))
                {
                    Directory.CreateDirectory(buildLocation);
                }
                return buildLocation;
            }
        }

        public static void CheckListenerPriorities(IEnumerable<IBuildTasksProvider> listenersToCheck)
        {
            Dictionary<int, List<IBuildTasksProvider>> prioritizedListeners = new Dictionary<int, List<IBuildTasksProvider>>();
            foreach (var listener in listenersToCheck)
            {
                if (prioritizedListeners.TryGetValue(listener.Priority, out var listeners) == false)
                {
                    listeners = new List<IBuildTasksProvider>();
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

        static string CombinePaths(params string[] paths)
        {
            return string.Join("/", paths);
        }

        static BuildPlayerOptions GetBuildPlayerOptions(
            bool askForLocation = false,
            BuildPlayerOptions defaultOptions = new BuildPlayerOptions())
        {
            // Fake BuildLocation path to prevent Unity exception
            var activeBuildTarget = EditorUserBuildSettings.activeBuildTarget;
            var buildLocation = EditorUserBuildSettings.GetBuildLocation(activeBuildTarget);
            // Making sure that current BuildLocation is valid, otherwise create new proper one.
            // New created location will be like: .../<ProjectRoot>/Builds/<BuildTarget>

            switch (InternalEditorUtility.BuildCanBeAppended(activeBuildTarget,buildLocation))
            {
                case CanAppendBuild.No:
                case CanAppendBuild.Unsupported:
                    var newBuildLocation = CombinePaths(DefaultBuildLocation, "build");
                    EditorUserBuildSettings.SetBuildLocation(activeBuildTarget, newBuildLocation);
                    break;
            }

            // Get static internal "GetBuildPlayerOptionsInternal" method and invoke it
            var method = typeof(BuildPlayerWindow.DefaultBuildMethods).GetMethod(
                "GetBuildPlayerOptionsInternal",
                BindingFlags.NonPublic | BindingFlags.Static);

            return (BuildPlayerOptions)method.Invoke(
                null,
                new object[] { askForLocation, defaultOptions});
        }
    }
}
