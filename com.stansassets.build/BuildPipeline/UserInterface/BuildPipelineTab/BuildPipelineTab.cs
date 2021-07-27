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
        readonly VisualElement m_PreProcessStepsContainer;
        readonly VisualElement m_PostProcessStepsContainer;
        readonly VisualElement m_ScenePostProcessStepsContainer;

        readonly Label m_StepsProviderName;

        VisualElement m_SpreadsheetsListContainer;
        VisualElement m_ListMask;
        TextField m_MaskText;

        public string Title => "Build Pipeline";
        public VisualElement Tab => this;

        public BuildPipelineTab()
            : base($"{BuildPipelineSettings.WindowTabsPath}/{nameof(BuildPipelineTab)}/{nameof(BuildPipelineTab)}")
        {
            m_StepsProviderName = this.Q<Label>("providerName");
            m_StepsProviderName.AddToClassList("list-build-entity");
            m_PreProcessStepsContainer = this.Q<VisualElement>("listPreProcess");
            m_PostProcessStepsContainer = this.Q<VisualElement>("listPostProcess");
            m_ScenePostProcessStepsContainer = this.Q<VisualElement>("scenePostProcess");

            SetBuildSteps(BuildProcessor.GenerateBuildStepsContainer(), BuildProcessor.GetProviderName());
        }

        public void SetBuildSteps(IBuildTasksContainer buildTasksContainer, string providerName)
        {
            m_StepsProviderName.text = providerName;
            RenderBuildSteps(m_PreProcessStepsContainer, buildTasksContainer.PreBuildTasks);
            RenderBuildSteps(m_ScenePostProcessStepsContainer, buildTasksContainer.ScenePostProcessTasks);
            RenderBuildSteps(m_PostProcessStepsContainer, buildTasksContainer.PostBuildTasks);
        }

        void RenderBuildSteps(VisualElement container, IReadOnlyCollection<object> buildSteps)
        {
            container.Clear();
            if (buildSteps.Count > 0)
            {
                foreach (var step in buildSteps)
                {
                    var label = new Label { text = $"- {ObjectNames.NicifyVariableName(step.GetType().Name)}" };
                    label.AddToClassList("item-build-entity");
                    container.Add(label);
                }
            }
            else
            {
                var label = new Label { text = "No Steps Defined" };
                label.AddToClassList("item-build-entity");
                label.AddToClassList("italic");
                container.Add(label);
            }
        }
    }
}
