using UnityEditor;

namespace StansAssets.Build.Pipeline
{
    class UserEditorBuildSettings : IUserEditorBuildSettings
    {
        public BuildTarget BuildTarget => EditorUserBuildSettings.activeBuildTarget;
    }
}
