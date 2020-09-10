namespace StansAssets.Build.Editor
{
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
