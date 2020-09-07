using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using StansAssets.Git;
using StansAssets.GoogleDoc;
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

        static readonly List<object> s_Headers = new List<object> { "Build Number", "Version", "Has Changes In Working Copy",  "BranchName", "Commit Hash", "Commit Short Hash", "Commit Message", "Note", "Machine Name", "Build Time", "Commit Time" };

        public int callbackOrder => k_CallbackOrder;

        public void OnPreprocessBuild(BuildReport report)
        {
  ;
            var buildMetadata = CreateBuildMetadata();
            try
            {
                IncrementBuildNumber(buildMetadata);
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
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
            DeleteBuildMetadata();
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

        static void IncrementBuildNumber(BuildMetadata buildMetadata)
        {
            var spreadsheet = new Spreadsheet(BuildSystemSettings.Instance.SpreadsheetId);
            spreadsheet.Load();
            if (spreadsheet.SyncErrorMassage != string.Empty)
            {
                Debug.LogError(spreadsheet.SyncErrorMassage);
                throw new Exception(spreadsheet.SyncErrorMassage);
            }
            var sheetList = spreadsheet.Sheets.Where(sh => sh.Name == buildMetadata.Version);
            var sheetArr = sheetList as Sheet[] ?? sheetList.ToArray();
            var rangeAppend = $"{buildMetadata.Version}!A:K";
            var buildNumber = 0;
            if (!sheetArr.Any())
            {
                spreadsheet.CreateGoogleSheet(buildMetadata.Version);
                spreadsheet.AppendGoogleCell(rangeAppend, s_Headers);
                if (spreadsheet.SyncErrorMassage != string.Empty)
                {
                    Debug.LogError(spreadsheet.SyncErrorMassage);
                    throw new Exception(spreadsheet.SyncErrorMassage);
                }
            }
            else
            {
                buildNumber = sheetArr[0].GetCell(sheetArr[0].Rows.Count(), 0).GetValue<int>();
            }
            buildMetadata.SetBuildTime(buildNumber + 1);
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
                buildMetadata.BuildTime,
                buildMetadata.CommitTime
            });
            if (spreadsheet.SyncErrorMassage != string.Empty)
            {
                Debug.LogError(spreadsheet.SyncErrorMassage);
            }
        }
    }
}
