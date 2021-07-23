using UnityEditor;

namespace StansAssets.Build.Meta.Editor
{
    [InitializeOnLoad]
    static class MetadataProviderInit
    {
        static MetadataProviderInit()
        {
            var provider = new EditorMetadataProvider();
            Build.RegisterBuildMetadataProvider(provider);
        }
    }
}
