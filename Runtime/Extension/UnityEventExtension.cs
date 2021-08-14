using UnityEngine.Events;

namespace GUA.Extension
{
    public static class UnityEventExtension
    {
        public static void SetListener<T>(this UnityEvent<T> unityEvent, UnityAction<T> action)
        {
            unityEvent.RemoveAllListeners();
            unityEvent.AddListener(action);
        }
    }
}