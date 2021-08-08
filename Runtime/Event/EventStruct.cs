using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace GUA.Event
{
    public struct EventStruct
    {
        public Type EventType { get; }
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

        public EventStruct(Type eventType, UnityAction<object> action)
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
            thisStruct._events.ForEach(e => e.Invoke(thisStruct._eventObject));
        }
    }
}