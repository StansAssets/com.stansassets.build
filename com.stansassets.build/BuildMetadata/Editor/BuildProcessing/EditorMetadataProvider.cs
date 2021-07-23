namespace StansAssets.Build.Meta.Editor
{
    class EditorMetadataProvider : IBuildMetadataProvider
    {
        BuildMetadata m_BuildMetadata;

        public BuildMetadata GetBuildMetadata()
        {
            if (m_BuildMetadata == null)
            {
                m_BuildMetadata = BuildProcessor.CreateBuildMetadata();
            }

            return m_BuildMetadata;
        }
    }
}
