using System;
using StansAssets.Foundation.Models;

namespace StansAssets.Build.Pipeline
{
    /// <summary>
    /// Describes a single build step.
    /// </summary>
    public interface IBuildStep
    {
        void Run(IBuildContext buildContext, Action<Result> onComplete);
    }
}
