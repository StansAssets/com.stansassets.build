using StansAssets.Plugins;
using UnityEngine;

namespace StansAssets.Build.Editor
{
    class BuildSystemSettings : PackageScriptableSettingsSingleton<BuildSystemSettings>, ISerializationCallbackReceiver
    {
        public override string PackageName => "com.stansassets.build";

        public string SpreadsheetId => m_SpreadsheetId;
        string m_SpreadsheetId;

        public void SetSpreadsheetId(string id)
        {
            m_SpreadsheetId = id;
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
