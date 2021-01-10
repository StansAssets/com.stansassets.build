using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
#if GOOGLE_DOC_CONNECTOR_PRO_ENABLED
using StansAssets.Foundation;
using StansAssets.GoogleDoc;
#endif
using UnityEditor;
using UnityEngine;

namespace StansAssets.Build.Editor
{
    static class IncrementBuildNumber
    {
        static readonly List<object> s_Headers = new List<object> { "Build Number", "Version", "Machine Name", "Branch", "Commit Message", "Commit", "Build Time", "Commit Time" };

        static int s_LastBuildNumberAndroid;
        static string s_LastBuildNumberIOS;

        internal static void Increment(BuildMetadata buildMetadata, BuildTarget buildTarget)
        {
#if GOOGLE_DOC_CONNECTOR_PRO_ENABLED
            var firstOrDefault = BuildSystemSettings.Instance.MaskList.FirstOrDefault(m => Regex.IsMatch(buildMetadata.BranchName, m));

            s_LastBuildNumberAndroid = PlayerSettings.Android.bundleVersionCode;
            s_LastBuildNumberIOS = PlayerSettings.iOS.buildNumber;

            if (firstOrDefault != null || !BuildSystemSettings.Instance.MaskList.Any())
            {
                try
                {
                    SaveBuildMetadata(buildMetadata, buildTarget);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            else
            {
                Debug.Log($"Build Increment skipped, no matched branch mask found for the {buildMetadata.BranchName} branch. ");
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
        static void SaveBuildMetadata(BuildMetadata buildMetadata, BuildTarget buildTarget)
        {
            foreach (var extraField in BuildSystemSettings.Instance.ExtraFields)
            {
                s_Headers.Add(extraField.Name);
            }

            if (string.IsNullOrEmpty(BuildSystemSettings.Instance.SpreadsheetId))
            {
                Debug.LogError("Versions Spreadsheet Id is empty");
                return;
            }

            var spreadsheet = new Spreadsheet(BuildSystemSettings.Instance.SpreadsheetId);
            spreadsheet.Load();
            if (spreadsheet.SyncErrorMassage != null)
            {
                Debug.LogError(spreadsheet.SyncErrorMassage);
                throw new Exception(spreadsheet.SyncErrorMassage);
            }

            var sheetName = $"{buildMetadata.Version} - {buildTarget}";
            var versionsSheet = spreadsheet.Sheets.FirstOrDefault(sh => sheetName.Equals(sh.Name));
            var rangeAppend = $"{sheetName}!A:K";
            var buildNumber = 0;
            if (versionsSheet == null)
            {
                spreadsheet.CreateGoogleSheet(sheetName);
                spreadsheet.AppendGoogleCell(rangeAppend, s_Headers);
                if (spreadsheet.SyncErrorMassage != null)
                {
                    Debug.LogError(spreadsheet.SyncErrorMassage);
                    throw new Exception(spreadsheet.SyncErrorMassage);
                }
            }
            else
            {
                var cell = versionsSheet.GetCell(versionsSheet.Rows.Count() - 1, 0);
                buildNumber = cell != null
                    ? versionsSheet.GetCell(versionsSheet.Rows.Count() - 1, 0).GetValue<int>()
                    : 0;
            }

            buildMetadata.BuildNumber = buildNumber + 1;
            Debug.LogWarning("Setting build number to " + buildMetadata.BuildNumber);
            PlayerSettings.Android.bundleVersionCode = buildMetadata.BuildNumber;
            PlayerSettings.iOS.buildNumber = buildMetadata.BuildNumber.ToString();


            var commitValue = buildMetadata.CommitShortHash;
            if (!string.IsNullOrEmpty(BuildSystemSettings.Instance.GitHubRepository))
            {
                commitValue = $"=HYPERLINK(\"https://github.com/{BuildSystemSettings.Instance.GitHubRepository}/commit/{buildMetadata.CommitHash}\",\"{buildMetadata.CommitShortHash}\")";
            }

            if (UnityCloudBuildHooks.IsRunningOnUnityCloud)
            {
                var manifest = (TextAsset) Resources.Load("UnityCloudBuildManifest.json");
                if (manifest != null)
                {
                    var manifestDict = Json.Deserialize(manifest.text) as Dictionary<string,object>;
                    foreach (var kvp in manifestDict)
                    {
                        // Be sure to check for null values!
                        var value = kvp.Value != null ? kvp.Value.ToString() : string.Empty;

                        foreach (var extraField in BuildSystemSettings.Instance.ExtraFields)
                        {
                            extraField.Value = extraField.Value.Replace("{" + kvp.Key + "}", value);
                        }
                    }
                }
                else
                {
                    Debug.LogWarning("[IncrementBuildNumber] UnityCloudBuildManifest.json not found");
                }
            }

            var appendList = new List<object>
            {
                buildMetadata.BuildNumber,
                buildMetadata.Version,
                buildMetadata.MachineName,
                buildMetadata.BranchName,
                buildMetadata.CommitMessage,
                commitValue,
                buildMetadata.BuildTime.ToString("G"),
                buildMetadata.CommitTime.ToString("G")
            };

            foreach (var extraField in BuildSystemSettings.Instance.ExtraFields)
            {
                appendList.Add(extraField.Value);
            }

            spreadsheet.AppendGoogleCell(rangeAppend, appendList);
            if (spreadsheet.SyncErrorMassage != null)
            {
                Debug.LogError(spreadsheet.SyncErrorMassage);
            }
        }
#endif
    }
}
