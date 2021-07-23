using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StansAssets.Build.Pipeline
{
    /// <summary>
    /// Scene Post process task.
    /// The task is performed for each scenes included into the build.
    /// </summary>
    public interface IScenePostProcessStep
    {
        /// <summary>
        /// Perform scene post processing.
        /// </summary>
        /// <param name="buildContext">Build context.</param>
        /// <param name="scene">Scene to process.</param>
        void OnPostprocessScene(IBuildContext buildContext, Scene scene);

        /// <summary>
        /// Postprocess each GameObject and Components on it in a scene
        /// </summary>
        /// <param name="buildContext">Build context.</param>
        /// <param name="gameObject">Root GameObject.</param>
        /// <param name="components">Components on object.</param>
        void OnPostprocessGameObject(IBuildContext buildContext, GameObject gameObject, List<Component> components);
    }
}
