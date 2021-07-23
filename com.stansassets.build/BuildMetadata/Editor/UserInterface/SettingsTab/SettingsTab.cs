using System;
using System.Linq;
using JetBrains.Annotations;
using StansAssets.Build.Meta.Editor;
using StansAssets.Foundation.Editor;
using StansAssets.Plugins.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
#if GOOGLE_DOC_CONNECTOR_PRO_ENABLED
using StansAssets.GoogleDoc;
#endif

namespace StansAssets.Build.Editor
{
    [UsedImplicitly]
    class SettingsTab : BaseTab, IBuildSystemWindowTab
    {
        public event Action UpdateBuildEntitiesCallback = delegate { };

        const string k_MaskTextPlaceholder = "Paste mask branches to increase the build number";

        readonly VisualElement m_ListBuildStep;
        readonly VisualElement m_ListBuildTask;

        VisualElement m_SpreadsheetsListContainer;
        VisualElement m_ListMask;
        TextField m_MaskText;

        const string k_DefaultSpreadsheetField = "None";

        public string Title => "Build Version";
        public VisualElement Tab => this;

        public SettingsTab()
            : base($"{BuildSystemSettings.WindowTabsPath}/SettingsTab/SettingsTab")
        {
            m_ListBuildStep = this.Q<VisualElement>("listBuildStep");
            m_ListBuildTask = this.Q<VisualElement>("listBuildTask");

            var downloadGoogleDocConnector = this.Q<Button>("download-google-doc-connector-pro");
            downloadGoogleDocConnector.clicked += DownloadGoogleDoc;

           // var refreshBtn = this.Q<Button>("refreshBtn");
            //refreshBtn.clicked += UpdateBuildEntitiesCallback;

            //SetBuildEntities(null, null);
            BindVersionIncrementSection();
        }

        void BindVersionIncrementSection()
        {
            var incrementBuildNumberToggle = this.Q<Toggle>("incrementBuildNumber-toggle");
            incrementBuildNumberToggle.RegisterValueChangedCallback(e =>
            {
                BuildSystemSettings.Instance.AutomatedBuildNumberIncrement = e.newValue;
            });

            var automatedBuildNumber = BuildSystemSettings.Instance.AutomatedBuildNumberIncrement;
            incrementBuildNumberToggle.SetValueWithoutNotify(automatedBuildNumber);

            var googleDocMissingBlock = this.Q("google-doc-missing-block");
            var googleDocInstalledBlock = this.Q("google-doc-installed-block");

            m_SpreadsheetsListContainer = this.Q<VisualElement>("list-spreadsheet");
            if (StanAssetsPackages.IsGoogleDocConnectorProInstalled)
            {
#if GOOGLE_DOC_CONNECTOR_PRO_ENABLED
                var versionIncrementBlock = this.Q("version-increment");
                UIToolkitEditorUtility.ApplyStyle(versionIncrementBlock, GoogleDocConnectorPackage.LocalizationTabPath);

                var openBtn = this.Q<Button>("openBtn");
                openBtn.clicked += () =>
                {
                    Application.OpenURL(BuildVersionsSpreadsheet.Url);
                };
                CreateListSpreadsheet();
#endif
                googleDocInstalledBlock.style.display = DisplayStyle.Flex;
                googleDocMissingBlock.style.display = DisplayStyle.None;
            }
            else
            {
                googleDocInstalledBlock.style.display = DisplayStyle.None;
                googleDocMissingBlock.style.display = DisplayStyle.Flex;
            }

            m_MaskText = this.Q<TextField>("mask-text");
            m_ListMask = this.Q<VisualElement>("listMask");
            m_MaskText.value = k_MaskTextPlaceholder;
            m_MaskText.tooltip = k_MaskTextPlaceholder;
            m_ListMask.Clear();
            foreach (var mask in BuildSystemSettings.Instance.MaskList)
            {
                CreateMaskElement(mask);
            }

            var addMask = this.Q<Button>("add-mask");
            addMask.clicked += AddMask;

            var gitRepositoryName = this.Q<TextField>("git-repo-text");
            gitRepositoryName.SetValueWithoutNotify(BuildSystemSettings.Instance.GitHubRepository);
            gitRepositoryName.RegisterValueChangedCallback((e) =>
            {
                BuildSystemSettings.Instance.GitHubRepository = e.newValue;
            });


            RebindExtraFieldsList();
            var addExtraFiledButton = this.Q<Button>("add-extra-field");
            addExtraFiledButton.clicked += () =>
            {
                BuildSystemSettings.Instance.AddNewExtraField();
                RebindExtraFieldsList();
            };
        }

