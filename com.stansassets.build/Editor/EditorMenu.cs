using StansAssets.Git;
using StansAssets.Plugins.Editor;
using UnityEditor;
using UnityEngine;

namespace StansAssets.Build.Editor
{
    static class EditorMenu
    {
        [MenuItem(PluginsDevKitPackage.RootMenu + "/" + BuildSystemPackage.DisplayName +"/Do Something")]
        static void DoSomething()
        {
            Debug.Log("Doing Something..2.");
            var git = Gits.GetFromCurrentDirectory();
            Debug.Log(Build.Metadata.BranchName);
            Debug.Log(Build.Metadata.CommitMessage);
            Debug.Log(Build.Metadata.CommitHash);
            Debug.Log(Build.Metadata.GitCommitHubHash);
            Debug.Log(Build.Metadata.CommitShortHash);

            var dat = Build.Metadata.CommitTime;
            Debug.Log(dat.ToString("dd MMMM HH:mm"));

        }

        [MenuItem(PluginsDevKitPackage.RootMenu + "/" + BuildSystemPackage.DisplayName +"/Do TestBuildIncrement")]
        static void TestBuildIncrement()
        {
            var buildMetadata = Build.Metadata;
            var incrementBuildNumberEnable = BuildSystemSettings.Instance.AutomatedBuildNumberIncrement && StanAssetsPackages.IsGoogleDocConnectorProInstalled;
            if (incrementBuildNumberEnable)
            {
                IncrementBuildNumber.Increment(buildMetadata, EditorUserBuildSettings.activeBuildTarget);
            }
        }




        [MenuItem(PluginsDevKitPackage.RootMenu + "/" + BuildSystemPackage.DisplayName +"/CreateBuildMetadata")]
        static void Create()
        {
          //  BuildProcessor.CreateBuildMetadata();
        }

        [MenuItem(PluginsDevKitPackage.RootMenu + "/" + BuildSystemPackage.DisplayName +"/DeleteBuildMetadata")]
        static void Remove()
        {
           // BuildProcessor.DeleteBuildMetadata();
        }
    }
}
