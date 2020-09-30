using System;
using System.Collections.Generic;
using StansAssets.Plugins;
using UnityEngine;

namespace StansAssets.Build.Editor
{
    class BuildSystemSettings : PackageScriptableSettingsSingleton<BuildSystemSettings>, ISerializationCallbackReceiver
    {
        public override string PackageName => "com.stansassets.build";
        protected override bool IsEditorOnly => false;

        public SettingsTab SettingsTab { set; internal get; }

        public string SpreadsheetId => m_SpreadsheetId;
        [SerializeField]
        string m_SpreadsheetId;

        public IEnumerable<string> MaskList => m_MaskList;
        [SerializeField]
        List<string> m_MaskList = new List<string>();

        public bool IncrementBuildNumberEnable => m_IncrementBuildNumberEnable;
        [SerializeField]
        bool m_IncrementBuildNumberEnable;

        public void SetSpreadsheetId(string id)
        {
            m_SpreadsheetId = id;
            Save();
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

        public void IncrementBuildNumberEnableSet(bool value)
        {
            m_IncrementBuildNumberEnable = value;
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
