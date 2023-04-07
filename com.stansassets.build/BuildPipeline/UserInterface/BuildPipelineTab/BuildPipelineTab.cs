using System.Collections.Generic;
using JetBrains.Annotations;
using StansAssets.Build.Editor;
using StansAssets.Plugins.Editor;
using UnityEditor;
using UnityEngine.UIElements;

namespace StansAssets.Build.Pipeline
{
    [UsedImplicitly]
    class BuildPipelineTab : BaseTab, IBuildSystemWindowTab
    {
        readonly VisualElement m_PreProcessTasksContainer;
        readonly VisualElement m_PostProcessTasksContainer;
        readonly VisualElement m_ScenePostProcessTasksContainer;

        readonly Label m_TasksProviderName;

        VisualElement m_SpreadsheetsListContainer;
        VisualElement m_ListMask;
        TextField m_MaskText;

        public string Title => "Build Pipeline";
        public VisualElement Tab => this;

        public BuildPipelineTab()
            : base($"{BuildPipelineSettings.WindowTabsPath}/{nameof(BuildPipelineTab)}/{nameof(BuildPipelineTab)}")
        {
            m_TasksProviderName = this.Q<Label>("providerName");
            m_TasksProviderName.AddToClassList("list-build-entity");
            m_PreProcessTasksContainer = this.Q<VisualElement>("listPreProcess");
            m_PostProcessTasksContainer = this.Q<VisualElement>("listPostProcess");
            m_ScenePostProcessTasksContainer = this.Q<VisualElement>("scenePostProcess");

            SetBuildTasks(BuildProcessor.GenerateBuildTasksContainer(), BuildProcessor.GetProviderName());
        }

        public void SetBuildTasks(IBuildTasksContainer buildTasksContainer, string providerName)
        {
            m_TasksProviderName.text = providerName;
            RenderBuildTasks(m_PreProcessTasksContainer, buildTasksContainer.PreBuildTasks);
            RenderBuildTasks(m_ScenePostProcessTasksContainer, buildTasksContainer.ScenePostProcessTasks);
            RenderBuildTasks(m_PostProcessTasksContainer, buildTasksContainer.PostBuildTasks);
        }

        void RenderBuildTasks(VisualElement container, IReadOnlyCollection<object> buildTasks)
        {
            container.Clear();
            if (buildTasks.Count > 0)
            {
                foreach (var task in buildTasks)
                {
                    var label = new Label { text = $"- {ObjectNames.NicifyVariableName(task.GetType().Name)}" };
                    label.AddToClassList("item-build-entity");
                    container.Add(label);
                }
            }
            else
            {
                var label = new Label { text = "No Tasks Defined" };
                label.AddToClassList("item-build-entity");
                label.AddToClassList("italic");
                container.Add(label);
            }
        }
    }
}
