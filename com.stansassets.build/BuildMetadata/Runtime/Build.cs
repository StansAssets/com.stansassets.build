namespace StansAssets.Build.Meta
{
    /// <summary>
    /// Accesses point to to the build system API.
    /// </summary>
    public static class Build
    {
        static IBuildMetadataProvider s_MetadataProvider;

        internal static void RegisterBuildMetadataProvider(IBuildMetadataProvider provider)
        {
            s_MetadataProvider = provider;
        }

        static IBuildMetadataProvider MetadataProvider
            => s_MetadataProvider ?? (s_MetadataProvider = new DefaultBuildMetadataProvider());

        /// <summary>
        /// Current build metadata.
        /// </summary>
        public static BuildMetadata Metadata => MetadataProvider.GetBuildMetadata();
    }
}
