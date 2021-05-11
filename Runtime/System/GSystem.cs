using System.Collections.Generic;
using GUA.Data;
using GUA.Event;
using GUA.Invoke;

namespace GUA.System
{
    public sealed class GSystem
    {
        private readonly List<IFixedRunSystem> _fixedRunSystems = new List<IFixedRunSystem>();
        private readonly List<IRunSystem> _runSystems = new List<IRunSystem>();
        private readonly List<IStartSystem> _startSystems = new List<IStartSystem>();

        public void Add(ISystem system)
        {
            if (system is IStartSystem startSystem) _startSystems.Add(startSystem);
            if (system is IRunSystem runSystem) _runSystems.Add(runSystem);
            if (system is IFixedRunSystem fixedRunSystem) _fixedRunSystems.Add(fixedRunSystem);
        }

        public void Initialize()
        {
            _startSystems.ForEach(system => system.Start());
        }

        public void Run()
        {
            _runSystems.ForEach(system => system.Run());
        }

        public void FixedRun()
        {
            _fixedRunSystems.ForEach(system => system.FixedRun());
        }

        public void Destroy()
        {
            ClearSystem();
        }

        public void ApplicationQuit()
        {
            ClearSystem(true);
        }

        private static void ClearSystem(bool fullClear = false)
        {
            GDataPool.Clear();
            GEventPool.Clear();
            if (fullClear) GInvoke.Instance.Clear();
        }
    }
}