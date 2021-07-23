using System;
using System.Collections.Generic;

namespace StansAssets.Build.Editor
{
    public class BuildSettings
    {
        readonly Dictionary<string, object> m_Data = new Dictionary<string, object>();

        public void AddData(string key, object data)
        {
            m_Data.Add(key, data);
        }

        public void AddData<T>(string key, T data)
        {
            m_Data.Add(key, data);
        }

        public bool TryGetData(string key, out object data)
        {
            return m_Data.TryGetValue(key, out data);
        }

        public bool TryGetData<T>(string key, out T data)
        {
            data = default;
            if (m_Data.TryGetValue(key, out var objData))
            {
                if (objData is T == false)
                {
                    throw new ArgumentException($"Object with key {key} can't be casted to {typeof(T).Name}. Actual type is: {objData.GetType().Name}");
                }
                data = (T)objData;
                return true;
            }

            return false;
        }

        internal void Reset(){
            m_Data.Clear();
        }
    }
}
