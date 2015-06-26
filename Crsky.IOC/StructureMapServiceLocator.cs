using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ServiceLocation;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Configuration.DSL.Expressions;
using StructureMap.Graph;
using StructureMap.Pipeline;
using StructureMap.TypeRules;

namespace Crsky.IoC
{
    /// <summary>
    /// Adapts StructureMap IoC container to the ServiceLocator pattern.
    /// </summary>
    /// <remarks>
    /// The only purpose for adapting StructureMap to a service locator is to ensure the *application*, not the Library
    /// can determine which IoC tool to use.   This means that IoC container specific code should stay out of our domain objects.
    /// Similarly, ServiceLocators should also not live in domain objects (the should primarily *only* be referenced in the library's bootstrapper)
    /// 
    /// The IRegistration support is here so that we can isolate the reference to StructureMap
    /// into this assembly.  Services and other parts of the code that use IoC should wire up
    /// the IoC container through the IRegistration API.  The API is partial, and only covers the
    /// parts of StructureMap's configuration API that we use; but we can flesh it out incrementally
    /// as we need more functionality.
    /// </remarks>
    public class StructureMapServiceLocator : ServiceLocatorImplBase, IRegistration
    {
        private IContainer _container;
        public BootstrapLogger Logger { get; private set; }

        public List<Type> ExplicitRegistrations { get; set; }

        public StructureMapServiceLocator() : this(null)
        {
        }

        public StructureMapServiceLocator(IContainer container)
        {
            _container = container ?? ObjectFactory.Container;
            Logger = new BootstrapLogger(new StringBuilder());
            ExplicitRegistrations = new List<Type>();
        }

        /// <summary>
        /// Registers the current StructureMapServiceLocator with Enterprise Library as the current locator.
        /// </summary>
        public void UseAsDefault()
        {
            ServiceLocator.SetLocatorProvider(() => this);
            Map<IRegistration>(() => this).Singleton();
        }

        /// <summary>
        /// When implemented by inheriting classes, this method will do the actual work of resolving
        /// the requested service instance.
        /// </summary>
        /// <param name="serviceType">Type of instance requested.</param>
        /// <param name="key">Name of registered service you want. May be null.</param>
        /// <returns>
        /// The requested service instance.
        /// </returns>
        protected override object DoGetInstance(Type serviceType, string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return _container.GetInstance(serviceType);
            }
            else
            {
                return _container.GetInstance(serviceType, key);
            }
        }

