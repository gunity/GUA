using UnityEngine;

namespace GUA.System
{
    public abstract class MonoSystem
    {
        public bool Enabled
        {
            get => _enabled;
            protected set 
            {
                _enabled = value;
#if UNITY_EDITOR
                Debug.Log($"<color=green>GSystem:</color> system <b>{GetType()}</b> turned {(Enabled ? "<color=green>on" : "<color=red>off")}</color>");
#endif
                Startup();
            }
        }
        
        private bool _enabled;
        private bool _initialized;

        protected MonoSystem(bool enabled)
        {
            Enabled = enabled;
            Startup();
        }

        private void Startup()
        {
            if (!Enabled) return;
            if (_initialized) return;
            
            _initialized = true;
            Start();
        }

        protected virtual void Start()
        {
            // nothing
        }

        public virtual void Run()
        {
            // nothing
        }

        public virtual void FixedRun()
        {
            // nothing
        }
    }
}