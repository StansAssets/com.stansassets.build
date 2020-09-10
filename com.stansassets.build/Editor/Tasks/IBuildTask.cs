namespace StansAssets.Build.Editor
{
    public interface IBuildTask
    {
        void OnPostprocessScene(PlatformType type);
        int Priority { get; }
    }
}
