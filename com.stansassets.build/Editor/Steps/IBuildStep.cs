namespace StansAssets.Build.Editor
{    
    /// <summary>
    /// Single functional field of build process
    /// </summary>
    public interface IBuildStep
    {    
        /// <summary>
        /// Run step execution
        /// </summary>
        bool Execute(BuildContext buildContext);
        
        /// <summary>
        /// Queue number of the step
        /// (use less then 0 value if it needs to run before build step)
        /// </summary>
        int Priority { get; }
    }
}
