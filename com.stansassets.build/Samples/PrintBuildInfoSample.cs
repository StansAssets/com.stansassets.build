using StansAssets.Build.Meta;
using UnityEngine;

public class PrintBuildInfoSample : MonoBehaviour
{
    void OnGUI()
    {
        GUILayout.Label($"Branch: {Build.Metadata.BranchName} / {Build.Metadata.CommitShortHash}");
        GUILayout.Label($"CommitTime: {Build.Metadata.CommitTime:dd MMMM HH:mm}");
        GUILayout.Label($"CommitMessage: {Build.Metadata.CommitMessage}");

        GUILayout.FlexibleSpace();
        GUILayout.Label($"Build Made: {Build.Metadata.BuildTime:dd MMMM HH:mm} on {Build.Metadata.MachineName}");

        if (!string.IsNullOrEmpty(Build.Metadata.Note))
        {
            GUILayout.FlexibleSpace();
            GUILayout.Label($"HasChangesInWorkingCopy: {Build.Metadata.HasChangesInWorkingCopy}");
        }
    }

}
