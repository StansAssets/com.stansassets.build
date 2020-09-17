using System.Collections.Generic;
using UnityEngine;

namespace StansAssets.Build.Editor
{
    /// <summary>
    /// Single task which runs when build started.
    /// Will be called for all included to a build scenes
    /// </summary>
    public interface IBuildTask
    {
        /// <summary>
        /// Set context
        /// </summary>
        void SetContext(IBuildContext buildContext);

        /// <summary>
        /// Scene postprocessing started
        /// </summary>
        void OnPostprocessScene();

        /// <summary>
        /// Postprocess each GameObject and Components on it in a scene
        /// </summary>
        /// <param name="go">root GameObject</param>
        /// <param name="components">components on object</param>
        void OnPostprocessGameObject(GameObject go, List<Component> components);

        /// <summary>
        /// Incidicates when build finished
        /// </summary>
        void OnBuildFinished();

        /// <summary>
        /// Queue number of the step
        /// (use less then 0 value if it needs to run before build step)
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// Task name
        /// </summary>
        string Name { get; }
    }
}
