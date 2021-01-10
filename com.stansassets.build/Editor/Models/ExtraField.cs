using System;
using UnityEngine;

namespace StansAssets.Build.Editor
{
    [Serializable]
    class ExtraField
    {
        [field: SerializeField]
        public string Name { get; set; }

        [field: SerializeField]
        public string Value { get; set; }
    }
}
