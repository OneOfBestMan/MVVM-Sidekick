using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MVVMSidekick.Views
{
   public interface IDispatcher
    {
        Task ExecuteInUIThread(Action action);
    }
}
