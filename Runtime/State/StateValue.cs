using System;
using System.Collections.Generic;

namespace GUA.State
{
    public class StateValue
    {
        public Enum StateType
        {
            get => _stateType;
            set
            {
                _stateType = value;
                RefreshExecutableObject();
            }
        }

        public object ExecutableObject;

        private Enum _stateType;
        private readonly object _mainExecutableObject;
        private readonly Dictionary<Enum, object> _states;

        public StateValue(Enum stateType, object mainExecutableObject)
        {
            _states = new Dictionary<Enum, object>();
            ExecutableObject = mainExecutableObject;
            _mainExecutableObject = mainExecutableObject;
            StateType = stateType;
        }

        public void RegisterState(Enum state, object executableObject)
        {
            _states.Add(state, executableObject);
            RefreshExecutableObject();
        }

        private void RefreshExecutableObject()
        {
            ExecutableObject = _states.ContainsKey(_stateType) 
                ? _states[_stateType] 
                : _mainExecutableObject;
        }
    }
}