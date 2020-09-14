namespace StansAssets.Build.Editor
{
    /// <summary>
    /// IBuildStep execution result report data
    /// </summary>
    public class BuildStepResultArgs
    {
        public IBuildStep Step;
        public bool IsSuccess;
        public string ResultMessage;
    }
}