        /// <summary>
        /// When implemented by inheriting classes, this method will do the actual work of
        /// resolving all the requested service instances.
        /// </summary>
        /// <param name="serviceType">Type of service requested.</param>
        /// <returns>
        /// Sequence of service instance objects.
        /// </returns>
        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            foreach (object obj in _container.GetAllInstances(serviceType))
            {
                yield return obj;
            }
        }

        #region IRegistration support

        private readonly List<Registry> _registries = new List<Registry>(); 

        /// <summary>
        /// Supplies a registry to the container, but does not load it (yet). That should be
        /// done with a call to Load(), because some containers may need this to be wrapped
        /// in a single operation.
        /// </summary>
        /// <param name="registry">
        /// The registry or module containing the bootstrapping info.
        /// </param>
        public void Register(object registry)
        {
            _registries.Add(registry as Registry);
        }

        /// <summary>
        /// Scans an assembly and registers the types within it using the default conventions,
        /// which may vary by container. After this is called, Load() should be called to 
        /// actually wire up the container.
        /// </summary>
        /// <typeparam name="TContainedType">
        /// A type defined in the assembly. This method will use the type to look up the containing assembly.
        /// </typeparam>
        public void ScanAssembly<TContainedType>()
        {
            var registry = new Registry();

            registry.Scan(with => {
                with.AssemblyContainingType<TContainedType>();
                with.WithDefaultConventions();
                with.RegisterConcreteTypesAgainstTheFirstInterface();
            });

            Register(registry);
        }

        /// <summary>
        /// Scans an assembly and registers the types within it by associating concrete types with the 
        /// open generic types that they close. After this is called, Load() should be called to 
        /// actually wire up the container.
        /// </summary>
        /// <typeparam name="TContainedType">
        /// A type defined in the assembly. This method will use the type to look up the containing assembly.
        /// </typeparam>
        /// <param name="openGenericType">
        /// The open generic type closed by one or more concrete types in the assembly.
        /// </param>
        public void ScanAssemblyAndConnectImplementationsToTypesClosing<TContainedType>(Type openGenericType)
        {
            var registry = new Registry();

            registry.Scan(with =>
            {
                with.AssemblyContainingType<TContainedType>();
                with.With(new ConnectImplementationsToTypesClosing(openGenericType, ExplicitRegistrations));
            });
            Register(registry);
        }
        class ConnectImplementationsToTypesClosing : ConfigurableRegistrationConvention
        {
            List<Type> ExcludedInterfaces { get; set; }
            private readonly Type _openType;

            public ConnectImplementationsToTypesClosing(Type openType, List<Type> excludedInterfaces)
            {
                ExcludedInterfaces = excludedInterfaces;
                _openType = openType;

                if (!_openType.IsOpenGeneric())
                {
                    throw new ApplicationException("This scanning convention can only be used with open generic types");
                }
            }

            public override void Process(Type type, Registry registry)
            {
                var interfaceTypes = type.FindInterfacesThatClose(_openType);
                foreach (var interfaceType in interfaceTypes)
                {
                    if (ExcludedInterfaces !=null && ExcludedInterfaces.Contains(interfaceType))
                        continue;
                    var family = registry.For(interfaceType);
                    ConfigureFamily(family);
                    family.Add(type);
                }
            }
        }

        /// <summary>
        /// Registers a specific type as being the implementation type for a given interface type
        /// (which need not necessarily be an interface).  After this is called, Load() should be
        /// called to actually wire up the container.
        /// </summary>
        /// <typeparam name="TInterface">The type to be requested from the container.</typeparam>
        /// <typeparam name="TImplementation">The type to be supplied by the container in response.</typeparam>
        public IMapping<TInterface, TImplementation> MapExplicitly<TInterface, TImplementation>() where TImplementation : TInterface
        {
            var registry = new Registry();
            var mapping = new StructureMapMapping<TInterface, TImplementation>(registry);
            Register(registry);
            try
            {
                if (ExplicitRegistrations.Contains(typeof (TInterface)))
                {
                    throw new Exception();
                }
                ExplicitRegistrations.Add(typeof (TInterface));
            }
            catch(Exception ex)
            {
                Logger.Error(ex,"The type {0} was already explicitly mapped",typeof(TInterface).FullName);
            }
            return mapping;
        }

        /// <summary>
        /// Registers a specific type as being the implementation type for a given interface type
        /// (which need not necessarily be an interface).  After this is called, Load() should be
        /// called to actually wire up the container.
        /// </summary>
        /// <typeparam name="TInterface">The type to be requested from the container.</typeparam>
        /// <typeparam name="TImplementation">The type to be supplied by the container in response.</typeparam>
        public IMapping<TInterface, TImplementation> Map<TInterface, TImplementation>() where TImplementation : TInterface
        {
            var registry = new Registry();
            var mapping = new StructureMapMapping<TInterface, TImplementation>(registry);
            Register(registry);
            return mapping;
        }

        /// <summary>
        /// Registers a function to return an instance of a given interface type (which need not 
        /// necessarily be an interface).  After this is called, Load() should be called to 
        /// actually wire up the container.
        /// </summary>
        /// <typeparam name="TInterface">The type to be requested from the container.</typeparam>
        /// <param name="function">A function that returns an instance of the specified type.</param>
        public IMapping<TInterface, TInterface> Map<TInterface>(Func<TInterface> function)
        {
            var registry = new Registry();
            var mapping = new StructureMapMapping<TInterface, TInterface>(registry, function);
            Register(registry);
            return mapping;
        }

        /// <summary>
        /// Registers a function to return an instance of a given interface type (which need not 
        /// necessarily be an interface).  After this is called, Load() should be called to 
        /// actually wire up the container.
        /// </summary>
        /// <typeparam name="TInterface">The type to be requested from the container.</typeparam>
        /// <param name="function">A function that returns an instance of the specified type.</param>
        public IMapping<TInterface, TInterface> Map<TInterface>(Func<IServiceLocator, TInterface> function)
        {
            var registry = new Registry();
            var mapping = new StructureMapMapping<TInterface, TInterface>(registry, function);
            Register(registry);
            return mapping;
        }

        /// <summary>
        /// Adds a dependency after the container has been loaded.  Do not use this unless you absolutely have to 
        /// add a dependency after calling .Load
        /// Seriously - don't
        /// </summary>
        /// <typeparam name="TInterface">The dependency type to map</typeparam>
        /// <param name="instance">The instance to map to</param>
        public void Inject<TInterface>(TInterface instance)
        {
            _container.Inject(instance);
        }

        /// <summary>
        /// Removes a dependency instance from the container after the container has been loaded.  Do not use this 
        /// unless you absolutely have to.
        /// </summary>
        /// <typeparam name="TInterface">The dependency type to eject</typeparam>
        public void Eject<TInterface>() 
        {
            _container.EjectAllInstancesOf<TInterface>();
        }

        /// <summary>
        /// Forwards an assembly reference from TInterface to TImplimentation
        /// </summary>
        /// <typeparam name="TInterface">From</typeparam>
        /// <typeparam name="TInterface2">To</typeparam>
        /// <returns>Fowarding policy</returns>
        public void Forward<TInterface, TInterface2>()
            where TInterface : class
            where TInterface2 : class
        {
            var registry = new Registry();
            registry.Forward<TInterface, TInterface2>();
            Register(registry);
        }

        /// <summary>
        /// Causes the container to load the registry objects registered so far (by previous
        /// calls to Register()), then clear its list of pending registries.
        /// </summary>
        public void Load()
        {
            ObjectFactory.Initialize(
                factory =>
                {
                    foreach (var registry in _registries)
                    {
                        try
                        {
                            factory.AddRegistry(registry);
                        }
                        catch (Exception ex)
                        {
                            Logger.Error(ex, "Error while loading registry");
                            throw;
                        }
                    }
                }
            );
            Logger.Log(ObjectFactory.WhatDoIHave());

            _container = ObjectFactory.Container;
            _registries.Clear();
        }

        public IEnumerable<Type> GetAllImplementingRegisteredTypesOf<T>(Func<Type, bool> filter)
        {
            return _container
                .Model
                .PluginTypes
                .Where(x => typeof(T).IsAssignableFrom(x.PluginType))
                .SelectMany(i => i.Instances.Select(d=>d.ConcreteType))
                .Where(filter)
                .Distinct()
                .ToList();
        }

        #endregion
    }

    /// <summary>
    /// Provides a fluent-style interface for detailed configuration of type mappings.
    /// </summary>
    public class StructureMapMapping<TInterface, TImplementation> : IMapping<TInterface, TImplementation> where TImplementation : TInterface
    {
        private CreatePluginFamilyExpression<TInterface> _for;
        private SmartInstance<TImplementation> _mapping;

        public StructureMapMapping(Registry registry)
        {
            _for = registry.For<TInterface>();
            _mapping = _for.Use<TImplementation>();
        } 

        public StructureMapMapping(Registry registry, Func<TInterface> function)
        {
            _for = registry.For<TInterface>();
            _for.Use(function);
        }

        public StructureMapMapping(Registry registry, Func<IServiceLocator, TInterface> function)
        {
            _for = registry.For<TInterface>();
            _for.Use(() => function(ServiceLocator.Current));
        } 

        /// <summary>
        /// Configures the container with specific information for a particular constructor argument.
        /// </summary>
        /// <typeparam name="TArg">The argument type.</typeparam>
        /// <param name="argName">The argument name.</param>
        /// <returns>An object with methods to further specify what behavior to use for the argument.</returns>
        public IConstructorArgumentMapping<TInterface, TImplementation, TArg> WithConstructorArgument<TArg>(string argName)
        {
            if (_mapping == null)
            {
                throw new InvalidOperationException("Constructor arguments may not be specified when a function is used to instantiate objects.");
            }

            return new StructureMapConstructorArgumentMapping<TInterface, TImplementation, TArg>(this, _mapping, argName);
        }

        /// <summary>
        /// Configures the container with specific information for a particular constructor argument.
        /// </summary>
        /// <typeparam name="TArg">The argument type.</typeparam>
        /// <returns>An object with methods to further specify what behavior to use for the argument.</returns>
        public IConstructorArgumentMapping<TInterface, TImplementation, TArg> WithConstructorArgument<TArg>()
        {
            if (_mapping == null)
            {
                throw new InvalidOperationException("Constructor arguments may not be specified when a function is used to instantiate objects.");
            }

            return new StructureMapConstructorArgumentMapping<TInterface, TImplementation, TArg>(this, _mapping);
        }

        /// <summary>
        /// Tells the container that it should return one unique instance per HTTP context.
        /// </summary>
        public StructureMapMapping<TInterface, TImplementation> HttpContextOrThreadLocalScoped()
        {
            _for.HybridHttpOrThreadLocalScoped();
            return this;
        }

        /// <summary>
        /// One instance per thread - be careful if using in ASP.NET (mutliple users will get the same object)
        /// </summary>
        /// <returns></returns>
        public StructureMapMapping<TInterface, TImplementation> ThreadLocalScoped()
        {
            _for.LifecycleIs(new ThreadLocalStorageLifecycle());
            return this;
        }

        /// <summary>
        /// Tells the container that it should create one instance of the specified type, and return it
        /// for all requests.
        /// </summary>
        public StructureMapMapping<TInterface, TImplementation> Singleton()
        {
            _for.Singleton();
            return this;
        }

        public StructureMapMapping<TInterface, TImplementation> DecorateWith<TDecorator>(Func<IServiceLocator,TInterface,TDecorator> function) where  TDecorator : TInterface
        {
            _for.EnrichAllWith((original)=> function(ServiceLocator.Current,original));
            return this;
        }
    }

    /// <summary>
    /// Provides a fluent-style interface for configuration of a constructor argument mapping.
    /// </summary>
    public class StructureMapConstructorArgumentMapping<TInterface, TImplementation, TArg> : IConstructorArgumentMapping<TInterface, TImplementation, TArg> where TImplementation : TInterface
    {
        private readonly SmartInstance<TImplementation>.DependencyExpression<TArg> _argMapping;
        private readonly IMapping<TInterface, TImplementation> _mapping;

        public StructureMapConstructorArgumentMapping(IMapping<TInterface, TImplementation> mapping, SmartInstance<TImplementation> argMapping, string argName)
        {
            _mapping = mapping;
            _argMapping = argMapping.Ctor<TArg>(argName);
        }

        public StructureMapConstructorArgumentMapping(IMapping<TInterface, TImplementation> mapping, SmartInstance<TImplementation> argMapping)
        {
            _mapping = mapping;
            _argMapping = argMapping.Ctor<TArg>();
        } 

        /// <summary>
        /// Tells the container to set the constructor argument to the specified app setting.
        /// </summary>
        /// <param name="appSettingName">The app setting name.</param>
        /// <returns>The current object (to support method chaining).</returns>
        public IConstructorArgumentMapping<TInterface, TImplementation, TArg> EqualToAppSetting(string appSettingName)
        {
            _argMapping.EqualToAppSetting(appSettingName);
            return this;
        }
        
        /// <summary>
        /// Tells the container to set the constructor argument to the specified object.
        /// </summary>
        /// <param name="value">Value to use</param>
        /// <returns>The current object (to support method chaining).</returns>
        public IMapping<TInterface, TImplementation> Is(TArg value)
        {
            _argMapping.Is(x=> x.IsThis(value));
            return _mapping;
        }

        /// <summary>
        /// Tells the container to use the specified type and lambda to supply the constructor argument.
        /// </summary>
        /// <param name="providerFunc">A lambda that should provide the constructor argument</param>
        /// <returns>The current object (to support method chaining).</returns>
        public IMapping<TInterface, TImplementation> Is<TProvider>(Func<TProvider, TArg> providerFunc)
        {
            _argMapping.Is(
                context => 
                {
                    var provider = context.GetInstance<TProvider>();
                    return providerFunc(provider);
                }
            );

            return _mapping;
        }
    }
}