        void RebindExtraFieldsList()
        {
            var extraFieldsContainer = this.Q("extra-fields-container");
            extraFieldsContainer.Clear();

            var extraFieldItemTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"{BuildSystemSettings.WindowTabsPath}/SettingsExtraFieldItem.uxml");

            foreach (var field in BuildSystemSettings.Instance.ExtraFields)
            {
                var item = extraFieldItemTree.CloneTree();
                extraFieldsContainer.Add(item);

                var nameField = item.Q<TextField>("extra-filed-name");
                var valueField = item.Q<TextField>("extra-filed-value");
                var removeButton = item.Q<Button>("remove-extra-filed");

                nameField.SetValueWithoutNotify(field.Name);
                nameField.RegisterValueChangedCallback(e =>
                {
                    field.Name = e.newValue;
                    BuildSystemSettings.Save();
                });

                valueField.SetValueWithoutNotify(field.Value);
                valueField.RegisterValueChangedCallback(e =>
                {
                    field.Value = e.newValue;
                    BuildSystemSettings.Save();
                });

                removeButton.clicked += () =>
                {
                    BuildSystemSettings.Instance.ExtraFields.Remove(field);
                    RebindExtraFieldsList();
                };
            }
        }



        void OnNameChange(ChangeEvent<string> evt, int fieldIndex) { }

#if GOOGLE_DOC_CONNECTOR_PRO_ENABLED
        Spreadsheet BuildVersionsSpreadsheet => GoogleDocConnector.GetSpreadsheet(BuildSystemSettings.Instance.SpreadsheetId) ?? new Spreadsheet();

        void CreateListSpreadsheet()
        {
            m_SpreadsheetsListContainer.Clear();
            var listName = GoogleDocConnectorSettings.Instance.Spreadsheets.Where(v => v.Name != "<Spreadsheet>").Select(v => v.Name).ToList();
            listName.Add(k_DefaultSpreadsheetField);
            var spreadsheetsPopupField = new PopupField<string>("", listName, 0) { value = BuildVersionsSpreadsheet.Name ?? k_DefaultSpreadsheetField };

            spreadsheetsPopupField.RegisterCallback<ChangeEvent<string>>(evt =>
            {
                var selectedSpreadSheet = GoogleDocConnectorSettings.Instance.Spreadsheets.FirstOrDefault(s => s.Name == evt.newValue);
                BuildSystemSettings.Instance.SpreadsheetId = selectedSpreadSheet != null
                    ? selectedSpreadSheet.Id
                    : string.Empty;
            });

            m_SpreadsheetsListContainer.Add(spreadsheetsPopupField);
        }
#endif

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

        void DownloadGoogleDoc()
        {
            Application.OpenURL("https://github.com/StansAssets/com.stansassets.google-doc-connector-pro");

            // StanAssetsPackages.AddStanAssetsPackage(StanAssetsPackages.GoogleDocConnectorProPackage, StanAssetsPackages.GoogleDocConnectorProPackageVersion);
        }

        /*
        public void SetBuildEntities([CanBeNull] IEnumerable<BuildStepEntity> buildStep, [CanBeNull] IEnumerable<BuildTaskEntity> buildTask)
        {
            m_ListBuildStep.Clear();
            if (buildStep == null || !buildStep.Any())
            {
                var label = new Label() { text = "Nothing" };
                label.AddToClassList("item-build-entity");
                label.AddToClassList("italic");
                m_ListBuildStep.Add(label);
            }
            else
            {
                foreach (var step in buildStep)
                {
                    var label = new Label() { text = $"- {step.Name}" };
                    label.AddToClassList("item-build-entity");
                    m_ListBuildStep.Add(label);
                }
            }

            m_ListBuildTask.Clear();
            if (buildTask == null || !buildTask.Any())
            {
                var label = new Label() { text = "Nothing" };
                label.AddToClassList("item-build-entity");
                label.AddToClassList("italic");
                m_ListBuildTask.Add(label);
            }
            else
            {
                foreach (var task in buildTask)
                {
                    var label = new Label() { text = $"- {task.Name}" };
                    label.AddToClassList("item-build-entity");
                    m_ListBuildTask.Add(label);
                }
            }
        }
        */
    }
}
