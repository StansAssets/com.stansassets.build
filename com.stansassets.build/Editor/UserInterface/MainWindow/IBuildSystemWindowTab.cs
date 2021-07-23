using UnityEngine.UIElements;

namespace StansAssets.Build.Editor
{
    public interface IBuildSystemWindowTab
    {
        string Title { get; }
        VisualElement Tab { get; }
    }
}
