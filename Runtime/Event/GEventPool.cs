using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

namespace GUA.Event
{
    public static class GEventPool
    {
        private static readonly List<EventStruct> UEvents = new List<EventStruct>();

        public static void SendMessage(object uEvent)
        {
            var any = UEvents.Any(e =>
            {
                if (e.EventType != uEvent.GetType()) return false;
                if (!e.HasObject) e.SetObject(uEvent);
                e.Invoke();
                return true;
            });
            if (!any) UEvents.Add(new EventStruct(uEvent));
        }

        public static void AddListener<T>(Action<T> uEventMethod)
        {
            var any = UEvents.Any(e =>
            {
                if (e.EventType != typeof(T)) return false;
                e.AddAction(t => uEventMethod((T) t));
                return true;
            });
            if (any) return;
            {
                var eventStruct = new EventStruct(typeof(T), t => uEventMethod((T) t));
                UEvents.Add(eventStruct);
            }
        }
        
        public static void Clear()
        {
            UEvents.Clear();
        }

        private struct EventStruct
        {
            public global::System.Type EventType { get; }
            public bool HasObject { get; private set; }

            private object _eventObject;
            private readonly List<UnityAction<object>> _events;

            public EventStruct(object eventObject)
            {
                _eventObject = eventObject;
                _events = new List<UnityAction<object>>();
                EventType = eventObject.GetType();
                HasObject = true;
            }

            public EventStruct(global::System.Type eventType, UnityAction<object> action)
            {
                _eventObject = null;
                _events = new List<UnityAction<object>>
                {
                    action
                };
                EventType = eventType;
                HasObject = false;
            }

            public void SetObject(object eventObject)
            {
                _eventObject = eventObject;
                HasObject = true;
            }

            public void AddAction(UnityAction<object> action)
            {
                _events.Add(action);
            }

            public void Invoke()
            {
                var thisStruct = this;
                thisStruct._events.ForEach(e => { e.Invoke(thisStruct._eventObject); });
            }
        }
    }
}