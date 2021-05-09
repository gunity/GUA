using System.Collections.Generic;

namespace GUA.Data
{
    public static class GDataPool
    {
        private static readonly Dictionary<global::System.Type, object> Data = new Dictionary<global::System.Type, object>();

        public static void Set(object data)
        {
            Data.Add(data.GetType(), data);
        }

        public static T Get<T>()
        {
            return (T) Data[typeof(T)];
        }
        
        public static void Clear()
        {
            Data.Clear();
        }
    }
}