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
        /// Method runs on opened scene
        /// </summary>
        void OnPostprocessScene(IBuildStepContext buildContext);

        /// <summary>
        /// Switch configuration file on object
        /// </summary>
        /// <param name="go">root GameObject</param>
        /// <param name="components">components on object</param>
        void OnPostprocessGameObject(GameObject go, List<Component> components);

        /// <summary>
        /// Set build context data
        /// </summary>
        void SetContext(IBuildStepContext buildContext);
        
        /// <summary>
        /// Queue number of the step
        /// (use less then 0 value if it needs to run before build step)
        /// </summary>
        int Priority { get; }
    }
}
