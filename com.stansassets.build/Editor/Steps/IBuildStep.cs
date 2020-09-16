using System;

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
        /// <param name="buildContext">Data class with necessary parameters for build execution</param>
        /// <param name="onComplete">Delegate which invokes when  step executing will be completed</param>
        void Execute(IBuildContext buildContext, Action<BuildStepResultArgs> onComplete = null);

        /// <summary>
        /// Queue number of the step
        /// (use less then 0 value if it needs to run before build step)
        /// </summary>
        int Priority { get; }
    }
}
