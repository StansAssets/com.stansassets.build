using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StansAssets.Foundation.UIElements;
using StansAssets.Plugins.Editor;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UIElements;

namespace StansAssets.Build.Editor
{
    class SettingsTab : BaseTab
    {
        const string k_MaskTextPlaceholder = "Paste mask branches to increase the build number";

        readonly TextField m_SpreadsheetIdText;
        readonly TextField m_MaskText;
        readonly VisualElement m_ListMask;
        readonly SettingsBlock m_BlockGoogleDoc;
        readonly SettingsBlock m_BlockSpreadsheetId;
        readonly SettingsBlock m_BlockMask;
        readonly Toggle m_IncrementBuildNumberToggle;

        public SettingsTab()
            : base($"{BuildSystemPackage.WindowTabsPath}/SettingsTab")
        {
            m_MaskText = this.Q<TextField>("mask-text");
            m_SpreadsheetIdText = this.Q<TextField>("spreadsheetIdText");
            m_BlockGoogleDoc = this.Q<SettingsBlock>("block-google-doc-connector-pro");
            m_BlockSpreadsheetId = this.Q<SettingsBlock>("block-spreadsheet-id");
            m_BlockMask = this.Q<SettingsBlock>("block-mask");
            m_ListMask = this.Q<VisualElement>("listMask");
            var spreadsheetIdText = this.Q<Button>("save-id");
            spreadsheetIdText.clicked += SaveId;
            var addMask = this.Q<Button>("add-mask");
            addMask.clicked += AddMask;
            var clearMask = this.Q<Button>("clear-mask");
            clearMask.clicked += ClearMaskList;
            var downloadGoogleDocConnector = this.Q<Button>("download-google-doc-connector-pro");
            downloadGoogleDocConnector.clicked += DownloadGoogleDoc;
            
            m_IncrementBuildNumberToggle = this.Q<Toggle>("incrementBuildNumber-toggle");
            m_IncrementBuildNumberToggle.RegisterValueChangedCallback(e =>
            {
                IncrementBuildNumberToggleBind(e.newValue);
                BuildSystemSettings.Instance.IncrementBuildNumberEnableSet(e.newValue);
            });
            m_IncrementBuildNumberToggle.value = BuildSystemSettings.Instance.IncrementBuildNumberEnable;
            IncrementBuildNumberToggleBind(m_IncrementBuildNumberToggle.value);
            if (!StanAssetsPackages.IsGoogleDocConnectorProInstalled)
            {
                m_BlockGoogleDoc.style.display = DisplayStyle.Flex;
                m_BlockSpreadsheetId.SetEnabled(false);
                m_BlockMask.SetEnabled(false);
                m_IncrementBuildNumberToggle.SetEnabled(false);
            }
            else
            {
                m_BlockGoogleDoc.style.display = DisplayStyle.None;
            }
        }
        
        void Bind()
        {
            m_SpreadsheetIdText.value = BuildSystemSettings.Instance.SpreadsheetId;
            m_MaskText.value = k_MaskTextPlaceholder;
            m_MaskText.tooltip = k_MaskTextPlaceholder;
            m_ListMask.Clear();
            foreach (var mask in BuildSystemSettings.Instance.MaskList)
            {
                CreateMaskElement(mask);
            }
            if (!StanAssetsPackages.IsGoogleDocConnectorProInstalled)
            {
                m_BlockGoogleDoc.style.display = DisplayStyle.Flex;
                m_BlockSpreadsheetId.SetEnabled(false);
                m_BlockMask.SetEnabled(false);
            }
            else
            {
                m_BlockSpreadsheetId.SetEnabled(true);
                m_BlockMask.SetEnabled(true);
                m_BlockGoogleDoc.style.display = DisplayStyle.None;
            }
        }

        void SaveId()
        {
            BuildSystemSettings.Instance.SetSpreadsheetId(m_SpreadsheetIdText.value);
        }

        void AddMask()
        {
            BuildSystemSettings.Instance.AddMask(m_MaskText.value);
            CreateMaskElement(m_MaskText.value);
            m_MaskText.value = k_MaskTextPlaceholder;
        }

        void CreateMaskElement(string mask)
        {
            var visualElement = new VisualElement();
            visualElement.AddToClassList("mask-element");
            var label = new Label { text = mask };
            visualElement.Add(label);
            var btnRemove = new Button { tooltip = "Remove mask" };
            btnRemove.AddToClassList("btn-remove");
            btnRemove.clicked += () => { RemoveMask(visualElement, mask); };
            visualElement.Add(btnRemove);
            m_ListMask.Add(visualElement);
        }

        void RemoveMask(VisualElement visualElement, string mask)
        {
            m_ListMask.Remove(visualElement);
            BuildSystemSettings.Instance.RemoveMask(mask);
        }

        void ClearMaskList()
        {
            m_ListMask.Clear();
            BuildSystemSettings.Instance.ClearMaskList();
        }

        void DownloadGoogleDoc()
        {
            StanAssetsPackages.AddStanAssetsPackage(StanAssetsPackages.GoogleDocConnectorProPackage, StanAssetsPackages.GoogleDocConnectorProPackageVersion);
            m_BlockGoogleDoc.style.display = DisplayStyle.None;
            m_BlockSpreadsheetId.SetEnabled(true);
            m_BlockMask.SetEnabled(true);
            m_IncrementBuildNumberToggle.SetEnabled(true);
        }

        void IncrementBuildNumberToggleBind(bool newValue)
        {
            if (newValue)
            {
                m_BlockGoogleDoc.style.display = DisplayStyle.Flex;
                m_BlockSpreadsheetId.style.display = DisplayStyle.Flex;
                m_BlockMask.style.display = DisplayStyle.Flex;
                Bind();
            }
            else
            {
                m_BlockGoogleDoc.style.display = DisplayStyle.None;
                m_BlockSpreadsheetId.style.display = DisplayStyle.None;
                m_BlockMask.style.display = DisplayStyle.None;
            }
        }
    }
}
