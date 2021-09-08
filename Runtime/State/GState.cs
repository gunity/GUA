using System;
using System.Collections.Generic;

namespace GUA.State
{
    public static class GState
    {
        private static readonly Dictionary<Type, StateValue> States = new Dictionary<Type, StateValue>();
        private static readonly Dictionary<Type, object> Executables = new Dictionary<Type, object>();
        
        public static void SetState<T>(T stateType, object mainExecutableObject = null) where T : Enum
        {
            var type = typeof(T);
            
            if (States.ContainsKey(type))
            {
                var state = States[type];
                state.StateType = stateType;

                if (state.ExecutableObject == null) return;
                SetExecutable(state.ExecutableObject);
            }
            else
            {
                States.Add(type, new StateValue(stateType, mainExecutableObject));
            
                if (mainExecutableObject == null) return;
                SetExecutable(mainExecutableObject);
            }
        }

        public static T GetState<T>() where T : Enum
        {
            return (T)States[typeof(T)].StateType;
        }

        public static T GetExecutable<T>() where T : class
        {
            if (!Executables.ContainsKey(typeof(T)))
                throw new Exception("Error");
            return Executables[typeof(T)] as T;
        }

        private static void SetExecutable(object executable)
        {
            var type = executable.GetType();
            var baseType = type.BaseType;

            if (baseType == null) return;
            if (baseType == typeof(object))
                Executables[type] = executable;
            else
                Executables[baseType] = executable;
        }

        public static void RegisterState(Enum state, object executable)
        {
            var currentState = States[state.GetType()];
            currentState.RegisterState(state, executable);

            if(!Equals(state, currentState.StateType)) return;
            
            SetExecutable(executable);
        }
    }
}
