﻿// ***********************************************************************
// Assembly         : MVVMSidekick_Wp8
// Author           : waywa
// Created          : 05-17-2014
//
// Last Modified By : waywa
// Last Modified On : 01-04-2015
// ***********************************************************************
// <copyright file="Commands.cs" company="">
//     Copyright ©  2012
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Windows.Input;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Utilities;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.CompilerServices;
#if NETFX_CORE
using Windows.UI.Xaml;

#elif WPF
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections.Concurrent;
using System.Windows.Navigation;

using System.Windows.Controls.Primitives;

#elif SILVERLIGHT_5 || SILVERLIGHT_4
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
using Microsoft.Runtime.CompilerServices;

#elif WINDOWS_PHONE_8 || WINDOWS_PHONE_7
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
#endif






namespace MVVMSidekick
{

    namespace Commands
    {





        namespace EventBinding
        {






            /// <summary>
            /// Class CommandBinding.
            /// </summary>
            public class CommandBinding : FrameworkElement

            {

                /// <summary>
                /// Initializes a new instance of the <see cref="CommandBinding"/> class.
                /// </summary>
                public CommandBinding()
                {
                    base.Width = 0;
                    base.Height = 0;
                    base.Visibility = Visibility.Collapsed;
                }








                /// <summary>
                /// Gets or sets the name of the event.
                /// </summary>
                /// <value>The name of the event.</value>
                public string EventName { get; set; }


                /// <summary>
                /// Gets or sets the event source.
                /// </summary>
                /// <value>The event source.</value>
                public FrameworkElement EventSource
                {
                    get { return (FrameworkElement)GetValue(EventSourceProperty); }
                    set { SetValue(EventSourceProperty, value); }
                }

                // Using a DependencyProperty as the backing store for EventSource.  This enables animation, styling, binding, etc...
                /// <summary>
                /// The event source property
                /// </summary>
                public static readonly DependencyProperty EventSourceProperty =
                    DependencyProperty.Register("EventSource", typeof(FrameworkElement), typeof(CommandBinding), new PropertyMetadata(null,
                        (dobj, arg) =>
                        {
                            CommandBinding obj = dobj as CommandBinding;
                            if (obj == null)
                            {
                                return;
                            }
                            if (obj.oldEventDispose != null)
                            {
                                obj.oldEventDispose.Dispose();
                            }
                            var nv = arg.NewValue;
                            if (nv != null)
                            {

                                obj.oldEventDispose = nv.BindEvent(
                                    obj.EventName,
                                    (o, a, en, ehType) =>
                                    {
                                        obj.ExecuteFromEvent(o, a, en, ehType);
                                    });

                            }

                        }


                        ));


                /// <summary>
                /// The old event dispose
                /// </summary>
                IDisposable oldEventDispose;

                /// <summary>
                /// Gets or sets the command.
                /// </summary>
                /// <value>The command.</value>
                public ICommand Command
                {
                    get { return (ICommand)GetValue(CommandProperty); }
                    set { SetValue(CommandProperty, value); }
                }

                // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
                /// <summary>
                /// The command property
                /// </summary>
                public static readonly DependencyProperty CommandProperty =
                    DependencyProperty.Register("Command", typeof(ICommand), typeof(CommandBinding), new PropertyMetadata(null));




                /// <summary>
                /// Gets or sets the parameter.
                /// </summary>
                /// <value>The parameter.</value>
                public Object Parameter
                {
                    get { return (Object)GetValue(ParameterProperty); }
                    set { SetValue(ParameterProperty, value); }
                }

                // Using a DependencyProperty as the backing store for Parameter.  This enables animation, styling, binding, etc...
                /// <summary>
                /// The parameter property
                /// </summary>
                public static readonly DependencyProperty ParameterProperty =
                    DependencyProperty.Register("Parameter", typeof(Object), typeof(CommandBinding), new PropertyMetadata(null));



                /// <summary>
                /// Executes from event.
                /// </summary>
                /// <param name="sender">The sender.</param>
                /// <param name="eventArgs">The event arguments.</param>
                /// <param name="eventName">Name of the event.</param>
                /// <param name="eventHandlerType">Type of the event handler.</param>
                public void ExecuteFromEvent(object sender, object eventArgs, string eventName, Type eventHandlerType)
                {
                    object vm = null;

                    var s = (sender as FrameworkElement);

                    if (Command == null)
                    {
                        return;
                    }
                    var cvm = Command as ICommandWithViewModel;
                    if (cvm != null)
                    {
                        vm = cvm.ViewModel;
                    }


                    var newe = EventCommandEventArgs.Create(Parameter, vm, sender, eventArgs, eventName, eventHandlerType);

                    if (Command.CanExecute(newe))
                    {
                        var spEventCommand = Command as EventCommandBase;
                        if (spEventCommand == null)
                        {
                            Command.Execute(newe);
                        }
                        else
                        {
                            spEventCommand.OnCommandExecute(newe);

                        }
                    }

                }



            }












        }

    }
}