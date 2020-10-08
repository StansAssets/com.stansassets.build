namespace StansAssets.Build.Editor
{
    /// <summary>
    /// Single listener which runs when build started.
    /// Will be called before run all steps and tasks
    /// </summary>
    public interface IBuildExecutorListener
    {
        /// <summary>
        /// Indicates is listener active or not while registration flow. It allows toggle to build steps/tasks registration without re-compile
        /// </summary>
        bool Active { get; }

        /// <summary>
        /// Indicates an execution priority of a <see cref="Register"/> method
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// Register executor listener
        /// </summary>
        /// <param name="buildContext">Data class with necessary parameters for build execution</param>
        void Register(IBuildContext buildContext);
    }
}
