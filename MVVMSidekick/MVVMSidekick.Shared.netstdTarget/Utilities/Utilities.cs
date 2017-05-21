// ***********************************************************************
// Assembly         : MVVMSidekick_Wp8
// Author           : waywa
// Created          : 05-17-2014
//
// Last Modified By : waywa
// Last Modified On : 01-04-2015
// ***********************************************************************
// <copyright file="Utilities.cs" company="">
//     Copyright ©  2012
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.CompilerServices;
using System.Reactive.Linq;
using System.Collections.ObjectModel;
using System.Security;
using System.ComponentModel;
#if NETFX_CORE
using System.Collections.Concurrent;
using Windows.System.Threading;
using System.Reactive.Disposables;
#elif NETSTANDARD
using System.Collections.Concurrent;
using System.Reactive.Disposables;
using System.Threading;
#elif WPF
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections.Concurrent;
using System.Windows.Navigation;

using MVVMSidekick.Views;
using System.Windows.Controls.Primitives;
using MVVMSidekick.Services;
using System.Reactive.Disposables;


#elif SILVERLIGHT_5 || SILVERLIGHT_4
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
using System.Reactive.Disposables;

#elif WINDOWS_PHONE_8 || WINDOWS_PHONE_7
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
using Microsoft.Phone.Reactive;
#endif



namespace MVVMSidekick
{

	namespace Utilities
	{

	




		/// <summary>
		/// Unify Task(4.5) and TaskEx (SL5) method in this helper
		/// </summary>
		public static class TaskExHelper
		{



			/// <summary>
			/// Yields this instance.
			/// </summary>
			/// <returns>Task.</returns>
			public static async Task Yield()
			{
#if SILVERLIGHT_5||WINDOWS_PHONE_7||NET40
				await TaskEx.Yield();

#else
				await Task.Yield();
#endif

			}

			/// <summary>
			/// Froms the result.
			/// </summary>
			/// <typeparam name="T"></typeparam>
			/// <param name="result">The result.</param>
			/// <returns>
			/// Task&lt;T&gt;.
			/// </returns>
			public static async Task<T> FromResult<T>(T result)
			{
#if SILVERLIGHT_5||WINDOWS_PHONE_7||NET40
				return await TaskEx.FromResult(result);

#else
				return await Task.FromResult(result);
#endif

			}

			/// <summary>
			/// Delays the specified ms.
			/// </summary>
			/// <param name="ms">The ms.</param>
			/// <returns>Task.</returns>
			public static async Task Delay(int ms)
			{

#if SILVERLIGHT_5||WINDOWS_PHONE_7||NET40
				await TaskEx.Delay(ms);

#else

				await Task.Delay(ms);
#endif

			}

		}
		/// <summary>
		/// Class TypeInfoHelper.
		/// </summary>
		public static class TypeInfoHelper
		{
#if NETFX_CORE||NETSTANDARD
			/// <summary>
			/// Gets the type or type information.
			/// </summary>
			/// <param name="type">The type.</param>
			/// <returns>TypeInfo.</returns>
			public static TypeInfo GetTypeOrTypeInfo(this Type type)
			{
                return type.GetTypeInfo();

			}
#else
			/// <summary>
			/// Gets the type or type information.
			/// </summary>
			/// <param name="type">The type.</param>
			/// <returns>Type.</returns>
			public static Type GetTypeOrTypeInfo(this Type type)
			{
				return type;

			}
#endif

		}

		/// <summary>
		/// Class ReflectionCache.
		/// </summary>
		public static class ReflectionCache
		{
			/// <summary>
			/// Class ReflectInfoCache.
			/// </summary>
			/// <typeparam name="T"></typeparam>
			static class ReflectInfoCache<T> where T : MemberInfo
			{
				/// <summary>
				/// The cache
				/// </summary>
				static ConcurrentDictionary<Type, Dictionary<string, T>> cache
					= new ConcurrentDictionary<Type, Dictionary<string, T>>();

				/// <summary>
				/// Gets the cache.
				/// </summary>
				/// <param name="type">The type.</param>
				/// <param name="dataGetter">The data getter.</param>
				/// <returns>Dictionary&lt;System.String, T&gt;.</returns>
				static public Dictionary<string, T> GetCache(Type type, Func<Type, T[]> dataGetter)
				{
					return cache.GetOrAdd(type, s => dataGetter(s).ToDictionary(x => x.Name, x => x));
				}
			}


			/// <summary>
			/// Gets the methods from cache.
			/// </summary>
			/// <param name="type">The type.</param>
			/// <returns>Dictionary&lt;System.String, MethodInfo&gt;.</returns>
			public static Dictionary<string, MethodInfo> GetMethodsFromCache(this Type type)
			{
#if NETFX_CORE || NETSTANDARD
                return ReflectInfoCache<MethodInfo>.GetCache(type, x => x.GetRuntimeMethods().ToArray());
#else
				return ReflectInfoCache<MethodInfo>.GetCache(type, x => x.GetMethods());
#endif
			}

