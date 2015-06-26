using System;
using System.Collections.Generic;
using Microsoft.Practices.ServiceLocation;

namespace Crsky.IoC
{
    /// <summary>
    /// This interface describes functionality for wiring up an IoC container, but without 
    /// being tied to a specific container implementation.  The goal is to wrap enough of
    /// our chosen IoC container's configuration API that we don't need to reference it 
    /// directly except from this assembly.  As we need additional features from our IoC
    /// container that require new configuration scenarios, we can add that support here.
    /// </summary>
    public interface IRegistration
    {

        BootstrapLogger Logger { get; }
        /// <summary>
        /// Supplies a registry to the container, but does not load it (yet). That should be
        /// done with a call to Load(), because some containers may need this to be wrapped
        /// in a single operation.
        /// </summary>
        /// <param name="registry">
        /// The registry or module containing the bootstrapping info. This must be of the correct
        /// type as required by the container.
        /// </param>
        void Register(object registry);

        /// <summary>
        /// Scans an assembly and registers the types within it using the default conventions,
        /// which may vary by container. After this is called, Load() should be called to 
        /// actually wire up the container.
        /// </summary>
        /// <typeparam name="TContainedType">
        /// A type defined in the assembly. This method will use the type to look up the containing assembly.
        /// </typeparam>
        void ScanAssembly<TContainedType>();

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
        void ScanAssemblyAndConnectImplementationsToTypesClosing<TContainedType>(Type openGenericType);

        /// <summary>
        /// Registers a specific type as being the implementation type for a given interface type
        /// (which need not necessarily be an interface).  After this is called, Load() should be
        /// called to actually wire up the container.
        /// </summary>
        /// <typeparam name="TInterface">The type to be requested from the container.</typeparam>
        /// <typeparam name="TImplementation">The type to be supplied by the container in response.</typeparam>
        IMapping<TInterface, TImplementation> Map<TInterface, TImplementation>() where TImplementation : TInterface;

        /// <summary>
        /// Registers a function to return an instance of a given interface type (which need not 
        /// necessarily be an interface).  After this is called, Load() should be called to 
        /// actually wire up the container.
        /// </summary>
        /// <typeparam name="TInterface">The type to be requested from the container.</typeparam>
        /// <param name="function">A function that returns an instance of the specified type.</param>
        IMapping<TInterface, TInterface> Map<TInterface>(Func<TInterface> function);

        /// <summary>
        /// Registers a function to return an instance of a given interface type (which need not 
        /// necessarily be an interface).  After this is called, Load() should be called to 
        /// actually wire up the container.
        /// </summary>
        /// <typeparam name="TInterface">The type to be requested from the container.</typeparam>
        /// <param name="function">A function that returns an instance of the specified type.</param>
        IMapping<TInterface, TInterface> Map<TInterface>(Func<IServiceLocator, TInterface> function);

        /// <summary>
        /// Adds a dependency after the container has been loaded.  Do not use this unless you absolutely have to 
        /// add a dependency after calling .Load
        /// Seriously - don't
        /// </summary>
        /// <typeparam name="TInterface">The dependency type to map</typeparam>
        /// <param name="instance">The instance to map to</param>
        void Inject<TInterface>(TInterface instance);

        /// <summary>
        /// Removes a dependency instance from the container after the container has been loaded.  Do not use this 
        /// unless you absolutely have to.
        /// </summary>
        /// <typeparam name="TInterface">The dependency type to eject</typeparam>
        void Eject<TInterface>();

        /// <summary>
        /// Causes the container to load the registry objects registered so far (by previous
        /// calls to the other methods in this interface), then clear its list of pending registries.
        /// </summary>
        void Load();

        IEnumerable<Type> GetAllImplementingRegisteredTypesOf<T>(Func<Type, bool> filter);

        /// <summary>
        /// Registers a specific type as being the implementation type for a given interface type
        /// (which need not necessarily be an interface).  After this is called, Load() should be
        /// called to actually wire up the container.
        /// </summary>
        /// <typeparam name="TInterface">The type to be requested from the container.</typeparam>
        /// <typeparam name="TImplementation">The type to be supplied by the container in response.</typeparam>
        IMapping<TInterface, TImplementation> MapExplicitly<TInterface, TImplementation>() where TImplementation : TInterface;
    }

    /// <summary>
    /// Provides a fluent-style interface for detailed configuration of type mappings.
    /// </summary>
    public interface IMapping<TInterface, TImplementation> where TImplementation : TInterface
    {
        /// <summary>
        /// Configures the container with specific information for a particular constructor argument.
        /// </summary>
        /// <typeparam name="TArg">The argument type.</typeparam>
        /// <param name="argName">The argument name.</param>
        /// <returns>An object with methods to further specify what behavior to use for the argument.</returns>
        IConstructorArgumentMapping<TInterface, TImplementation, TArg> WithConstructorArgument<TArg>(string argName);

        /// <summary>
        /// Configures the container with specific information for a particular constructor argument.
        /// </summary>
        /// <typeparam name="TArg">The argument type.</typeparam>
        /// <returns>An object with methods to further specify what behavior to use for the argument.</returns>
        IConstructorArgumentMapping<TInterface, TImplementation, TArg> WithConstructorArgument<TArg>();

        /// <summary>
        /// Tells the container that it should return one unique instance per HTTP context (or use thread local if http is not available).
        /// </summary>
        StructureMapMapping<TInterface, TImplementation> HttpContextOrThreadLocalScoped();

        /// <summary>
        /// WARNING! -- For ASP.NET this object will cross calls - ie, same object for different users.
        /// Tells the container that it should return one unique instance thread.  Only use in http if you desire
        /// a singleton per thread!
        /// </summary>
        StructureMapMapping<TInterface, TImplementation> ThreadLocalScoped();

        /// <summary>
        /// Tells the container that it should create one instance of the specified type, and return it
        /// for all requests.
        /// </summary>
        StructureMapMapping<TInterface, TImplementation> Singleton();

        /// <summary>
        /// Decorates the original implementation witha  different implementation 
        /// </summary>
        /// <typeparam name="TDecorator">The Decorated Type</typeparam>
        /// <param name="function">Function that takes teh original and a service locator and returns instance of the Decorated Class</param>
        /// <returns></returns>
        StructureMapMapping<TInterface, TImplementation> DecorateWith<TDecorator>(
            Func<IServiceLocator, TInterface, TDecorator> function) where TDecorator : TInterface;

    }


    /// <summary>
    /// Provides a fluent-style interface for configuration of a constructor argument mapping.
    /// </summary>
    public interface IConstructorArgumentMapping<TInterface, TImplementation, TArg> where TImplementation : TInterface
    {
        /// <summary>
        /// Tells the container to set the constructor argument to the specified app setting.
        /// </summary>
        /// <param name="appSettingName">The app setting name.</param>
        /// <returns>The current object (to support method chaining).</returns>
        IConstructorArgumentMapping<TInterface, TImplementation, TArg> EqualToAppSetting(string appSettingName);

        /// <summary>
        /// Tells the container to set the constructor argument to the specified object.
        /// </summary>
        /// <param name="value">Value to use</param>
        /// <returns>The current object (to support method chaining).</returns>
        IMapping<TInterface, TImplementation> Is(TArg value);

        /// <summary>
        /// Tells the container to use the specified type and lambda to supply the constructor argument.
        /// </summary>
        /// <param name="provider">A lambda that should provide the constructor argument</param>
        /// <returns>The current object (to support method chaining).</returns>
        IMapping<TInterface, TImplementation> Is<TProvider>(Func<TProvider, TArg> provider);
    }
}
