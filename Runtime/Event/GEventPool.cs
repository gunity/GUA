using System;
using System.Collections.Generic;
using System.Linq;

namespace GUA.Event
{
    public static class GEventPool
    {
        private static readonly List<EventStruct> GEvents = new List<EventStruct>();

        public static void SendMessage<T>(T gEvent) where T : struct
        {
            var any = GEvents.Any(e =>
            {
                if (e.EventType != gEvent.GetType()) return false;
                if (!e.HasObject) e.SetObject(gEvent);
                e.Invoke();
                return true;
            });

            if (any) return;
            GEvents.Add(new EventStruct(gEvent));
        }

        public static void AddListener<T>(Action<T> uEventMethod)
        {
            var any = GEvents.Any(e =>
            {
                if (e.EventType != typeof(T)) return false;
                e.AddAction(t => uEventMethod((T) t));
                return true;
            });
            
            if (any) return;
            var eventStruct = new EventStruct(typeof(T), t => uEventMethod((T) t));
            GEvents.Add(eventStruct);
        }
        
        public static void Clear()
        {
            GEvents.Clear();
        }
    }
}