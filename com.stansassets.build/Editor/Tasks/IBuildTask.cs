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
        void OnPostprocessScene();
        
        /// <summary>
        /// Queue number of the step
        /// (use less then 0 value if it needs to run before build step)
        /// </summary>
        int Priority { get; }
    }
}
