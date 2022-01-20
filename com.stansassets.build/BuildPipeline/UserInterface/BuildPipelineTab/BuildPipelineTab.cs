using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using StansAssets.Build.Editor;
using StansAssets.Plugins.Editor;
using UnityEngine.UIElements;

namespace StansAssets.Build.Pipeline
{
    [UsedImplicitly]
    class BuildPipelineTab : BaseTab, IBuildSystemWindowTab
    {
        const string k_PreProcessStageBaseName = "preProcessSteps";
        const string k_SceneProcessStageBaseName = "sceneProcessSteps";
        const string k_PostProcessStageBaseName = "postProcessSteps";

        readonly Label m_TasksProviderName;

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

            var unityBuildStepsProvider = new UnityBuildStepsViewModelProvider();
            var buildStepsProvider = new DefaultBuildStepsViewModelProvider();

            SetBuildSteps(unityBuildStepsProvider.GetBuildSteps(), buildStepsProvider.GetBuildSteps());
            SetProvider(BuildProcessor.GetProviderName());
        }

        void SetProvider(string providerName)
        {
            m_StepsProviderName.text = providerName;
        }
        
        void SetBuildSteps(IBuildStepsViewModelContainer unityBuildStepsContainer, IBuildStepsViewModelContainer buildStepsContainer)
        {
            AddStageBuildSteps(k_PreProcessStageBaseName, unityBuildStepsContainer.PreBuildSteps, buildStepsContainer.PreBuildSteps);
            AddStageBuildSteps(k_SceneProcessStageBaseName, unityBuildStepsContainer.SceneProcessSteps, buildStepsContainer.SceneProcessSteps);
            AddStageBuildSteps(k_PostProcessStageBaseName, unityBuildStepsContainer.PostBuildSteps, buildStepsContainer.PostBuildSteps);
        }

        void AddStageBuildSteps(string containerBaseName, IReadOnlyList<BuildStepViewModel> unityBuildSteps, IReadOnlyList<BuildStepViewModel> buildSteps)
        {
            var containerBefore = Root.Q<VisualElement>($"{containerBaseName}Before");
            var container = Root.Q<VisualElement>(containerBaseName);
            var containerAfter = Root.Q<VisualElement>($"{containerBaseName}After");

            var callbackOrder = BuildProcessor.GetCallbackOrder();
            var stepsBefore = unityBuildSteps.Where(step => step.callbackOrder <= callbackOrder);
            var stepsAfter = unityBuildSteps.Where(step => step.callbackOrder > callbackOrder);
            
            AddUnityBuildSteps(containerBefore, stepsBefore);
            AddBuildSteps(container, buildSteps);
            AddUnityBuildSteps(containerAfter, stepsAfter);
        }

        static void AddUnityBuildSteps(VisualElement container, IEnumerable<BuildStepViewModel> buildSteps)
        {
            if (container == null)
                return;

            foreach (var step in buildSteps)
            {
                var label = new Label { text = $"{step.callbackOrder} {step.name}" };
                label.AddToClassList("item-build-entity");
                container.Add(label);
            }
        }

        static void AddBuildSteps(VisualElement container, IEnumerable<BuildStepViewModel> buildSteps)
        {
            if (container == null)
                return;

            if (!buildSteps.Any())
            {
                var label = new Label { text = "No Build Steps Defined" };
                label.AddToClassList("item-build-entity");
                label.AddToClassList("italic");
                container.Add(label);
                
                return;
            }
                
            foreach (var step in buildSteps)
            {
                var label = new Label { text = $"- {step.name}" };
                label.AddToClassList("item-build-entity");
                container.Add(label);
            }
        }
    }
}
