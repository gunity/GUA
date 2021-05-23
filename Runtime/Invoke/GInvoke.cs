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
        
        private struct RepeatStruct
        {
            public UnityAction UnityAction1;
            public UnityAction UnityAction2;
            public float RepeatTime;
            public float RepeatCount;
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

        public void Repeat(UnityAction action1, UnityAction action2, float repeatTime, float repeatCount)
        {
            StartCoroutine(nameof(RepeatCoroutine), new RepeatStruct
            {
                UnityAction1 = action1,
                UnityAction2 = action2,
                RepeatTime = repeatTime,
                RepeatCount = repeatCount
            });
        }

        private IEnumerator RepeatCoroutine(RepeatStruct repeatStruct)
        {
            for (var index = 0; index < repeatStruct.RepeatCount; index++)
            {
                yield return new WaitForSeconds(repeatStruct.RepeatTime / repeatStruct.RepeatCount);
                if(index % 2 == 0) repeatStruct.UnityAction1.Invoke();
                else repeatStruct.UnityAction2.Invoke();
            }
        }
        
        public void Clear()
        {
            StopAllCoroutines();
        }
    }
}