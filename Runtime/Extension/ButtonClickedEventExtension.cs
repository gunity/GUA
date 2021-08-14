using UnityEngine.Events;
using UnityEngine.UI;

namespace GUA.Extension
{
    public static class ButtonClickedEventExtension
    {
        public static void SetListener(this Button.ButtonClickedEvent unityEvent, UnityAction action)
        {
            unityEvent.RemoveAllListeners();
            unityEvent.AddListener(action);
        }
    }
}