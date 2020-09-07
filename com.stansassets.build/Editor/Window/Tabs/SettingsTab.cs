using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StansAssets.Plugins.Editor;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UIElements;

namespace StansAssets.Build.Editor
{
    class SettingsTab : BaseTab
    {
        readonly TextField m_SpreadsheetIdText;

        public SettingsTab()
            : base($"{BuildSystemPackage.WindowTabsPath}/SettingsTab")
        {
            m_SpreadsheetIdText = this.Q<TextField>("spreadsheetIdText");
            m_SpreadsheetIdText.value = BuildSystemSettings.Instance.SpreadsheetId;
            var spreadsheetIdText = this.Q<Button>("save-id");
            spreadsheetIdText.clicked += SaveId;
        }
        
        public void SaveId()
        {
            BuildSystemSettings.Instance.SetSpreadsheetId(m_SpreadsheetIdText.value);
        }
    }
}
