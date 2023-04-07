using System.Collections;

namespace StansAssets.Build.Pipeline
{
    /// <summary>
    /// Represents a single async build task.
    /// </summary>
    public interface IAsyncBuildTask : IBuildTask
    {
        IEnumerator Run(IBuildContext buildContext);
    }
}
