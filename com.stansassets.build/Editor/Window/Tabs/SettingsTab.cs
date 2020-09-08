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
        const string k_MaskTextPlaceholder = "Paste mask branches to increase the build number";

        readonly TextField m_SpreadsheetIdText;
        readonly TextField m_MaskText;
        readonly VisualElement m_ListMask;

        public SettingsTab()
            : base($"{BuildSystemPackage.WindowTabsPath}/SettingsTab")
        {
            m_SpreadsheetIdText = this.Q<TextField>("spreadsheetIdText");
            m_SpreadsheetIdText.value = BuildSystemSettings.Instance.SpreadsheetId;
            var spreadsheetIdText = this.Q<Button>("save-id");
            spreadsheetIdText.clicked += SaveId;
            m_MaskText = this.Q<TextField>("mask-text");
            m_MaskText.value = k_MaskTextPlaceholder;
            m_MaskText.tooltip = k_MaskTextPlaceholder;
            var addMask = this.Q<Button>("add-mask");
            addMask.clicked += AddMask;
            var clearMask = this.Q<Button>("clear-mask");
            clearMask.clicked += ClearMaskList;
            m_ListMask = this.Q<VisualElement>("listMask");
            foreach (var mask in BuildSystemSettings.Instance.MaskList)
            {
                CreateMaskElement(mask);
            }
        }

        public void SaveId()
        {
            BuildSystemSettings.Instance.SetSpreadsheetId(m_SpreadsheetIdText.value);
        }

        public void AddMask()
        {
            BuildSystemSettings.Instance.AddMask(m_MaskText.value);
            CreateMaskElement(m_MaskText.value);
            m_MaskText.value = k_MaskTextPlaceholder;
        }

        public void CreateMaskElement(string mask)
        {
            var visualElement = new VisualElement();
            visualElement.AddToClassList("mask-element");
            var label = new Label { text = mask };
            visualElement.Add(label);
            var btnRemove = new Button { tooltip = "Remove mask" };
            btnRemove.AddToClassList("btn-remove");
            btnRemove.clicked += () => { RemoveMask(visualElement, mask);};
            visualElement.Add(btnRemove);
            m_ListMask.Add(visualElement);
        }

        public void RemoveMask(VisualElement visualElement, string mask)
        {
            m_ListMask.Remove(visualElement);
            BuildSystemSettings.Instance.RemoveMask(mask);
        }

        public void ClearMaskList()
        {
            m_ListMask.Clear();
            BuildSystemSettings.Instance.ClearMaskList();
        }
    }
}
