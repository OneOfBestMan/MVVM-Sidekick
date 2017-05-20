using MVVMSidekick.Common;
using MVVMSidekick.EventRouting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MVVMSidekick.ViewModels
{
    /// <summary>
    /// Interface IBindable
    /// </summary>
    public interface IBindable : INotifyPropertyChanged, IDisposable, IDisposeGroup
    {

        /// <summary>
        /// Gets the Global event router.
        /// </summary>
        /// <value>The event router.</value>
        EventRouter GlobalEventRouter { get; }

        /// <summary>
        /// Gets or sets the event router.
        /// </summary>
        /// <value>The event router.</value>
        EventRouter LocalEventRouter { get; set; }
        /// <summary>
        /// Gets the bindable instance identifier.
        /// </summary>
        /// <value>The bindable instance identifier.</value>
        string BindableInstanceId { get; }

        /// <summary>
        /// Gets the error.
        /// </summary>
        /// <value>The error.</value>
        string ErrorMessage { get; }

        //IDictionary<string,object >  Values { get; }
        /// <summary>
        /// Gets the field names.
        /// </summary>
        /// <returns>System.String[].</returns>
        string[] GetFieldNames();
        /// <summary>
        /// Gets or sets the <see cref="System.Object"/> with the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>System.Object.</returns>
        object this[string name] { get; set; }

        IValueContainer GetValueContainer(string propertyName);
    }
}
