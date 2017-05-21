using MVVMSidekick.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MVVMSidekick.Services
{

    /// <summary>
    /// Interface ITypeSpecifiedServiceLocator
    /// </summary>
    /// <typeparam name="TService">The type of the t service.</typeparam>
    public interface ITypeSpecifiedServiceLocator<TService>
    {
        /// <summary>
        /// Determines whether the specified name has instance.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns><c>true</c> if the specified name has instance; otherwise, <c>false</c>.</returns>
        bool HasInstance(string name = "");
        /// <summary>
        /// Determines whether the specified name is asynchronous.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns><c>true</c> if the specified name is asynchronous; otherwise, <c>false</c>.</returns>
        bool IsAsync(string name = "");
        /// <summary>
        /// Registers the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>ServiceLocatorEntryStruct&lt;TService&gt;.</returns>
        ServiceLocatorEntryStruct<TService> Register(TService instance);
        /// <summary>
        /// Registers the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="instance">The instance.</param>
        /// <returns>ServiceLocatorEntryStruct&lt;TService&gt;.</returns>
        ServiceLocatorEntryStruct<TService> Register(string name, TService instance);
        /// <summary>
        /// Registers the specified factory.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="isAlwaysCreatingNew">if set to <c>true</c> [always new].</param>
        /// <returns>ServiceLocatorEntryStruct&lt;TService&gt;.</returns>
        ServiceLocatorEntryStruct<TService> Register(Func<object, TService> factory, bool isAlwaysCreatingNew = true);
        /// <summary>
        /// Registers the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="factory">The factory.</param>
        /// <param name="isAlwaysCreatingNew">if set to <c>true</c> [always new].</param>
        /// <returns>ServiceLocatorEntryStruct&lt;TService&gt;.</returns>
        ServiceLocatorEntryStruct<TService> Register(string name, Func<object, TService> factory, bool isAlwaysCreatingNew = true);
        /// <summary>
        /// Resolves the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns>TService.</returns>
        TService Resolve(string name = null, object parameter = null);
        /// <summary>
        /// Registers the specified asynchronous factory.
        /// </summary>
        /// <param name="asyncFactory">The asynchronous factory.</param>
        /// <param name="isAlwaysCreatingNew">if set to <c>true</c> [always new].</param>
        /// <returns>ServiceLocatorEntryStruct&lt;TService&gt;.</returns>
        ServiceLocatorEntryStruct<TService> Register(Func<object, Task<TService>> asyncFactory, bool isAlwaysCreatingNew = true);
        /// <summary>
        /// Registers the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="asyncFactory">The asynchronous factory.</param>
        /// <param name="isAlwaysCreatingNew">if set to <c>true</c> [always new].</param>
        /// <returns>ServiceLocatorEntryStruct&lt;TService&gt;.</returns>
        ServiceLocatorEntryStruct<TService> Register(string name, Func<object, Task<TService>> asyncFactory, bool isAlwaysCreatingNew = true);
        /// <summary>
        /// Resolves the asynchronous.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns>Task&lt;TService&gt;.</returns>
        Task<TService> ResolveAsync(string name = null, object parameter = null);
    }

    /// <summary>
    /// Class ServiceLocatorEntryStruct.
    /// </summary>
    /// <typeparam name="TService">The type of the t service.</typeparam>
    public class ServiceLocatorEntryStruct<TService>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLocatorEntryStruct{TService}"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public ServiceLocatorEntryStruct(string name)
        {
            Name = name;
        }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the type of the cache.
        /// </summary>
        /// <value>The type of the cache.</value>
        public CacheType CacheType { get; set; }
        /// <summary>
        /// Gets or sets the service instance.
        /// </summary>
        /// <value>The service instance.</value>
        public TService ServiceInstance { private get; set; }
        /// <summary>
        /// Gets or sets the service factory.
        /// </summary>
        /// <value>The service factory.</value>
        public Func<object, TService> ServiceFactory { private get; set; }
        /// <summary>
        /// Gets or sets the asynchronous service factory.
        /// </summary>
        /// <value>The asynchronous service factory.</value>
        public Func<object, Task<TService>> AsyncServiceFactory { private get; set; }
        /// <summary>
        /// Gets the is value created.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool GetIsValueCreated()
        {

            return (ServiceInstance != null && (!ServiceInstance.Equals(default(TService))));

        }

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>TService.</returns>
        /// <exception cref="System.ArgumentException">No such value supported in enum  + typeof(CacheType).ToString()</exception>
        public TService GetService(object parameter = null)
        {
            switch (CacheType)
            {
                case CacheType.Instance:
                    return ServiceInstance;
                case CacheType.Factory:
                    return ServiceFactory(parameter);
                case CacheType.LazyInstance:
                    var rval = ServiceInstance;
                    if (rval == null || rval.Equals(default(TService)))
                    {
                        lock (this)
                        {
                            if (ServiceInstance == null || ServiceInstance.Equals(default(TService)))
                            {
                                return ServiceInstance = ServiceFactory(parameter);
                            }
                        }
                    }
                    return rval;
                case CacheType.AsyncFactory:            //  not really suguessed to acces async factory in sync method cos may lead to deadlock ,
                case CacheType.AsyncLazyInstance:       // but still can do.
                    Task<TService> t = GetServiceAsync(parameter);
                    return t.Result;
                default:
                    throw new ArgumentException("No such value supported in enum " + typeof(CacheType).ToString());
            }
        }

        /// <summary>
        /// The _ not finished service task
        /// </summary>
        Task<TService> _NotFinishedServiceTask;

        /// <summary>
        /// get service as an asynchronous operation.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>Task&lt;TService&gt;.</returns>
        public async Task<TService> GetServiceAsync(object parameter = null)
        {
            switch (CacheType)
            {
                case CacheType.AsyncFactory:
                    return await AsyncServiceFactory(parameter);
                case CacheType.AsyncLazyInstance:
                    if (GetIsValueCreated())
                    {
                        return ServiceInstance;
                    }
                    else
                    {
                        TService rval;
                        Task<TService> rawait;
                        lock (this)
                        {
                            if (GetIsValueCreated())
                            {
                                return ServiceInstance;
                            }

                            rawait = _NotFinishedServiceTask;
                            if (rawait == null)
                            {
                                rawait = _NotFinishedServiceTask = AsyncServiceFactory(parameter);
                            }

                        }

                        rval = await rawait;
                        lock (this)
                        {
                            ServiceInstance = rval;
                            _NotFinishedServiceTask = null;
                        }

                        return rval;
                    }


                default:
#if SILVERLIGHT_5 || WINDOWS_PHONE_7
						return GetService(parameter);


#else
                    return GetService(parameter);
#endif

            }

        }
    }

    /// <summary>
    /// Class TypeSpecifiedServiceLocatorBase.
    /// </summary>
    /// <typeparam name="TSubClass">The type of the t sub class.</typeparam>
    /// <typeparam name="TService">The type of the t service.</typeparam>
    public class TypeSpecifiedServiceLocatorBase<TSubClass, TService> : ITypeSpecifiedServiceLocator<TService>
        where TSubClass : TypeSpecifiedServiceLocatorBase<TSubClass, TService>
    {
        /// <summary>
        /// Registers the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>ServiceLocatorEntryStruct&lt;TService&gt;.</returns>
        public ServiceLocatorEntryStruct<TService> Register(TService instance)
        {

            return Register(null, instance);
        }

        /// <summary>
        /// Registers the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="instance">The instance.</param>
        /// <returns>ServiceLocatorEntryStruct&lt;TService&gt;.</returns>
        public ServiceLocatorEntryStruct<TService> Register(string name, TService instance)
        {
            name = name ?? "";
            return dic[name] =
                    new ServiceLocatorEntryStruct<TService>(name)
                    {
                        CacheType = CacheType.Instance,
                        ServiceInstance = instance,
                    };
        }

        /// <summary>
        /// Registers the specified factory.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="isAlwaysCreatingNew">if set to <c>true</c> [always new].</param>
        /// <returns>ServiceLocatorEntryStruct&lt;TService&gt;.</returns>
        public ServiceLocatorEntryStruct<TService> Register(Func<object, TService> factory, bool isAlwaysCreatingNew = true)
        {
            return Register(null, factory, isAlwaysCreatingNew);
        }

        /// <summary>
        /// Registers the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="factory">The factory.</param>
        /// <param name="isAlwaysCreatingNew">if set to <c>true</c> [always new].</param>
        /// <returns>ServiceLocatorEntryStruct&lt;TService&gt;.</returns>
        public ServiceLocatorEntryStruct<TService> Register(string name, Func<object, TService> factory, bool isAlwaysCreatingNew = true)
        {
            name = name ?? "";

            if (isAlwaysCreatingNew)
            {
                return dic[name] = new ServiceLocatorEntryStruct<TService>(name)
                {
                    CacheType = CacheType.Factory,
                    ServiceFactory = factory
                };

            }
            else
            {

                return dic[name] = new ServiceLocatorEntryStruct<TService>(name)
                {
                    CacheType = CacheType.LazyInstance,
                    ServiceFactory = factory
                };

            }


        }

        /// <summary>
        /// Resolves the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>TService.</returns>
        public TService Resolve(string name = null, object parameters = null)
        {
            name = name ?? "";
            var subdic = dic;
            ServiceLocatorEntryStruct<TService> entry = null;
            if (subdic.TryGetValue(name, out entry))
            {
                return entry.GetService(parameters);
            }
            else
                return default(TService);
        }




        /// <summary>
        /// The dic
        /// </summary>
        static Dictionary<string, ServiceLocatorEntryStruct<TService>> dic
           = new Dictionary<string, ServiceLocatorEntryStruct<TService>>();




        /// <summary>
        /// Determines whether the specified name has instance.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns><c>true</c> if the specified name has instance; otherwise, <c>false</c>.</returns>
        public bool HasInstance(string name = "")
        {
            name = name ?? "";
            ServiceLocatorEntryStruct<TService> entry = null;
            if (dic.TryGetValue(name, out entry))
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
        /// Registers the specified asynchronous factory.
        /// </summary>
        /// <param name="asyncFactory">The asynchronous factory.</param>
        /// <param name="isAlwaysCreatingNew">if set to <c>true</c> [always new].</param>
        /// <returns>ServiceLocatorEntryStruct&lt;TService&gt;.</returns>
        public ServiceLocatorEntryStruct<TService> Register(Func<object, Task<TService>> asyncFactory, bool isAlwaysCreatingNew = true)
        {
            return Register(null, asyncFactory, isAlwaysCreatingNew);
        }

        /// <summary>
        /// Registers the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="asyncFactory">The asynchronous factory.</param>
        /// <param name="isAlwaysCreatingNew">if set to <c>true</c> [always new].</param>
        /// <returns>ServiceLocatorEntryStruct&lt;TService&gt;.</returns>
        public ServiceLocatorEntryStruct<TService> Register(string name, Func<object, Task<TService>> asyncFactory, bool isAlwaysCreatingNew = true)
        {
            name = name ?? "";

            if (isAlwaysCreatingNew)
            {
                return dic[name] = new ServiceLocatorEntryStruct<TService>(name)
                {
                    CacheType = CacheType.AsyncFactory,
                    AsyncServiceFactory = asyncFactory
                };

            }
            else
            {
                return dic[name] = new ServiceLocatorEntryStruct<TService>(name)
                {
                    CacheType = CacheType.AsyncLazyInstance,
                    AsyncServiceFactory = asyncFactory
                };

            }

        }

        /// <summary>
        /// resolve as an asynchronous operation.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns>Task&lt;TService&gt;.</returns>
        public async Task<TService> ResolveAsync(string name = null, object parameter = null)
        {
            name = name ?? "";
            var subdic = dic;
            ServiceLocatorEntryStruct<TService> entry = null;
            if (subdic.TryGetValue(name, out entry))
            {
                return await entry.GetServiceAsync();
            }
            else

                return await TaskExHelper.FromResult(default(TService));

        }


        /// <summary>
        /// Determines whether the specified name is asynchronous.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns><c>true</c> if the specified name is asynchronous; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentException">No such key</exception>
        public bool IsAsync(string name = "")
        {
            name = name ?? "";
            ServiceLocatorEntryStruct<TService> entry = null;
            if (dic.TryGetValue(name, out entry))
            {
                return
                   entry.GetIsValueCreated();

            }
            else
            {
                throw new ArgumentException("No such key");
            }
        }
    }

    /// <summary>
    /// Enum CacheType
    /// </summary>
    public enum CacheType
    {
        /// <summary>
        /// The instance
        /// </summary>
        Instance,
        /// <summary>
        /// The factory
        /// </summary>
        Factory,
        /// <summary>
        /// The lazy instance
        /// </summary>
        LazyInstance,
        /// <summary>
        /// The asynchronous factory
        /// </summary>
        AsyncFactory,
        /// <summary>
        /// The asynchronous lazy instance
        /// </summary>
        AsyncLazyInstance
    }

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
        /// Registers the typeMapping
        /// </summary>
        IServiceLocator Register<TFrom, TTo>(string name = "")
            where TTo : TFrom;

        /// <summary>
        /// Registers the specified instance.
        /// </summary>
        /// <typeparam name="TService">The type of the t service.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns>ServiceLocatorEntryStruct&lt;TService&gt;.</returns>
        IServiceLocator Register<TService>(TService instance);
        /// <summary>
        /// Registers the specified name.
        /// </summary>
        /// <typeparam name="TService">The type of the t service.</typeparam>
        /// <param name="name">The name.</param>
        /// <param name="instance">The instance.</param>
        /// <returns>ServiceLocatorEntryStruct&lt;TService&gt;.</returns>
        IServiceLocator Register<TService>(string name, TService instance);

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
        IServiceLocator RegisterFactory<TService>(string name, Func<object, IServiceLocator, TService> factory, bool isAlwaysCreatingNew = true);
        /// <summary>
        /// Resolves the specified name.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="name">The name.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns>
        /// TService.
        /// </returns>
        TService Resolve<TService>(string name = null);

        /// <summary>
        /// Registers the specified asynchronous factory.
        /// </summary>
        /// <typeparam name="TService">The type of the t service.</typeparam>
        /// <param name="asyncFactory">The asynchronous factory.</param>
        /// <param name="isAlwaysCreatingNew">if set to <c>true</c> [always new].</param>
        /// <returns>ServiceLocatorEntryStruct&lt;TService&gt;.</returns>
        IServiceLocator RegisterAsyncFactory<TService>(Func<object, IServiceLocator, Task<TService>> asyncFactory, bool isAlwaysCreatingNew = true);
        /// <summary>
        /// Registers the specified name.
        /// </summary>
        /// <typeparam name="TService">The type of the t service.</typeparam>
        /// <param name="name">The name.</param>
        /// <param name="asyncFactory">The asynchronous factory.</param>
        /// <param name="isAlwaysCreatingNew">if set to <c>true</c> [always new].</param>
        /// <returns>ServiceLocatorEntryStruct&lt;TService&gt;.</returns>
        IServiceLocator RegisterAsyncFactory<TService>(string name, Func<object, IServiceLocator, Task<TService>> asyncFactory, bool isAlwaysCreatingNew = true);


        /// <summary>
        /// Resolves the asynchronous.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="name">The name.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns>Task&lt;TService&gt;.</returns>
        TService ResolveFactory<TService>(string name = null, object parameter = null);

        /// <summary>
        /// Resolves the asynchronous.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="name">The name.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns>Task&lt;TService&gt;.</returns>
        Task<TService> ResolveAsyncFactoryAsync<TService>(string name = null, object parameter = null);
    }

    public class ResolveFactory<TService>
    {
        public ResolveFactory(Func<object, IServiceLocator, TService> factory, bool isAlwaysCreatingNew)
        {
            IsAlwaysCreatingNew = isAlwaysCreatingNew;
            _factory = factory;
        }

        public bool IsAlwaysCreatingNew { get; private set; }

        private TService _instance;
        private bool _executed;
        private object _lock = new object();
        private Func<object, IServiceLocator, TService> _factory;

        public virtual TService GetInstance(object parameter, IServiceLocator locator)
        {
            if (IsAlwaysCreatingNew)
            {
                return _factory(parameter, locator);
            }
            else
            {
                lock (_lock)
                {
                    if (_executed)
                    {
                        return _instance;
                    }
                    else
                    {
                        _instance = _factory(parameter, locator);
                        _executed = true;
                        return _instance;
                    }
                }
            }
        }


    }


    public static class ServiceLocatorExtensions
    {

        public static TServiceLocator Configure<TServiceLocator>(this TServiceLocator instance, Action<TServiceLocator> configureBody) where TServiceLocator : ServiceLocator
        {
            configureBody(instance);
            return instance;
        }
        public static async Task<TServiceLocator> ConfigureAsync<TServiceLocator>(this Task<TServiceLocator> instanceTask, Action<TServiceLocator> configureBody) where TServiceLocator : ServiceLocator
        {
            var instance = await instanceTask;
            instance.Configure(configureBody);
            return instance;
        }
        public static async Task<TServiceLocator> ConfigureAsync<TServiceLocator>(this TServiceLocator instance, AsyncAction<TServiceLocator> configureBody) where TServiceLocator : ServiceLocator
        {
            await configureBody(instance);
            return instance;
        }
        public static async Task<TServiceLocator> ConfigureAsync<TServiceLocator>(this Task<TServiceLocator> instanceTask, AsyncAction<TServiceLocator> configureBody) where TServiceLocator : ServiceLocator
        {
            var instance = await instanceTask;
            await instance.ConfigureAsync(configureBody);
            return instance;
        }
    }



    public delegate Task AsyncAction<T>(T input);

    public abstract class ServiceLocator : IServiceLocator
    {

        private static Lazy<ServiceLocator> defaultInstance;
        //= new Lazy<ServiceLocator>(() => new UnityServiceLocator(), true);

        public static void SetDefaultInstanceFactory(Func<ServiceLocator> factory)
        {
            defaultInstance = new Lazy<ServiceLocator>(factory, true);
        }


        public static IServiceLocator Instance
        {
            get
            {

                return defaultInstance.Value;
               
            }
        }







        public abstract bool HasInstance<TService>(string name = "");
        public abstract IServiceLocator Register<TService>(TService instance);
        public abstract IServiceLocator Register<TService>(string name, TService instance);
        public abstract IServiceLocator Register<TFrom, TTo>(string name = "")
            where TTo : TFrom;
        public abstract IServiceLocator RegisterAsyncFactory<TService>(Func<object, IServiceLocator, Task<TService>> asyncFactory, bool isAlwaysCreatingNew = true);
        public abstract IServiceLocator RegisterAsyncFactory<TService>(string name, Func<object, IServiceLocator, Task<TService>> asyncFactory, bool isAlwaysCreatingNew = true);
        public abstract IServiceLocator RegisterFactory<TService>(string name, Func<object, IServiceLocator, TService> factory, bool isAlwaysCreatingNew = true);
        public abstract TService Resolve<TService>(string name = null);
        public abstract Task<TService> ResolveAsyncFactoryAsync<TService>(string name = null, object parameter = null);
        public abstract TService ResolveFactory<TService>(string name = null, object parameter = null);
    }

}
