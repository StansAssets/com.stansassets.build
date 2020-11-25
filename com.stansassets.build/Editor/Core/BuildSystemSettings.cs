using System;
using System.Collections.Generic;
using StansAssets.Plugins;
using UnityEngine;

namespace StansAssets.Build.Editor
{
    class BuildSystemSettings : PackageScriptableSettingsSingleton<BuildSystemSettings>, ISerializationCallbackReceiver
    {
        public override string PackageName => "com.stansassets.build";
        protected override bool IsEditorOnly => true;

        [SerializeField]
        string m_SpreadsheetId;

        [SerializeField]
        bool m_AutomatedBuildNumberIncrement;

        [SerializeField]
        List<string> m_MaskList = new List<string>();

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