			/// <summary>
			/// Gets the events from cache.
			/// </summary>
			/// <param name="type">The type.</param>
			/// <returns>Dictionary&lt;System.String, EventInfo&gt;.</returns>
			public static Dictionary<string, EventInfo> GetEventsFromCache(this Type type)
			{
#if NETFX_CORE || NETSTANDARD
                return ReflectInfoCache<EventInfo>.GetCache(type, x => x.GetRuntimeEvents().ToArray());
#else
				return ReflectInfoCache<EventInfo>.GetCache(type, x => x.GetEvents());
#endif
			}



		}


		/// <summary>
		/// Inveoker of event handler
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="eventArgs">The event arguments.</param>
		/// <param name="eventName">Name of the event.</param>
		/// <param name="eventHandlerType">Type of the event handler.</param>
		public delegate void EventHandlerInvoker(object sender, object eventArgs, string eventName, Type eventHandlerType);
		/// <summary>
		/// Class EventHandlerHelper.
		/// </summary>
		public static class EventHandlerHelper
		{
			/// <summary>
			/// Creates the handler.
			/// </summary>
			/// <param name="bind">The bind.</param>
			/// <param name="eventName">Name of the event.</param>
			/// <param name="delegateType">Type of the delegate.</param>
			/// <param name="eventParametersTypes">The event parameters types.</param>
			/// <returns>Delegate.</returns>
			private static Delegate CreateHandler(
				Expression<EventHandlerInvoker> bind,
				string eventName,
				Type delegateType,
				Type[] eventParametersTypes
			)
			{
				var pars =
						eventParametersTypes
							.Select(
								et => System.Linq.Expressions.Expression.Parameter(et))
						.ToArray();
				var en = System.Linq.Expressions.Expression.Constant(eventName, typeof(string));
				var eht = System.Linq.Expressions.Expression.Constant(delegateType, typeof(Type));


				var expInvoke = System.Linq.Expressions.Expression.Invoke(bind, pars[0], pars[1], en, eht);
				var lambda = System.Linq.Expressions.Expression.Lambda(delegateType, expInvoke, pars);
				var compiled = lambda.Compile();
				return compiled;
			}

			/// <summary>
			/// Binds the event.
			/// </summary>
			/// <param name="sender">The sender.</param>
			/// <param name="eventName">Name of the event.</param>
			/// <param name="executeAction">The execute action.</param>
			/// <returns>IDisposable.</returns>
			public static IDisposable BindEvent(this object sender, string eventName, EventHandlerInvoker executeAction)
		
			{



				var t = sender.GetType();

				while (t != null)
				{

					var es = t.GetEventsFromCache();
					EventInfo ei = es.MatchOrDefault(eventName);


					if (ei != null)
					{

						var handlerType = ei.EventHandlerType;
						var eventMethod = handlerType.GetMethodsFromCache().MatchOrDefault("Invoke");
						if (eventMethod != null)
						{
							var pts = eventMethod.GetParameters().Select(p => p.ParameterType)
								.ToArray();
							var newHandler = CreateHandler(
											   (o, e, en, ehtype) => executeAction(o, e, en, ehtype),
											   eventName,
											   handlerType,
											   pts
											   );

#if NETFX_CORE ||WINDOWS_PHONE_8
							var etmodule = sender.GetType().GetTypeOrTypeInfo().Module;
							try
							{
								return DoNetEventBind(sender, ei, newHandler);
							}
							catch (InvalidOperationException )
							{
								var newMI = WinRTEventBindMethodInfo.MakeGenericMethod(newHandler.GetType());

								var rval = newMI.Invoke(null, new object[] { sender, ei, newHandler }) as IDisposable;


								return rval;
							}


#else

							return DoNetEventBind(sender, ei, newHandler);
#endif


						}

						return null;
					}

					t = t.GetTypeOrTypeInfo().BaseType;
				}

				return null;
			}


#if NETFX_CORE||WINDOWS_PHONE_8
			/// <summary>
			/// The win rt event bind method information
			/// </summary>
			static MethodInfo WinRTEventBindMethodInfo = typeof(EventHandlerHelper).GetTypeInfo().GetDeclaredMethod("WinRTEventBind");
			/// <summary>
			/// Wins the rt event bind.
			/// </summary>
			/// <typeparam name="THandler">The type of the t property.</typeparam>
			/// <param name="sender">The sender.</param>
			/// <param name="ei">The ei.</param>
			/// <param name="handler">The handler.</param>
			/// <returns>IDisposable.</returns>
			private static IDisposable WinRTEventBind<THandler>(object sender, EventInfo ei, object handler)
			{
				System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken tk = default(System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken);

				Action<System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken> remove
					= et =>
					{
						ei.RemoveMethod.Invoke(sender, new object[] { et });
					};

				System.Runtime.InteropServices.WindowsRuntime.WindowsRuntimeMarshal.AddEventHandler<THandler>(
					ev =>
					{
						tk = (System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken)ei.AddMethod.Invoke(sender, new object[] { ev });
						return tk;
					},
					remove,
					(THandler)handler);

				return Disposable.Create(() =>
					{
						System.Runtime.InteropServices.WindowsRuntime.WindowsRuntimeMarshal.RemoveEventHandler<THandler>(
						   remove,
						(THandler)handler);


					}
				);

			}
#endif
			/// <summary>
			/// Does the net event bind.
			/// </summary>
			/// <param name="sender">The sender.</param>
			/// <param name="ei">The ei.</param>
			/// <param name="newHandler">The new handler.</param>
			/// <returns>IDisposable.</returns>
			private static IDisposable DoNetEventBind(object sender, EventInfo ei, Delegate newHandler)
			{
				ei.AddEventHandler(sender, newHandler);
				return Disposable.Create(() => ei.RemoveEventHandler(sender, newHandler));
			}

		}

