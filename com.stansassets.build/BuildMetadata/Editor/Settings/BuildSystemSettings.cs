using System.Collections.Generic;
using StansAssets.Build.Editor;
using StansAssets.Plugins;
using UnityEngine;

namespace StansAssets.Build.Meta.Editor
{
    class BuildSystemSettings : PackageScriptableSettingsSingleton<BuildSystemSettings>, ISerializationCallbackReceiver
    {
        internal static readonly string WindowTabsPath = $"{BuildSystemPackage.RootPath}/BuildMetadata/Editor/UserInterface";

        public override string PackageName => BuildSystemPackage.PackageName;
        protected override bool IsEditorOnly => true;

        [SerializeField]
        string m_SpreadsheetId;

        [SerializeField]
        string m_GitHubRepository;

        [SerializeField]
        List<ExtraField> m_ExtraFields = new List<ExtraField>();

        [SerializeField]
        bool m_AutomatedBuildNumberIncrement;

        [SerializeField]
        List<string> m_MaskList = new List<string>();

        public List<ExtraField> ExtraFields => m_ExtraFields;
        public IEnumerable<string> MaskList => m_MaskList;

        public bool AutomatedBuildNumberIncrement
        {
            get => m_AutomatedBuildNumberIncrement;
            internal set
            {
                m_AutomatedBuildNumberIncrement = value;
                Save();
            }
        }

        public string SpreadsheetId
        {
            get => m_SpreadsheetId;
            internal set
            {
                m_SpreadsheetId = value;
                Save();
            }
        }

        public string GitHubRepository
        {
            get => m_GitHubRepository;
            internal set
            {
                m_GitHubRepository = value;
                Save();
            }
        }

        public void AddMask(string mask)
        {
            m_MaskList.Add(mask);
            Save();
        }

        public void RemoveMask(string mask)
        {
            m_MaskList.Remove(mask);
            Save();
        }

        public void AddNewExtraField()
        {
            m_ExtraFields.Add(new ExtraField());
            Save();
        }

        public void ClearMaskList()
        {
            m_MaskList.Clear();
            Save();
        }

        public void OnBeforeSerialize()
        {
            //Nothing to do here. We just need OnAfterDeserialize to repopulate m_SpreadsheetsMap
            //with serialized Spreadsheets data
        }

        public void OnAfterDeserialize()
        {
            //Nothing to do here. We just need OnAfterDeserialize to repopulate m_SpreadsheetsMap
            //with serialized Spreadsheets data
        }
    }
}
