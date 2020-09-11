using System.Collections.Generic;
using StansAssets.Foundation.Editor;

namespace StansAssets.Build.Editor
{
    public static class StanAssetsPackages
    {
        public static readonly string GoogleDocConnectorProPackage = "com.stansassets.google-doc-connector-pro";
        public static readonly string GoogleDocConnectorProPackageVersion = "https://github.com/StansAssets/com.stansassets.google-doc-connector-pro.git";
        static ScopeRegistry StanAssetsScopeRegistry =>
            new ScopeRegistry("Stans Assets extra package",
                "https://stansassets.com",
                new HashSet<string>
                {
                    "com.stansassets"
                });

        public static bool IsGoogleDocConnectorProInstalled
        {
            get
            {
#if GOOGLE_DOC_CONNECTOR_PRO_ENABLED
                return true;
#else
                return false;
#endif
            }
        }

        public static void AddStanAssetsPackage(string packageName, string packageVersion)
        {
            AddPackage(StanAssetsScopeRegistry, packageName, packageVersion);
        }

        static void AddPackage(ScopeRegistry scopeRegistry, string packageName, string packageVersion)
        {
            var manifest = new Manifest();
            manifest.Fetch();
            
            var manifestUpdated = false;
            if (!manifest.TryGetScopeRegistry(scopeRegistry.Url, out  _))
            {
                manifest.SetScopeRegistry(scopeRegistry.Url, scopeRegistry);
                manifestUpdated = true;
            }
            
            if (!manifest.IsDependencyExists(packageName))
            {
                manifest.SetDependency(packageName, packageVersion);
                manifestUpdated = true;
            }

            if (manifestUpdated)
                manifest.ApplyChanges();
        }
    }
}
