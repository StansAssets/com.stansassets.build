using StansAssets.Foundation.Extensions;
using StansAssets.Git;
using StansAssets.Plugins.Editor;
using UnityEditor;
using UnityEngine;

namespace StansAssets.Build.Editor
{
    static class EditorMenu
    {
        [MenuItem(PackagesConfigEditor.RootMenu + "/" + BuildSystemPackage.DisplayName +"/Do Something")]
        static void DoSomething()
        {
            Debug.Log("Doing Something..2.");
            var git = Gits.GetFromCurrentDirectory();
            Debug.Log(Build.Metadata.BranchName);
            Debug.Log(Build.Metadata.CommitHash);
            
            var dat = Build.Metadata.CommitTime;
            Debug.Log(dat.ToString("dd MMMM HH:mm"));

        }
        
        [MenuItem(PackagesConfigEditor.RootMenu + "/" + BuildSystemPackage.DisplayName +"/CreateBuildMetadata")]
        static void Create()
        {
          //  BuildProcessor.CreateBuildMetadata();
        }
        
        [MenuItem(PackagesConfigEditor.RootMenu + "/" + BuildSystemPackage.DisplayName +"/DeleteBuildMetadata")]
        static void Remove()
        {
           // BuildProcessor.DeleteBuildMetadata();
        }
    }
}
