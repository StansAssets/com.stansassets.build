using JetBrains.Annotations;
using StansAssets.Build.Meta.Editor;
using StansAssets.Plugins.Editor;
using UnityEditor;
using UnityEngine.UIElements;

namespace StansAssets.Build.Editor
{
    [UsedImplicitly]
    class MetaTab : BaseTab, IBuildSystemWindowTab
    {
        public string Title => "Build Meta";
        public VisualElement Tab => this;

        readonly VisualElement m_MetaContainer;

        public MetaTab()
            : base($"{BuildSystemSettings.WindowTabsPath}/MetaTab/MetaTab")
        {
            m_MetaContainer = this.Q("build-meta");
            var metadata = Meta.Build.Metadata;
            AddMetaRow("App Version", metadata.Version);
            AddMetaRow("Unity Version", metadata.FullUnityVersion);
            AddMetaRow("Machine Name", metadata.MachineName);
            AddMetaRow("Branch", metadata.BranchName);
            AddMetaRow("Commit", metadata.GitCommitHubHash);
            AddMetaRow("Commit Time", metadata.CommitTime.ToString("d"));
            AddMetaRow("Commit Msg", metadata.CommitMessage);
        }

        void AddMetaRow(string title, string value)
        {
            var metaRowTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"{BuildSystemSettings.WindowTabsPath}/MetaTab/BuildMetaRowTemplate.uxml");
            var item = metaRowTemplate.CloneTree();
            m_MetaContainer.Add(item);

            var titleLabel = item.Q<Label>("row-label");
            var valueLabel = item.Q<Label>("row-value");

            titleLabel.text = $"{title}:";
            valueLabel.text = value;
        }
    }
}
