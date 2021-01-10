using System;
using System.IO;
using StansAssets.Git;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Callbacks;
using UnityEditorInternal;
using UnityEngine;

namespace StansAssets.Build.Editor
{
    class BuildProcessor : IPreprocessBuildWithReport
    {
        const int k_CallbackOrder = 1;
        static readonly string k_BuildMetadataPath = $"Assets/Resources/{nameof(BuildMetadata)}.asset";
        static string BuildMetadataDirectoryPath => Path.GetDirectoryName(k_BuildMetadataPath);

        static bool s_IncrementBuildNumberEnable;
        public int callbackOrder => k_CallbackOrder;

        public void OnPreprocessBuild(BuildReport report)
        {
            var buildMetadata = CreateBuildMetadata();
            s_IncrementBuildNumberEnable = BuildSystemSettings.Instance.AutomatedBuildNumberIncrement && StanAssetsPackages.IsGoogleDocConnectorProInstalled;
            if (s_IncrementBuildNumberEnable)
            {
                IncrementBuildNumber.Increment(buildMetadata, report.summary.platform);
            }

            switch (report.summary.platform)
            {
                case BuildTarget.Android:
                    buildMetadata.BuildNumber = PlayerSettings.Android.bundleVersionCode;
                    break;

                case BuildTarget.iOS:
                    buildMetadata.BuildNumber = !string.IsNullOrEmpty(PlayerSettings.iOS.buildNumber)
                        ? Convert.ToInt32(PlayerSettings.iOS.buildNumber)
                        : 0;
                    break;
            }

            SaveBuildMetadata(buildMetadata);
        }

        [PostProcessBuild(k_CallbackOrder)]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
        {
            Debug.LogWarning("[LOG] OnPostprocessBuild called");
            DeleteBuildMetadata();
            if (s_IncrementBuildNumberEnable)
            {
                IncrementBuildNumber.Decrement();
            }
        }

        public static BuildMetadata CreateBuildMetadata()
        {
            var meta = ScriptableObject.CreateInstance<BuildMetadata>();
            var git = Gits.GetFromCurrentDirectory();
            meta.HasChangesInWorkingCopy = git.WorkingCopy.HasChanges;
            meta.BranchName = git.Branch.Name;
            meta.CommitHash = git.Commit.Hash;
            meta.CommitShortHash = git.Commit.ShortHash;
            meta.GitCommitHubHash = git.Commit.GitHubHash;
            meta.CommitMessage = git.Commit.Message;
            meta.SetCommitTime(git.Commit.UnixTimestamp);
            meta.SetBuildTime(DateTime.Now.Ticks);

            meta.MachineName = SystemInfo.deviceName;
            meta.FullUnityVersion = InternalEditorUtility.GetFullUnityVersion();
            return meta;
        }

        static void SaveBuildMetadata(BuildMetadata buildMetadata)
        {
            if (!Directory.Exists(BuildMetadataDirectoryPath))
                Directory.CreateDirectory(BuildMetadataDirectoryPath);

            AssetDatabase.CreateAsset(buildMetadata, k_BuildMetadataPath);
        }

        static void DeleteBuildMetadata()
        {
            AssetDatabase.DeleteAsset(k_BuildMetadataPath);
            var directoryInfo = new DirectoryInfo(BuildMetadataDirectoryPath);
            if (directoryInfo.Exists)
            {
                if (directoryInfo.GetFileSystemInfos().Length == 0)
                {
                    AssetDatabase.DeleteAsset(BuildMetadataDirectoryPath);
                }
            }
        }

    }
}
