using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace chainsharp.core.Services
{
    public class ServiceRegistry
    {
        // a map between contracts -> concrete implementation classes

        #region Private Fields

        private static readonly object _theLock = new SemaphoreSlim(1, 1);
        private static IServiceRegistry _instance;
        private IDictionary<Type, object> _instantiatedServices;
        private IDictionary<Type, Type> _servicesType;

        #endregion Private Fields

        // a map containing references to concrete implementation already instantiated

        #region Private Constructors

        // (the service locator uses lazy instantiation).
        private ServiceRegistry()
        {
            _servicesType = new Dictionary<Type, Type>();
            _instantiatedServices = new Dictionary<Type, object>();

            BuildServiceTypesMap();
        }

        #endregion Private Constructors

        #region Public Properties

        public static IServiceRegistry Instance
        {
            get
            {
                lock (_theLock) // thread safety

                {
                    if (_instance == null)
                    {
                        _instance = (IServiceRegistry)new ServiceRegistry();
                    }
                }

                return _instance;
            }
        }

        #endregion Public Properties

        #region Public Methods

        public T GetService<T>()
        {
            if (_instantiatedServices.ContainsKey(typeof(T)))
            {
                return (T)_instantiatedServices[typeof(T)];
            }
            else
            {
                // lazy initialization

                try
                {
                    // use reflection to invoke the service

                    ConstructorInfo constructor = _servicesType[typeof(T)].GetConstructor(new Type[0]);
                    Debug.Assert(constructor != null, "Cannot find a suitable constructor for " + typeof(T));

                    T service = (T)constructor.Invoke(null);

                    // add the service to the ones that we have already instantiated

                    PopulateStructures(service);

                    return service;
                }
                catch (KeyNotFoundException)
                {
                    throw new ApplicationException("The requested service is not registered");
                }
            }
        }

        public void SetService<T>(T service)
        {
            PopulateStructures(service);
        }

        #endregion Public Methods

        #region Private Methods

        private void BuildServiceTypesMap()
        {
            //servicesType.Add(typeof(IServiceA),
            //    typeof(ServiceA));

            //servicesType.Add(typeof(IServiceB),
            //    typeof(ServiceB));

            //servicesType.Add(typeof(IServiceC),
            //    typeof(ServiceC));
        }

        private void PopulateStructures<T>(T service)
        {
            _instantiatedServices.Add(typeof(T), service);
            _servicesType.Add(typeof(T), service.GetType());
        }

        #endregion Private Methods
    }
}