		/// <summary>
		/// Class ColllectionHelper.
		/// </summary>
		public static class ColllectionHelper
		{


			/// <summary>
			/// To the observable collection.
			/// </summary>
			/// <typeparam name="T"></typeparam>
			/// <param name="items">The items.</param>
			/// <returns>ObservableCollection&lt;T&gt;.</returns>
			public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> items)
			{

				return new ObservableCollection<T>(items);
			}


			/// <summary>
			/// Matches the or default.
			/// </summary>
			/// <typeparam name="TKey">The type of the key.</typeparam>
			/// <typeparam name="TValue">The type of the value.</typeparam>
			/// <param name="dic">The dic.</param>
			/// <param name="key">The key.</param>
			/// <returns>
			/// TValue.
			/// </returns>
			public static TValue MatchOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key)
			{
				TValue val = default(TValue);
				dic.TryGetValue(key, out val);
				return val;
			}
		}

#if WINDOWS_PHONE_7
    public class Lazy<T>
    {
        public Lazy(Func<T> factory)
        { 
            _factory =()=>
            {
                lock(this)
                {
                    if(_value.Equals(default(T)))
                    {
                        _value=_factory();
                 
                    
                    }
                    return _value;
                }
            
            };
        
        }

        T _value;
        Func<T> _factory;
        public T Value 
        { 
            get
            {
               return _value.Equals (default(T))?_factory():_value; 
            }
        
            set
            {
                _value=value;
            } 
        }
    }



#endif

		/// <summary>
		/// Class ExpressionHelper.
		/// </summary>
		public class ExpressionHelper
		{
			/// <summary>
			/// Gets the name of the property.
			/// </summary>
			/// <typeparam name="TSubClassType">The type of the sub class type.</typeparam>
			/// <typeparam name="TProperty">The type of the t property.</typeparam>
			/// <param name="expression">The expression.</param>
			/// <returns>
			/// System.String.
			/// </returns>
			public static string GetPropertyName<TSubClassType, TProperty>(Expression<Func<TSubClassType, TProperty>> expression)
			{
				MemberExpression body = expression.Body as MemberExpression;
				var propName = (body.Member is PropertyInfo) ? body.Member.Name : string.Empty;
				return propName;
			}



			/// <summary>
			/// Gets the name of the property.
			/// </summary>
			/// <typeparam name="TSubClassType">The type of the sub class type.</typeparam>
			/// <param name="expression">The expression.</param>
			/// <returns>
			/// System.String.
			/// </returns>
			/// <exception cref="System.InvalidOperationException">The expression inputed should be like \x=&gt;x.PropertyName\ but currently is not: + expression.ToString()</exception>
			public static string GetPropertyName<TSubClassType>(Expression<Func<TSubClassType, object>> expression)
			{
				MemberExpression body = expression.Body as MemberExpression;
				if (body != null)
				{
					var propName = (body.Member is PropertyInfo) ? body.Member.Name : string.Empty;
					return propName;
				}

				var exp2 = expression.Body as System.Linq.Expressions.UnaryExpression;
				if (exp2 != null)
				{
					body = exp2.Operand as MemberExpression;
					var propName = (body.Member is PropertyInfo) ? body.Member.Name : string.Empty;
					return propName;
				}
				else
				{

					throw new InvalidOperationException("The expression inputed should be like \"x=>x.PropertyName\" but currently is not:" + expression.ToString());
				}

			}





		}

#if SILVERLIGHT_5||WINDOWS_PHONE_8||WINDOWS_PHONE_7
		/// <summary>
		/// Class ConcurrentDictionary.
		/// </summary>
		/// <typeparam name="TK">The type of the t property.</typeparam>
		/// <typeparam name="TV">The type of the tv.</typeparam>
		public class ConcurrentDictionary<TK, TV> : Dictionary<TK, TV>
		{
			/// <summary>
			/// Gets the or add.
			/// </summary>
			/// <param name="key">The key.</param>
			/// <param name="factory">The factory.</param>
			/// <returns>TV.</returns>
			public TV GetOrAdd(TK key, Func<TK, TV> factory)
			{
				TV rval = default(TV);

				if (!base.TryGetValue(key, out rval))
				{
					lock (this)
					{
						if (!base.TryGetValue(key, out rval))
						{
							rval = factory(key);
							base.Add(key, rval);
						}


					}
				}

				return rval;
			}
		}
#endif


	}

}

