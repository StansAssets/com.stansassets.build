using System.Collections.Generic;
using StansAssets.Plugins;
using UnityEditor;
using UnityEngine;

namespace StansAssets.Build.Editor
{
    class BuildSystemSettings : PackageScriptableSettingsSingleton<BuildSystemSettings>, ISerializationCallbackReceiver
    {
        public override string PackageName => "com.stansassets.build";

        public string SpreadsheetId => m_SpreadsheetId;
        [SerializeField]
        string m_SpreadsheetId;

        public List<string> MaskList => m_MaskList;
        [SerializeField]
        List<string> m_MaskList = new List<string>();

        public void AddMask(string mask)
        {
            m_MaskList.Add(mask);
            EditorUtility.SetDirty(this);
        }

        public void RemoveMask(string mask)
        {
            m_MaskList.Remove(mask);
            EditorUtility.SetDirty(this);
        }

        public void ClearMaskList()
        {
            m_MaskList.Clear();
            EditorUtility.SetDirty(this);
        }

        public void SetSpreadsheetId(string id)
        {
            m_SpreadsheetId = id;
            EditorUtility.SetDirty(this);
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
