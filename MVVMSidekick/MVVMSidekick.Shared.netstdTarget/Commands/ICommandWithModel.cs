using MVVMSidekick.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace MVVMSidekick.Commands
{
    /// <summary>
    /// 带有VM的Command接口
    /// </summary>
    public interface ICommandWithViewModel : ICommand
    {
        /// <summary>
        /// Gets or sets the view model.
        /// </summary>
        /// <value>The view model.</value>
        BindableBase ViewModel { get; set; }
    }
}
