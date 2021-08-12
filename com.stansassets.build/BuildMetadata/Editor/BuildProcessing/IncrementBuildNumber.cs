using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
#if GOOGLE_DOC_CONNECTOR_PRO_ENABLED
using StansAssets.Foundation;
using StansAssets.GoogleDoc;
using StansAssets.GoogleDoc.Editor;
using Google;
using GoogleSheet = Google.Apis.Sheets.v4.Data;
#endif
using UnityEditor;
using UnityEngine;

namespace StansAssets.Build.Meta.Editor
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
            Debug.LogWarning($"{nameof(IncrementBuildNumber)} Build number is set to " + buildMetadata.BuildNumber);
            PlayerSettings.Android.bundleVersionCode = buildMetadata.BuildNumber;
            PlayerSettings.iOS.buildNumber = buildMetadata.BuildNumber.ToString();

            var commitValue = buildMetadata.CommitShortHash;
            if (!string.IsNullOrEmpty(BuildSystemSettings.Instance.GitHubRepository))
            {
                commitValue = $"=HYPERLINK(\"https://github.com/{BuildSystemSettings.Instance.GitHubRepository}/commit/{buildMetadata.CommitHash}\";\"{buildMetadata.CommitShortHash}\")";
            }

            if (UnityCloudBuildHooks.IsRunningOnUnityCloud)
            {
                var manifest = (TextAsset)Resources.Load("UnityCloudBuildManifest.json");
                if (manifest != null)
                {
                    var manifestDict = Json.Deserialize(manifest.text) as Dictionary<string, object>;
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
            var sheetID = spreadsheet.Sheets.First(s => s.Name == sheetName).Id;

            try
            {
                FormatSheet(sheetID, versionsSheet.Rows.Count());
            }
            catch (Exception exception)
            {
                var message = (exception is GoogleApiException) ? (exception as GoogleApiException).Error.Message : exception.Message;
                spreadsheet.SetError($"Error: {message}");
                spreadsheet.SetMachineName(SystemInfo.deviceName);
                spreadsheet.SyncDateTime = DateTime.Now;

                spreadsheet.ChangeStatus(Spreadsheet.SyncState.SyncedWithError);
            }

            if (spreadsheet.SyncErrorMassage != null)
            {
                Debug.LogError(spreadsheet.SyncErrorMassage);
            }
        }

        static void FormatSheet(int sheetID, int rowNumber)
        {
            var batchUpdate = new GoogleSheet.BatchUpdateSpreadsheetRequest { Requests = new List<GoogleSheet.Request>() };

            var titlesFormatRequest = new GoogleSheet.Request {
                RepeatCell = new GoogleSheet.RepeatCellRequest {
                    Range = new GoogleSheet.GridRange {
                        SheetId = sheetID,
                        StartColumnIndex = 0, EndColumnIndex = 8,
                        StartRowIndex = 0, EndRowIndex = 1
                    },
                    Cell = new GoogleSheet.CellData {
                        UserEnteredFormat = new GoogleSheet.CellFormat {
                            TextFormat = new GoogleSheet.TextFormat { Bold = true },
                            HorizontalAlignment = "CENTER"
                        },
                    },
                    Fields = "userEnteredFormat(textFormat, horizontalAlignment)"
                }
            };

            var commitColumnSizeRequest = new GoogleSheet.Request {
                UpdateDimensionProperties = new GoogleSheet.UpdateDimensionPropertiesRequest {
                    Range = new GoogleSheet.DimensionRange {
                        SheetId = sheetID,
                        Dimension = "COLUMNS",
                        StartIndex = 4,
                        EndIndex = 5
                    },
                    Properties = new GoogleSheet.DimensionProperties {
                        PixelSize = 160
                    },
                    Fields = "pixelSize"
                }
            };

            var dateColumnSizeRequest = new GoogleSheet.Request {
                UpdateDimensionProperties = new GoogleSheet.UpdateDimensionPropertiesRequest {
                    Range = new GoogleSheet.DimensionRange {
                        SheetId = sheetID,
                        Dimension = "COLUMNS",
                        StartIndex = 6,
                        EndIndex = 8
                    },
                    Properties = new GoogleSheet.DimensionProperties {
                        PixelSize = 180
                    },
                    Fields = "pixelSize"
                }
            };

            var datesAndCommitAligmentRequest = new GoogleSheet.Request {
                RepeatCell = new GoogleSheet.RepeatCellRequest {
                    Range = new GoogleSheet.GridRange {
                        SheetId = sheetID,
                        StartColumnIndex = 5, EndColumnIndex = 8,
                        StartRowIndex = rowNumber, EndRowIndex = rowNumber + 1
                    },
                    Cell = new GoogleSheet.CellData {
                        UserEnteredFormat = new GoogleSheet.CellFormat {
                            HorizontalAlignment = "RIGHT"
                        },
                    },
                    Fields = "userEnteredFormat(horizontalAlignment)"
                }
            };

            var dateFormatRequest = new GoogleSheet.Request {
                RepeatCell = new GoogleSheet.RepeatCellRequest {
                    Range = new GoogleSheet.GridRange {
                        SheetId = sheetID,
                        StartColumnIndex = 6, EndColumnIndex = 8,
                        StartRowIndex = rowNumber, EndRowIndex = rowNumber + 1
                    },
                    Cell = new GoogleSheet.CellData {
                        UserEnteredFormat = new GoogleSheet.CellFormat {
                            NumberFormat = new GoogleSheet.NumberFormat {
                                Type = "DATE",
                                Pattern = "ddd, dd mmm yyyy (hh:mm)"
                            }
                        }
                    },
                    Fields = "userEnteredFormat.numberFormat"
                }
            };

            batchUpdate.Requests.Add(titlesFormatRequest);
            batchUpdate.Requests.Add(commitColumnSizeRequest);
            batchUpdate.Requests.Add(dateColumnSizeRequest);
            batchUpdate.Requests.Add(datesAndCommitAligmentRequest);
            batchUpdate.Requests.Add(dateFormatRequest);

            var request = SpreadsheetSaverToGoogle.Service.Spreadsheets.BatchUpdate(batchUpdate, BuildSystemSettings.Instance.SpreadsheetId);
            request.Execute();
        }
#endif
    }
}
