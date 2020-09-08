using StansAssets.Foundation.Editor;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace StansAssets.Build.Editor
{
    /// <summary>
    /// Build System Package Static info.
    /// </summary>
    public static class BuildSystemPackage
    {
        /// <summary>
        /// The package name
        /// </summary>
        public const string DisplayName = "Build System";

        /// <summary>
        /// Foundation package root path.
        /// </summary>
        public static readonly string RootPath = PackageManagerUtility.GetPackageRootPath(BuildSystemSettings.Instance.PackageName);

        /// <summary>
        /// Build System package info.
        /// </summary>
        public static readonly PackageInfo Info = PackageManagerUtility.GetPackageInfo(BuildSystemSettings.Instance.PackageName);
        
        internal static readonly string WindowTabsPath = $"{RootPath}/Editor/Window/Tabs";
    }
}
