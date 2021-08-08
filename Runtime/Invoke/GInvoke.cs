using System.Threading;
using System.Threading.Tasks;
using UnityEngine.Events;

namespace GUA.Invoke
{
    public class GInvoke
    {
        private CancellationTokenSource _cancellationTokenSource;
        
        public async void Delay(UnityAction unityAction, int milliseconds)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            await Task.Delay(milliseconds, _cancellationTokenSource.Token);
            unityAction.Invoke();
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}