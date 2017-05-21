// ***********************************************************************
// Assembly         : MVVMSidekick_Wp8
// Author           : waywa
// Created          : 05-17-2014
//
// Last Modified By : waywa
// Last Modified On : 01-04-2015
// ***********************************************************************
// <copyright file="Services.cs" company="">
//     Copyright ©  2012
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MVVMSidekick.Utilities;
using Microsoft.Practices.Unity;
using MVVMSidekick.Views;
#if NETFX_CORE



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


    namespace Services
    {




#if False

		/// <summary>
		/// Interface IServiceLocator
		/// </summary>
		public interface IServiceLocator
		{
			/// <summary>
			/// Determines whether the specified name has instance.
			/// </summary>
			/// <typeparam name="TService">The type of the t service.</typeparam>
			/// <param name="name">The name.</param>
			/// <returns><c>true</c> if the specified name has instance; otherwise, <c>false</c>.</returns>
			bool HasInstance<TService>(string name = "");
			/// <summary>
			/// Determines whether the specified name is asynchronous.
			/// </summary>
			/// <typeparam name="TService">The type of the t service.</typeparam>
			/// <param name="name">The name.</param>
			/// <returns><c>true</c> if the specified name is asynchronous; otherwise, <c>false</c>.</returns>
			bool IsAsync<TService>(string name = "");
			/// <summary>
			/// Registers the specified instance.
			/// </summary>
			/// <typeparam name="TService">The type of the t service.</typeparam>
			/// <param name="instance">The instance.</param>
			/// <returns>ServiceLocatorEntryStruct&lt;TService&gt;.</returns>
			ServiceLocatorEntryStruct<TService> Register<TService>(TService instance);
			/// <summary>
			/// Registers the specified name.
			/// </summary>
			/// <typeparam name="TService">The type of the t service.</typeparam>
			/// <param name="name">The name.</param>
			/// <param name="instance">The instance.</param>
			/// <returns>ServiceLocatorEntryStruct&lt;TService&gt;.</returns>
			ServiceLocatorEntryStruct<TService> Register<TService>(string name, TService instance);
			/// <summary>
			/// Registers the specified factory.
			/// </summary>
			/// <typeparam name="TService">The type of the t service.</typeparam>
			/// <param name="factory">The factory.</param>
			/// <param name="isAlwaysCreatingNew">if set to <c>true</c> [always new].</param>
			/// <returns>ServiceLocatorEntryStruct&lt;TService&gt;.</returns>
			ServiceLocatorEntryStruct<TService> Register<TService>(Func<object, TService> factory, bool isAlwaysCreatingNew = true);
			/// <summary>
			/// Registers the specified name.
			/// </summary>
			/// <typeparam name="TService">The type of the t service.</typeparam>
			/// <param name="name">The name.</param>
			/// <param name="factory">The factory.</param>
			/// <param name="isAlwaysCreatingNew">if set to <c>true</c> [always new].</param>
			/// <returns>
			/// ServiceLocatorEntryStruct&lt;TService&gt;.
			/// </returns>
			ServiceLocatorEntryStruct<TService> Register<TService>(string name, Func<object, TService> factory, bool isAlwaysCreatingNew = true);
			/// <summary>
			/// Resolves the specified name.
			/// </summary>
			/// <typeparam name="TService">The type of the service.</typeparam>
			/// <param name="name">The name.</param>
			/// <param name="parameter">The parameter.</param>
			/// <returns>
			/// TService.
			/// </returns>
			TService Resolve<TService>(string name = null, object parameter = null);

			/// <summary>
			/// Registers the specified asynchronous factory.
			/// </summary>
			/// <typeparam name="TService">The type of the t service.</typeparam>
			/// <param name="asyncFactory">The asynchronous factory.</param>
			/// <param name="isAlwaysCreatingNew">if set to <c>true</c> [always new].</param>
			/// <returns>ServiceLocatorEntryStruct&lt;TService&gt;.</returns>
			ServiceLocatorEntryStruct<TService> Register<TService>(Func<object, Task<TService>> asyncFactory, bool isAlwaysCreatingNew = true);
			/// <summary>
			/// Registers the specified name.
			/// </summary>
			/// <typeparam name="TService">The type of the t service.</typeparam>
			/// <param name="name">The name.</param>
			/// <param name="asyncFactory">The asynchronous factory.</param>
			/// <param name="isAlwaysCreatingNew">if set to <c>true</c> [always new].</param>
			/// <returns>ServiceLocatorEntryStruct&lt;TService&gt;.</returns>
			ServiceLocatorEntryStruct<TService> Register<TService>(string name, Func<object, Task<TService>> asyncFactory, bool isAlwaysCreatingNew = true);
			/// <summary>
			/// Resolves the asynchronous.
			/// </summary>
			/// <typeparam name="TService"></typeparam>
			/// <param name="name">The name.</param>
			/// <param name="parameter">The parameter.</param>
			/// <returns>Task&lt;TService&gt;.</returns>
			Task<TService> ResolveAsync<TService>(string name = null, object parameter = null);

		}
		/// <summary>
		/// Class ServiceLocatorBase.
		/// </summary>
		/// <typeparam name="TSubClass">The type of the t sub class.</typeparam>
		public class ServiceLocatorBase<TSubClass> : IServiceLocator
			where TSubClass : ServiceLocatorBase<TSubClass>
		{
			/// <summary>
			/// The dispose actions
			/// </summary>
			Dictionary<Type, Action> disposeActions = new Dictionary<Type, Action>();




			/// <summary>
			/// Registers the specified instance.
			/// </summary>
			/// <typeparam name="TService">The type of the service.</typeparam>
			/// <param name="instance">The instance.</param>
			/// <returns>
			/// ServiceLocatorEntryStruct&lt;TService&gt;.
			/// </returns>
			public ServiceLocatorEntryStruct<TService> Register<TService>(TService instance)
			{

				return Register<TService>(null, instance);
			}

			/// <summary>
			/// Registers the specified name.
			/// </summary>
			/// <typeparam name="TService">The type of the service.</typeparam>
			/// <param name="name">The name.</param>
			/// <param name="instance">The instance.</param>
			/// <returns>
			/// ServiceLocatorEntryStruct&lt;TService&gt;.
			/// </returns>
			public ServiceLocatorEntryStruct<TService> Register<TService>(string name, TService instance)
			{
				name = name ?? "";


				if (!disposeActions.ContainsKey(typeof(TService)))
					disposeActions[typeof(TService)] = () => ServiceTypedCache<TService>.dic.Clear();
				return ServiceTypedCache<TService>.dic[name] =
					 new ServiceLocatorEntryStruct<TService>(name)
					 {
						 ServiceInstance = instance,
						 CacheType = CacheType.Instance
					 };
			}

			/// <summary>
			/// Registers the specified factory.
			/// </summary>
			/// <typeparam name="TService">The type of the service.</typeparam>
			/// <param name="factory">The factory.</param>
			/// <param name="isAlwaysCreatingNew">if set to <c>true</c> [always new].</param>
			/// <returns>
			/// ServiceLocatorEntryStruct&lt;TService&gt;.
			/// </returns>
			public ServiceLocatorEntryStruct<TService> Register<TService>(Func<object, TService> factory, bool isAlwaysCreatingNew = true)
			{
				return Register<TService>(null, factory, isAlwaysCreatingNew);
			}

			/// <summary>
			/// Registers the specified name.
			/// </summary>
			/// <typeparam name="TService">The type of the service.</typeparam>
			/// <param name="name">The name.</param>
			/// <param name="factory">The factory.</param>
			/// <param name="isAlwaysCreatingNew">if set to <c>true</c> [always new].</param>
			/// <returns>
			/// ServiceLocatorEntryStruct&lt;TService&gt;.
			/// </returns>
			public ServiceLocatorEntryStruct<TService> Register<TService>(string name, Func<object, TService> factory, bool isAlwaysCreatingNew = true)
			{
				name = name ?? "";
				ServiceLocatorEntryStruct<TService> rval;
				if (isAlwaysCreatingNew)
				{
					ServiceTypedCache<TService>.dic[name] =
					   rval =
				  new ServiceLocatorEntryStruct<TService>(name)
				  {
					  ServiceFactory = factory,
					  CacheType = CacheType.Factory
				  };
				}
				else
				{

					ServiceTypedCache<TService>.dic[name] =
						rval =
					   new ServiceLocatorEntryStruct<TService>(name)
					   {
						   ServiceFactory = factory,
						   CacheType = CacheType.LazyInstance
					   };
				}
				if (!disposeActions.ContainsKey(typeof(TService)))
					disposeActions[typeof(TService)] = () => ServiceTypedCache<TService>.dic.Clear();

				return rval;
			}

			/// <summary>
			/// Resolves the specified name.
			/// </summary>
			/// <typeparam name="TService">The type of the service.</typeparam>
			/// <param name="name">The name.</param>
			/// <param name="parameters">The parameters.</param>
			/// <returns>
			/// TService.
			/// </returns>
			public TService Resolve<TService>(string name = null, object parameters = null)
			{
				name = name ?? "";
				var subdic = ServiceTypedCache<TService>.dic;
				ServiceLocatorEntryStruct<TService> entry = null;
				if (subdic.TryGetValue(name, out entry))
				{
					return entry.GetService(parameters);
				}
				else
					return default(TService);
			}

			/// <summary>
			/// Disposes this instance.
			/// </summary>
			public void Dispose()
			{
				foreach (var act in disposeActions.Values)
				{
					try
					{
						act();
					}
					catch (Exception)
					{

					}

				}
			}

			/// <summary>
			/// Class ServiceTypedCache.
			/// </summary>
			/// <typeparam name="TService">The type of the t service.</typeparam>
			static class ServiceTypedCache<TService>
			{
				/// <summary>
				/// The dic
				/// </summary>
				public static Dictionary<string, ServiceLocatorEntryStruct<TService>> dic
					= new Dictionary<string, ServiceLocatorEntryStruct<TService>>();
			}



			/// <summary>
			/// Determines whether the specified name has instance.
			/// </summary>
			/// <typeparam name="TService">The type of the service.</typeparam>
			/// <param name="name">The name.</param>
			/// <returns>
			///   <c>true</c> if the specified name has instance; otherwise, <c>false</c>.
			/// </returns>
			public bool HasInstance<TService>(string name = "")
			{
				name = name ?? "";
				ServiceLocatorEntryStruct<TService> entry = null;
				if (ServiceTypedCache<TService>.dic.TryGetValue(name, out entry))
				{
					return
					   entry.GetIsValueCreated();

				}
				else
				{
					return false;
				}
			}





			/// <summary>
			/// Determines whether the specified name is asynchronous.
			/// </summary>
			/// <typeparam name="TService">The type of the service.</typeparam>
			/// <param name="name">The name.</param>
			/// <returns>
			///   <c>true</c> if the specified name is asynchronous; otherwise, <c>false</c>.
			/// </returns>
			/// <exception cref="System.ArgumentException">No such key</exception>
			public bool IsAsync<TService>(string name = "")
			{
				name = name ?? "";
				ServiceLocatorEntryStruct<TService> entry = null;
				if (ServiceTypedCache<TService>.dic.TryGetValue(name, out entry))
				{
					return
					   entry.GetIsValueCreated();

				}
				else
				{
					throw new ArgumentException("No such key");
				}
			}

			/// <summary>
			/// Registers the specified asynchronous factory.
			/// </summary>
			/// <typeparam name="TService">The type of the service.</typeparam>
			/// <param name="asyncFactory">The asynchronous factory.</param>
			/// <param name="isAlwaysCreatingNew">if set to <c>true</c> [always new].</param>
			/// <returns>
			/// ServiceLocatorEntryStruct&lt;TService&gt;.
			/// </returns>
			public ServiceLocatorEntryStruct<TService> Register<TService>(Func<object, Task<TService>> asyncFactory, bool isAlwaysCreatingNew = true)
			{
				return Register(null, asyncFactory, isAlwaysCreatingNew);
			}

			/// <summary>
			/// Registers the specified name.
			/// </summary>
			/// <typeparam name="TService"></typeparam>
			/// <param name="name">The name.</param>
			/// <param name="asyncFactory">The asynchronous factory.</param>
			/// <param name="isAlwaysCreatingNew">if set to <c>true</c> [always new].</param>
			/// <returns>ServiceLocatorEntryStruct&lt;TService&gt;.</returns>
			public ServiceLocatorEntryStruct<TService> Register<TService>(string name, Func<object, Task<TService>> asyncFactory, bool isAlwaysCreatingNew = true)
			{
				name = name ?? "";
				ServiceLocatorEntryStruct<TService> rval;
				if (isAlwaysCreatingNew)
				{
					ServiceTypedCache<TService>.dic[name] = rval = new ServiceLocatorEntryStruct<TService>(name)
					{
						CacheType = CacheType.AsyncFactory,
						AsyncServiceFactory = asyncFactory
					};

				}
				else
				{
					ServiceTypedCache<TService>.dic[name] = rval = new ServiceLocatorEntryStruct<TService>(name)
					{
						CacheType = CacheType.AsyncLazyInstance,
						AsyncServiceFactory = asyncFactory
					};

				}
				if (!disposeActions.ContainsKey(typeof(TService)))
					disposeActions[typeof(TService)] = () => ServiceTypedCache<TService>.dic.Clear();
				return rval;
			}


			/// <summary>
			/// resolve as an asynchronous operation.
			/// </summary>
			/// <typeparam name="TService">The type of the t service.</typeparam>
			/// <param name="name">The name.</param>
			/// <param name="parameter">The parameter.</param>
			/// <returns>Task&lt;TService&gt;.</returns>
			public async Task<TService> ResolveAsync<TService>(string name = null, object parameter = null)
			{
				name = name ?? "";
				var subdic = ServiceTypedCache<TService>.dic;
				ServiceLocatorEntryStruct<TService> entry = null;
				if (subdic.TryGetValue(name, out entry))
				{
					return await entry.GetServiceAsync();
				}
				else
					//#if SILVERLIGHT_5||WINDOWS_PHONE_7
					//                    return await T.askEx.FromResult(default(TService));
					//#else
					//                    return await T.ask.FromResult(default(TService));
					//#endif
					return await TaskExHelper.FromResult(default(TService));
			}
		}



	
	
		/// <summary>
		/// Class ServiceLocator. This class cannot be inherited.
		/// </summary>
		public sealed class ServiceLocator : ServiceLocatorBase<ServiceLocator>
		{
			/// <summary>
			/// Initializes static members of the <see cref="ServiceLocator"/> class.
			/// </summary>
			static ServiceLocator()
			{
				Instance = new ServiceLocator();
			}

			/// <summary>
			/// Prevents a default instance of the <see cref="ServiceLocator"/> class from being created.
			/// </summary>
			private ServiceLocator()
			{

			}

			/// <summary>
			/// Gets or sets the instance.
			/// </summary>
			/// <value>The instance.</value>
			public static IServiceLocator Instance { get; set; }
		}
#else


        public class UnityServiceLocator : ServiceLocator
        {



            public UnityServiceLocator() : this(new UnityContainer())
            {
            }


            //public UnityServiceLocator(bool isForUnitTesting) : this(new UnityContainer())
            //{
            //}

            public UnityServiceLocator(IUnityContainer coreContainer)
            {
                UnityContainer = coreContainer;
            }


            public IUnityContainer UnityContainer { get; protected set; }

            public override bool HasInstance<TService>(string name = "")
            {
                return name == null ? UnityContainer.IsRegistered(typeof(TService)) : UnityContainer.IsRegistered(typeof(TService), name);
            }

            public override IServiceLocator Register<TService>(TService instance)
            {
                UnityContainer.RegisterInstance(instance);
                return this;
            }

            public override IServiceLocator Register<TService>(string name, TService instance)
            {
                UnityContainer.RegisterInstance(name, instance);
                return this;

            }

            public override IServiceLocator Register<TFrom, TTo>(string name = "")

            {
                if (name == null)
                {
                    UnityContainer.RegisterType<TFrom, TTo>();

                }
                else
                {
                    UnityContainer.RegisterType<TFrom, TTo>(name);
                }

                return this;
            }

            public override IServiceLocator RegisterAsyncFactory<TService>(Func<object, IServiceLocator, Task<TService>> asyncFactory, bool isAlwaysCreatingNew = true)
            {
                var fac = new ResolveFactory<Task<TService>>(asyncFactory, isAlwaysCreatingNew);
                UnityContainer.RegisterInstance(fac);
                return this;
            }

            public override IServiceLocator RegisterAsyncFactory<TService>(string name, Func<object, IServiceLocator, Task<TService>> asyncFactory, bool isAlwaysCreatingNew = true)
            {
                var fac = new ResolveFactory<Task<TService>>(asyncFactory, isAlwaysCreatingNew);
                UnityContainer.RegisterInstance(name, fac);
                return this;
            }

            public override IServiceLocator RegisterFactory<TService>(string name, Func<object, IServiceLocator, TService> factory, bool isAlwaysCreatingNew = true)
            {
                var fac = new ResolveFactory<TService>(factory, isAlwaysCreatingNew);
                UnityContainer.RegisterInstance(name, fac);
                return this;
            }

            public override TService Resolve<TService>(string name = null)
            {
                return name == null ? UnityContainer.Resolve<TService>() : UnityContainer.Resolve<TService>(name);
            }

            public override async Task<TService> ResolveAsyncFactoryAsync<TService>(string name = null, object parameter = null)
            {
                return await ResolveFactory<Task<TService>>(name);
            }

            public override TService ResolveFactory<TService>(string name = null, object parameter = null)
            {
                var fac = Resolve<ResolveFactory<TService>>(name);
                if (fac != null)
                {
                    return fac.GetInstance(parameter, this);
                }
                else
                {
                    return default(TService);
                }
            }
        }

#endif


    }
}
