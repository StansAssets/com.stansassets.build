using UnityEngine;

namespace StansAssets.Build.Editor
{
    public interface IBuildTask
    {    
        /// <summary>
        /// Method runs on opened scene
        /// </summary>
        /// <param name="type"></param>
        void OnPostprocessScene();
        
        /// <summary>
        /// Queue number of the step
        /// (use less then 0 value if it needs to run before build step)
        /// </summary>
        int Priority { get; }
    }
}
