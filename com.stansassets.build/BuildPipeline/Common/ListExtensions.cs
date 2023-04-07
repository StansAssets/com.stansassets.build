using System;
using System.Collections.Generic;

namespace StansAssets.Build.Pipeline
{
    public static class ListExtensions
    {
        // borrowed from https://stackoverflow.com/a/22801345
        public static void AddSorted<T>(this List<T> @this, T item) where T : IComparable<T>
        {
            if (@this.Count == 0)
            {
                @this.Add(item);
                return;
            }

            if (@this[@this.Count - 1].CompareTo(item) <= 0)
            {
                @this.Add(item);
                return;
            }

            if (@this[0].CompareTo(item) >= 0)
            {
                @this.Insert(0, item);
                return;
            }

            var index = @this.BinarySearch(item);
            if (index < 0)
                index = ~index;

            @this.Insert(index, item);
        }
    }
}
