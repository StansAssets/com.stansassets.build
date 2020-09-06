using UnityEngine;

namespace StansAssets.Build
{
    public class DefaultBuildMetadataProvider : IBuildMetadataProvider
    {
        BuildMetadata m_BuildMetadata;

        public BuildMetadata GetBuildMetadata()
        {
            if (m_BuildMetadata == null)
            {
                m_BuildMetadata = Resources.Load(nameof(BuildMetadata)) as BuildMetadata;
                if (m_BuildMetadata == null)
                {
                    m_BuildMetadata = ScriptableObject.CreateInstance<BuildMetadata>();
                    Debug.LogWarning($"{typeof(Build).FullName}: No Build metadata found.");
                }
            }

            return m_BuildMetadata;
        }
    }
}
