using System.Collections;
using GUA.Extension;
using UnityEngine;
using UnityEngine.Events;

namespace GUA.Invoke
{
    public class GInvoke : Singleton<GInvoke>
    {
        private struct DelayStruct
        {
            public UnityAction UnityAction;
            public float DelayTime;
            public bool UnscaledTime;
        }
        
        public void Delay(UnityAction action, float delaySeconds, bool unscaledTime = false)
        {
            StartCoroutine(nameof(DelayCoroutine), new DelayStruct
            {
                UnityAction = action,
                DelayTime = delaySeconds,
                UnscaledTime = unscaledTime
            });
        }

        private IEnumerator DelayCoroutine(DelayStruct delayStruct)
        {
            if (delayStruct.UnscaledTime) yield return new WaitForSecondsRealtime(delayStruct.DelayTime);
            else yield return new WaitForSeconds(delayStruct.DelayTime);
            delayStruct.UnityAction.Invoke();
            yield return true;
        }
        
        public void Clear()
        {
            StopAllCoroutines();
        }
    }
}