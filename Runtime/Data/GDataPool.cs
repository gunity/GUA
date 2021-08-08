using System;
using System.Collections.Generic;

namespace GUA.Data
{
    public static class GDataPool
    {
        private static readonly Dictionary<Type, object> Data = new Dictionary<Type, object>();

        public static void Set(object data)
        {
#if UNITY_EDITOR
            if (Data.ContainsKey(data.GetType()))
            {
                throw new Exception($"<color=green>GDataPool:</color> <b>{data.GetType()}</b> type has already been added");
            }
#endif
            Data.Add(data.GetType(), data);
        }

        public static T Get<T>()
        {
#if UNITY_EDITOR
            if (!Data.ContainsKey(typeof(T)))
            {
                throw new Exception($"<color=green>GDataPool:</color> <b>{typeof(T)}</b> type has not been added");
            }
#endif
            return (T) Data[typeof(T)];
        }
        
        public static void Clear()
        {
            Data.Clear();
        }
    }
}