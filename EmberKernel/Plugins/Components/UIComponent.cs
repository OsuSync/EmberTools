using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmberKernel.Plugins.Components
{
    public abstract class UIComponent : IEntryComponent
    {
        protected Thread UIThread { get; }
        private TaskCompletionSource<bool> UIRunCompletionSource { get; }
        public UIComponent()
        {
            UIRunCompletionSource = new TaskCompletionSource<bool>();
            UIThread = new Thread(() =>
            {
                try
                {
                    UIRunCompletionSource.SetResult(Run());
                }
                catch (Exception e)
                {
                    UIRunCompletionSource.SetException(e);
                }
            });
            UIThread.SetApartmentState(ApartmentState.STA);
        }
        public void Dispose()
        {
            Stop();
            UIThread.Abort();
        }

        public Task Start()
        {
            UIThread.Start();
            return UIRunCompletionSource.Task;
        }

        public abstract bool Run();
        public abstract void Stop();
    }
}
