using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MVVMSidekick.ViewModels
{

    /// <summary>
    /// Interface INotifyChanges
    /// </summary>
    public interface INotifyChanges
    {
        /// <summary>
        /// Occurs when [value changed with name only].
        /// </summary>
        event PropertyChangedEventHandler ValueChangedWithNameOnly;
        /// <summary>
        /// Occurs when [value changed with nothing].
        /// </summary>
        event EventHandler ValueChangedWithNothing;  //TODO:Change this Crap Name

    }
    /// <summary>
    /// Interface INotifyChanges
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface INotifyChanges<T> : INotifyChanges
    {
        /// <summary>
        /// Occurs when [value changed].
        /// </summary>
        event EventHandler<ValueChangedEventArgs<T>> ValueChanged;

    }
    /// <summary>
    /// <para>Event args that fired when property changed, with old value and new value field.</para>
    /// <para>值变化事件参数</para>
    /// </summary>
    /// <typeparam name="TProperty">Type of propery/变化属性的类型</typeparam>
    public class ValueChangedEventArgs<TProperty> : PropertyChangedEventArgs
    {
        /// <summary>
        /// Constructor of ValueChangedEventArgs
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        public ValueChangedEventArgs(string propertyName, TProperty oldValue, TProperty newValue)
            : base(propertyName)
        {
            NewValue = newValue;
            OldValue = oldValue;
        }

        /// <summary>
        /// New Value
        /// </summary>
        /// <value>The new value.</value>
        public TProperty NewValue { get; private set; }
        /// <summary>
        /// Old Value
        /// </summary>
        /// <value>The old value.</value>
        public TProperty OldValue { get; private set; }
    }
}
