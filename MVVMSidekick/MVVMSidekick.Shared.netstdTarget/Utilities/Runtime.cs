using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MVVMSidekick.Utilities
{
    /// <summary>
    /// Class Runtime.
    /// </summary>
    public static class Runtime
    {

        /// <summary>
        /// The _ is in design mode
        /// </summary>
        static bool? _IsInDesignMode;


        /// <summary>
        /// <para>Gets if the code is running in design time. </para>
        /// <para>读取目前是否在设计时状态。</para>
        /// </summary>
        /// <value><c>true</c> if this instance is in design mode; otherwise, <c>false</c>.</value>
        public static bool IsInDesignMode
        {
            get
            {

                return (
                    _IsInDesignMode
                    ??
                    (

                        _IsInDesignMode =
#if  NETFX_CORE
 Windows.ApplicationModel.DesignMode.DesignModeEnabled
#elif NETSTANDARD
                      true  //TODO:NEED Set To False in bootstraper if is in a NETSTANDARD class lib
#else
 (bool)System.ComponentModel.DependencyPropertyDescriptor
                            .FromProperty(
                                DesignerProperties.IsInDesignModeProperty,
                                typeof(System.Windows.FrameworkElement))
                            .Metadata
                            .DefaultValue
#endif
))
                    .Value;
            }
#if NETSTANDARD
            set { _IsInDesignMode = value; }

#endif
        }

    }



}
