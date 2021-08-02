using JetBrains.Annotations;
using StansAssets.Build.Editor;
using StansAssets.Plugins.Editor;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
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

        public void SetBuildSteps(IBuildTasksContainerFull buildTasksContainer, string providerName)
        {
            m_StepsProviderName.text = providerName;
            RenderBuildSteps(m_PreProcessStepsContainer, buildTasksContainer.UnityPreBuildTasks, buildTasksContainer.PreBuildTasks);
            RenderBuildSteps(m_ScenePostProcessStepsContainer, buildTasksContainer.UnityPostBuildTasks, buildTasksContainer.ScenePostProcessTasks);
            RenderBuildSteps(m_PostProcessStepsContainer, buildTasksContainer.UnityProcessSceneTasks, buildTasksContainer.PostBuildTasks);
        }

        static void RenderBuildSteps(VisualElement container, IReadOnlyCollection<IOrderedCallback> unityBuildSteps, IReadOnlyCollection<object> buildSteps)
        {
            container.Clear();
            if (unityBuildSteps.Count > 0 || buildSteps.Count > 0)
            {
                RenderBuildSteps(container, unityBuildSteps.Where(step => step.callbackOrder <= 0));
                RenderBuildSteps(container, buildSteps);
                RenderBuildSteps(container, unityBuildSteps.Where(step => step.callbackOrder > 0));
            }
            else
            {
                var label = new Label { text = "No Tasks Defined" };
                label.AddToClassList("item-build-entity");
                label.AddToClassList("italic");
                container.Add(label);
            }
        }

        static void RenderBuildSteps(VisualElement container, IEnumerable<object> buildSteps)
        {
            foreach (var step in buildSteps)
            {
                var stepType = step.GetType();
                var label = new Label { text = $"- {ObjectNames.NicifyVariableName(stepType.Name)}" };
                label.AddToClassList("item-build-entity");
                container.Add(label);
            }
        }
    }
}
