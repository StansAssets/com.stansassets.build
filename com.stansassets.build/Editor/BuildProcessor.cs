﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using StansAssets.Git;
#if GOOGLE_DOC_CONNECTOR_PRO_ENABLED
using StansAssets.GoogleDoc;
#endif
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Callbacks;
using UnityEngine;

namespace StansAssets.Build.Editor
{
    class BuildProcessor : IPreprocessBuildWithReport
    {
        const int k_CallbackOrder = 1;
        static readonly string k_BuildMetadataPath = $"Assets/Resources/{nameof(BuildMetadata)}.asset";
        static string BuildMetadataDirectoryPath => Path.GetDirectoryName(k_BuildMetadataPath);

        static readonly List<object> s_Headers = new List<object> { "Build Number", "Version", "Has Changes In Working Copy", "BranchName", "Commit Hash", "Commit Short Hash", "Commit Message", "Note", "Machine Name", "Build Time", "Commit Time" };

        static int s_LastBuildNumberAndroid;
        static string s_LastBuildNumberIOS;

        public int callbackOrder => k_CallbackOrder;

        public void OnPreprocessBuild(BuildReport report)
        {
            var buildMetadata = CreateBuildMetadata();
            var firstOrDefault = BuildSystemSettings.Instance.MaskList.FirstOrDefault(m => Regex.IsMatch(buildMetadata.BranchName, m));
            
            s_LastBuildNumberAndroid = PlayerSettings.Android.bundleVersionCode;
            s_LastBuildNumberIOS = PlayerSettings.iOS.buildNumber;
#if GOOGLE_DOC_CONNECTOR_PRO_ENABLED
            if (firstOrDefault != null)
            {
                try
                {
                    IncrementBuildNumber(buildMetadata);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
#endif

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
            DeleteBuildMetadata();
            PlayerSettings.Android.bundleVersionCode = s_LastBuildNumberAndroid;
            PlayerSettings.iOS.buildNumber = s_LastBuildNumberIOS;
        }

        public static BuildMetadata CreateBuildMetadata()
        {
            var meta = ScriptableObject.CreateInstance<BuildMetadata>();
            var git = Gits.GetFromCurrentDirectory();
            meta.HasChangesInWorkingCopy = git.WorkingCopy.HasChanges;
            meta.BranchName = git.Branch.Name;
            meta.CommitHash = git.Commit.Hash;
            meta.CommitShortHash = git.Commit.ShortHash;
            meta.CommitMessage = git.Commit.Message;
            meta.SetCommitTime(git.Commit.UnixTimestamp);
            meta.SetBuildTime(DateTime.Now.Ticks);

            meta.MachineName = SystemInfo.deviceName;
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
#if GOOGLE_DOC_CONNECTOR_PRO_ENABLED
        static void IncrementBuildNumber(BuildMetadata buildMetadata)
        {
            var spreadsheet = new Spreadsheet(BuildSystemSettings.Instance.SpreadsheetId);
            spreadsheet.Load();
            if (spreadsheet.SyncErrorMassage != null)
            {
                Debug.LogError(spreadsheet.SyncErrorMassage);
                throw new Exception(spreadsheet.SyncErrorMassage);
            }

            var sheetList = spreadsheet.Sheets.FirstOrDefault(sh => sh.Name == buildMetadata.Version);
            var rangeAppend = $"{buildMetadata.Version}!A:K";
            var buildNumber = 0;
            if (sheetList == null)
            {
                spreadsheet.CreateGoogleSheet(buildMetadata.Version);
                spreadsheet.AppendGoogleCell(rangeAppend, s_Headers);
                if (spreadsheet.SyncErrorMassage != null)
                {
                    Debug.LogError(spreadsheet.SyncErrorMassage);
                    throw new Exception(spreadsheet.SyncErrorMassage);
                }
            }
            else
            {
                buildNumber = sheetList.GetCell(sheetList.Rows.Count() - 1, 0).GetValue<int>();
            }

            buildMetadata.BuildNumber = buildNumber + 1;
            Debug.LogWarning("Setting build number to " + buildMetadata.BuildNumber);
            PlayerSettings.Android.bundleVersionCode = buildMetadata.BuildNumber;
            PlayerSettings.iOS.buildNumber = buildMetadata.BuildNumber.ToString();
            spreadsheet.AppendGoogleCell(rangeAppend, new List<object>()
            {
                buildMetadata.BuildNumber,
                buildMetadata.Version,
                buildMetadata.HasChangesInWorkingCopy,
                buildMetadata.BranchName,
                buildMetadata.CommitHash,
                buildMetadata.CommitShortHash,
                buildMetadata.CommitMessage,
                buildMetadata.Note,
                buildMetadata.MachineName,
                buildMetadata.BuildTime.ToString("G"),
                buildMetadata.CommitTime.ToString("G")
            });
            if (spreadsheet.SyncErrorMassage != null)
            {
                Debug.LogError(spreadsheet.SyncErrorMassage);
            }
        }
#endif
    }
}
