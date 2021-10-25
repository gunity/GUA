using System.Threading.Tasks;
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
                Startup();
            }
        }
        
        private bool _enabled;
        private bool _initialized;

        protected MonoSystem(bool enabled)
        {
            Enabled = enabled;
        }

        private async void Startup()
        {
            await Task.Yield();
            
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