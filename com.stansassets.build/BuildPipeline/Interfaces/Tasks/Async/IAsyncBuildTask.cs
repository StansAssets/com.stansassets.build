using System.Collections;

namespace StansAssets.Build.Pipeline
{
    /// <summary>
    /// Represents a single async build step.
    /// </summary>
    public interface IAsyncBuildTask : IBuildTask
    {
        IEnumerator Run(IBuildContext buildContext);
    }
}
