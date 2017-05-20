using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace MVVMSidekick.ViewModels
{

    /// <summary>
    /// Interface IValueCanSet
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IValueCanSet<in T>
    {
        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <value>The value.</value>
        T Value { set; }
    }

    /// <summary>
    /// Interface IValueCanGet
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IValueCanGet<out T>
    {
        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
        T Value { get; }
    }



    /// <summary>
    /// Interface IValueContainer
    /// </summary>
    public interface IValueContainer : IErrorInfo, INotifyChanges
    {
        string PropertyName { get; }

        /// <summary>
        /// Gets the type of the property.
        /// </summary>
        /// <value>The type of the property.</value>
        Type PropertyType { get; }
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        Object Value { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is copy to allowed.
        /// </summary>
        /// <value><c>true</c> if this instance is copy to allowed; otherwise, <c>false</c>.</value>
        bool IsCopyToAllowed { get; set; }

        void AddErrorEntry(string message, Exception exception = null);

    }

}
