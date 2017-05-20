using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Windows.UI.Xaml.Controls;

namespace MVVMSidekick.Views
{
#if WPF
    public class ModelDispatcher : IDispatcher
    {
        public ModelDispatcher(System.Windows.Threading.Dispatcher core)
        {
            _core = core;
        }
        System.Windows.Threading.Dispatcher _core;

#if NET40
        public async Task ExecuteInUIThread(Action action)
        {
            var cts = new TaskCompletionSource<object>();
            _core.BeginInvoke(new Action(() =>
            {
                try
                {
                    cts.TrySetResult(null);
                }
                catch (Exception ex)
                {
                    cts.TrySetException(ex);

                }

            }));

            await cts.Task;
        }
#else
        public async Task ExecuteInUIThread(Action action)
        {
            await _core.InvokeAsync(action);
        }

#endif
    }
#elif NETFX_CORE
    public class ModelDispatcher : IDispatcher
    {

        public ModelDispatcher(Windows.UI.Core.CoreDispatcher core)
        {
            _core = core;
        }
        Windows.UI.Core.CoreDispatcher _core;
        public async Task ExecuteInUIThread(Action action)
        {
            await _core.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, new Windows.UI.Core.DispatchedHandler(action));
        }
    }
#endif

}
