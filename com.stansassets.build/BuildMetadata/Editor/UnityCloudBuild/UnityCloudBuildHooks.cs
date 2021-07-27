using UnityEngine;

namespace StansAssets.Build.Meta.Editor
{

    /// <summary>
    /// Used, just to demo of how to work with cloud builds APi
    /// </summary>
    public static class UnityCloudBuildHooks
    {
        internal static string BuildNumber { get; private set; } = "undefined";
        internal static string CloudBuildTargetName { get; private set; } = "undefined";

        public static void PreExport(UnityEngine.CloudBuild.BuildManifestObject manifest)
        {
            Debug.LogWarning("BuildProcessor PreExport json: " + manifest.ToJson());
            Debug.LogWarning($"BuildProcessor cloudBuildTargetName: { manifest.GetValue<string>("cloudBuildTargetName")}");
            Debug.LogWarning($"BuildProcessor buildNumber: { manifest.GetValue<string>("buildNumber")}");
            Debug.LogWarning($"BuildProcessor projectId: { manifest.GetValue<string>("projectId")}");

            BuildNumber = manifest.GetValue<string>("buildNumber");
            CloudBuildTargetName = manifest.GetValue<string>("cloudBuildTargetName");
        }

        public static void PostExport(string exportPath)
        {
            /*
            Debug.LogWarning($"[LOG] BuildProcessor PostExport  {exportPath}");


            var orgid = "7971701117095";
            //var projectid = "7971701117095/0f498ec7-4466-462c-81da-e18db4f01315";


            var projectid = "dungen-master";
            var buildtargetid = CloudBuildTargetName;
            var number = BuildNumber;
            //var requestUrl = $"https://build-api.cloud.unity3d.com/api/v1/orgs/{orgId}/projects/{projectId}/buildtargets/{targetId}/auditlog/";


            //var requestUrl = "https://build-api.cloud.unity3d.com/api/v1/api.json";

            var projects = $"/orgs/{orgid}/projects";
            var billingPlan = $"orgs/{orgid}/billingplan";
            var share = $"/orgs/{orgid}/projects/{projectid}/buildtargets/{buildtargetid}/builds/{number}/share";

            var buildtargets = $"/orgs/{orgid}/projects/{projectid}/buildtargets";


            var requestUrl = $"https://build-api.cloud.unity3d.com/api/v1{share}";


            Debug.Log($"requestUrl: {requestUrl}");
            var apiKey = "29a54d35d2b7e5e85b7c965639b8a1b5";
            var unityWebRequest = new UnityWebRequest(requestUrl)
            {
                method = "POST",
                timeout = 60,
                downloadHandler = new DownloadHandlerBuffer()
            };

            unityWebRequest.SetRequestHeader("Content-type", @"application/json");
            unityWebRequest.SetRequestHeader("Authorization", $"Basic {apiKey}");

            unityWebRequest.SendWebRequest();


            while (!unityWebRequest.isDone) {
                //do something, or nothing while blocking
            }

            Debug.LogWarning($"[LOG]  unityWebRequest.error: {unityWebRequest.error}");
            Debug.LogWarning($"[LOG]  unityWebRequest.downloadHandler.text: {unityWebRequest.downloadHandler.text}");
            */

            /*
             * Share link sample
             * $shareLink = "https://build.cloud.unity3d.com/share/";
             * $shareLink = $shareLink.$val["shareid"]."/webgl";
             * file_put_contents('shareUrl.txt', $shareLink);
             */
        }

#if UNITY_CLOUD_BUILD
        public static bool IsRunningOnUnityCloud => true;
#else
        public static bool IsRunningOnUnityCloud => false;
#endif
    }
}
