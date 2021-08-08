using System.Collections.Generic;
using GUA.Data;
using GUA.Event;

namespace GUA.System
{
    public sealed class GSystem
    {
        private readonly List<MonoSystem> _systems = new List<MonoSystem>();

        public void Add(MonoSystem system)
        {
            _systems.Add(system);
        }

        public void Run()
        {
            _systems.ForEach(system =>
            {
                if (system.Enabled) system.Run();
            });
        }

        public void FixedRun()
        {
            _systems.ForEach(system =>
            {
                if (system.Enabled) system.FixedRun();
            });
        }

        public static void ClearSystem()
        {
            GDataPool.Clear();
            GEventPool.Clear();
        }
    }
}