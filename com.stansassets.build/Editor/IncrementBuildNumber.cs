using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
#if GOOGLE_DOC_CONNECTOR_PRO_ENABLED
using StansAssets.GoogleDoc;
#endif
using UnityEditor;
using UnityEngine;

namespace StansAssets.Build.Editor
{
    static class IncrementBuildNumber
    {
        static readonly List<object> s_Headers = new List<object> { "Build Number", "Version", "Has Changes In Working Copy", "BranchName", "Commit Hash", "Commit Short Hash", "Commit Message", "Note", "Machine Name", "Build Time", "Commit Time" };

        static int s_LastBuildNumberAndroid;
        static string s_LastBuildNumberIOS;

        internal static void Increment(BuildMetadata buildMetadata)
        {
#if GOOGLE_DOC_CONNECTOR_PRO_ENABLED
            var firstOrDefault = BuildSystemSettings.Instance.MaskList.FirstOrDefault(m => Regex.IsMatch(buildMetadata.BranchName, m));

            s_LastBuildNumberAndroid = PlayerSettings.Android.bundleVersionCode;
            s_LastBuildNumberIOS = PlayerSettings.iOS.buildNumber;

            if (firstOrDefault != null)
            {
                try
                {
                    SaveBuildMetadata(buildMetadata);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
#endif
        }

        internal static void Decrement()
        {
#if GOOGLE_DOC_CONNECTOR_PRO_ENABLED
            PlayerSettings.Android.bundleVersionCode = s_LastBuildNumberAndroid;
            PlayerSettings.iOS.buildNumber = s_LastBuildNumberIOS;
#endif
        }

#if GOOGLE_DOC_CONNECTOR_PRO_ENABLED
        static void SaveBuildMetadata(BuildMetadata buildMetadata)
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
