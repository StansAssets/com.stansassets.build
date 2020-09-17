namespace StansAssets.Build.Editor
{
    /// <summary>
    /// Single listener which runs when build started.
    /// Will be called before run all steps and tasks
    /// </summary>
    public interface IBuildExecutorListener
    {
        /// <summary>
        /// Checking if include this listener to build
        /// </summary>
        bool Active { get; }
        /// <summary>
        /// Register executor listener
        /// </summary>
        /// <param name="buildContext">Data class with necessary parameters for build execution</param>
        void Register(IBuildContext context);
    }
}
