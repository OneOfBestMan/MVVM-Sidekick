// ***********************************************************************
// Assembly         : MVVMSidekick_Wp8
// Author           : waywa
// Created          : 05-17-2014
//
// Last Modified By : waywa
// Last Modified On : 01-04-2015
// ***********************************************************************
// <copyright file="ViewModels.cs" company="">
//     Copyright ©  2012
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Reflection;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Input;
using MVVMSidekick.Commands;
using System.Runtime.CompilerServices;
using MVVMSidekick.Reactive;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
#if NETFX_CORE
using Windows.UI.Xaml.Controls;


#elif WPF
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections.Concurrent;
using System.Windows.Navigation;
using MVVMSidekick.Views;
using System.Windows.Controls.Primitives;
using MVVMSidekick.Utilities;
using System.Windows.Threading;
#elif SILVERLIGHT_5 || SILVERLIGHT_4
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
#elif WINDOWS_PHONE_8 || WINDOWS_PHONE_7
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
#endif






namespace MVVMSidekick
{

    namespace ViewModels
    {
        using EventRouting;
        using System.Reactive.Disposables;
        using Utilities;
        using Views;
        using MVVMSidekick.Common;
        using System.Reactive;
        using System.Dynamic;









        ///// <summary>
        ///// <para>A Bindebale Tuple</para>
        ///// <para>一个可绑定的Tuple实现</para>
        ///// </summary>
        ///// <typeparam name="TItem1">Type of first item/第一个元素的类型</typeparam>
        ///// <typeparam name="TItem2">Type of second item/第二个元素的类型</typeparam>
        //[DataContract]
        //public class BindableTuple<TItem1, TItem2> : BindableBase<BindableTuple<TItem1, TItem2>>
        //{
        //	/// <summary>
        //	/// Initializes a new instance of the <see cref="BindableTuple{TItem1, TItem2}"/> class.
        //	/// </summary>
        //	/// <param name="item1">The item1.</param>
        //	/// <param name="item2">The item2.</param>
        //	public BindableTuple(TItem1 item1, TItem2 item2)
        //	{
        //		this.IsNotificationActivated = false;
        //		Item1 = item1;
        //		Item2 = item2;
        //		this.IsNotificationActivated = true;
        //	}
        //	/// <summary>
        //	/// 第一个元素
        //	/// </summary>
        //	/// <value>The item1.</value>

        //	public TItem1 Item1
        //	{
        //		get { return _Item1Locator(this).Value; }
        //		set { _Item1Locator(this).SetValueAndTryNotify(value); }
        //	}
        //	#region Property TItem1 Item1 Setup
        //	/// <summary>
        //	/// The _ item1
        //	/// </summary>
        //	protected Property<TItem1> _Item1 = new Property<TItem1> { LocatorFunc = _Item1Locator };
        //	/// <summary>
        //	/// The _ item1 locator
        //	/// </summary>
        //	static Func<BindableBase, ValueContainer<TItem1>> _Item1Locator = RegisterContainerLocator<TItem1>("Item1", model => model.Initialize("Item1", ref model._Item1, ref _Item1Locator, _Item1DefaultValueFactory));
        //	/// <summary>
        //	/// The _ item1 default value factory
        //	/// </summary>
        //	static Func<BindableBase, TItem1> _Item1DefaultValueFactory = null;
        //	#endregion

        //	/// <summary>
        //	/// 第二个元素
        //	/// </summary>
        //	/// <value>The item2.</value>

        //	public TItem2 Item2
        //	{
        //		get { return _Item2Locator(this).Value; }
        //		set { _Item2Locator(this).SetValueAndTryNotify(value); }
        //	}
        //	#region Property TItem2 Item2 Setup
        //	/// <summary>
        //	/// The _ item2
        //	/// </summary>
        //	protected Property<TItem2> _Item2 = new Property<TItem2> { LocatorFunc = _Item2Locator };
        //	/// <summary>
        //	/// The _ item2 locator
        //	/// </summary>
        //	static Func<BindableBase, ValueContainer<TItem2>> _Item2Locator = RegisterContainerLocator<TItem2>("Item2", model => model.Initialize("Item2", ref model._Item2, ref _Item2Locator, _Item2DefaultValueFactory));
        //	/// <summary>
        //	/// The _ item2 default value factory
        //	/// </summary>
        //	static Func<BindableBase, TItem2> _Item2DefaultValueFactory = null;
        //	#endregion


        //}
        ///// <summary>
        ///// <para>Fast create Bindable Tuple </para>
        ///// <para>帮助快速创建BindableTuple的帮助类</para>
        ///// </summary>
        //public static class BindableTuple
        //{
        //	/// <summary>
        //	/// Create a Tuple
        //	/// </summary>
        //	/// <typeparam name="TItem1">The type of the item1.</typeparam>
        //	/// <typeparam name="TItem2">The type of the item2.</typeparam>
        //	/// <param name="item1">The item1.</param>
        //	/// <param name="item2">The item2.</param>
        //	/// <returns>
        //	/// BindableTuple&lt;TItem1, TItem2&gt;.
        //	/// </returns>

        //	public static BindableTuple<TItem1, TItem2> Create<TItem1, TItem2>(TItem1 item1, TItem2 item2)
        //	{
        //		return new BindableTuple<TItem1, TItem2>(item1, item2);
        //	}

        //}



        //#if !NETFX_CORE

        //        public class StringToViewModelInstanceConverter : TypeConverter
        //        {
        //            public override bool CanConvertTo(ITypeDescriptorContext context, Type sourceType)
        //            {

        //                //if (sourceType == typeof(string))
        //                    return true;
        //                //return base.CanConvertFrom(context, sourceType);
        //            }
        //            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        //            {
        //                return true;
        //            }

        //            public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        //            {

        //                var str = value.ToString();
        //                var t = Type.GetType(str);
        //                var v = Activator.CreateInstance(t);
        //                return v;
        //                ////  return base.ConvertFrom(context, culture, value);
        //            }
        //            public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        //            {
        //                return value.ToString();
        //            }
        //        }

        //        [TypeConverter(typeof(StringToViewModelInstanceConverter))]
        //#endif



        /// <summary>
        /// Struct NoResult
        /// </summary>
        [DataContract]
        public struct NoResult
        {

        }

        /// <summary>
        /// Struct ShowAwaitableResult
        /// </summary>
        /// <typeparam name="TViewModel">The type of the t view model.</typeparam>
        public struct ShowAwaitableResult<TViewModel>
        {
            /// <summary>
            /// Gets or sets the view model.
            /// </summary>
            /// <value>The view model.</value>
            public TViewModel ViewModel { get; set; }
            /// <summary>
            /// Gets or sets the closing.
            /// </summary>
            /// <value>The closing.</value>
            public Task Closing { get; set; }

        }



      
        /// <summary>
        /// Interface ICommandModel
        /// </summary>
        /// <typeparam name="TCommand">The type of the t command.</typeparam>
        /// <typeparam name="TResource">The type of the t resource.</typeparam>
        public interface ICommandModel<TCommand, TResource> : ICommand
        {
            /// <summary>
            /// Gets the command core.
            /// </summary>
            /// <value>The command core.</value>
            TCommand CommandCore { get; }
            /// <summary>
            /// Gets or sets a value indicating whether [last can execute value].
            /// </summary>
            /// <value><c>true</c> if [last can execute value]; otherwise, <c>false</c>.</value>
            bool LastCanExecuteValue { get; set; }
            /// <summary>
            /// Gets or sets the resource.
            /// </summary>
            /// <value>The resource.</value>
            TResource State { get; set; }
        }




    
    }

}
