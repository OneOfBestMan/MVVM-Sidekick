using MVVMSidekick.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MVVMSidekick.Views
{
#if WPF
    public class ModelDispatcher : IDispatcher
    {
        public ModelDispatcher(IViewModel vm)
        {
            _core = GetCurrentViewDispatcher(vm);
        }
        System.Windows.Threading.Dispatcher _core;

        /// <summary>
        /// Gets the current view dispatcher.
        /// </summary>
        /// <returns>Dispatcher.</returns>
        private System.Windows.Threading.Dispatcher GetCurrentViewDispatcher(IViewModel vm)
        {
            DependencyObject dp = null;
            if (vm.StageManager == null)
            {
                return null;
            }
            else if ((dp = (vm.StageManager.CurrentBindingView as DependencyObject)) == null)
            {
                return null;
            }
            return dp.Dispatcher;

        }


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

        public ModelDispatcher(IViewModel vm)
        {
            _core = GetCurrentViewDispatcher(vm);
        }
        Windows.UI.Core.CoreDispatcher _core;
        public async Task ExecuteInUIThread(Action action)
        {
            await _core.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, new Windows.UI.Core.DispatchedHandler(action));
        }


        private Windows.UI.Core.CoreDispatcher GetCurrentViewDispatcher(IViewModel vm)
        {
            Windows.UI.Xaml.DependencyObject dp = null;
            if (vm.StageManager == null)
            {
                return null;
            }
            else if ((dp = (vm.StageManager.CurrentBindingView as Windows.UI.Xaml.DependencyObject)) == null)
            {
                return null;
            }
            return dp.Dispatcher;
        }
    }
#endif

}
