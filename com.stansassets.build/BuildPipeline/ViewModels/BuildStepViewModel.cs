using System;

namespace StansAssets.Build.Pipeline
{
    public class BuildStepViewModel : IComparable<BuildStepViewModel>
    {
        public readonly string name;
        public readonly int callbackOrder;
        
        public BuildStepViewModel(string name, int callbackOrder)
        {
            this.name = name;
            this.callbackOrder = callbackOrder;
        }

        public int CompareTo(BuildStepViewModel other)
        {
            if (ReferenceEquals(this, other)) return 0;

            return ReferenceEquals(null, other)
                ? 1
                : callbackOrder.CompareTo(other.callbackOrder);
        }
    }
}
