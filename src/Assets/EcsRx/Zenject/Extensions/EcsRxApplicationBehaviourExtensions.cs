using System.Linq;
using EcsRx.Extensions;
using EcsRx.Infrastructure.Dependencies;
using EcsRx.Systems;
using EcsRx.Views.Systems;
using EcsRx.Zenject.Helpers;
using Zenject;

namespace EcsRx.Zenject.Extensions
{
    public static class EcsRxApplicationBehaviourExtensions
    {
        /// <summary>
        /// This will register all ISystem implementations within the DI container
        /// with the SystemExecutor.
        /// </summary>
        /// <param name="application">The application to act on</param>
        public static void RegisterAllBoundSystems(this EcsRxApplicationBehaviour application)
        {
            var allSystems = application.DependencyContainer.ResolveAll<ISystem>();

            var orderedSystems = allSystems
                .OrderByDescending(x => x is ViewResolverSystem)
                .ThenByDescending(x => x is ISetupSystem)
                .ThenByPriority();

            orderedSystems.ForEachRun(application.SystemExecutor.AddSystem);
        }
        
        /// <summary>
        /// This will bind the given system type (T) to the DI container against `ISystem`
        /// and will then immediately register the system with the SystemExecutor.
        /// </summary>
        /// <param name="application">The application to act on</param>
        /// <typeparam name="T">The implementation of ISystem to bind/register</typeparam>
        public static void BindAndRegisterSystem<T>(this EcsRxApplicationBehaviour application) where T : ISystem
        {
            application.DependencyContainer.Bind<ISystem, T>(new BindingConfiguration{WithName = typeof(T).Name});
            RegisterSystem<T>(application);
        }

        /// <summary>
        /// This will resolve the given type (T) from the DI container then register it
        /// with the SystemExecutor.
        /// </summary>
        /// <param name="application">The application to act on</param>
        /// <typeparam name="T">The implementation of ISystem to register</typeparam>
        public static void RegisterSystem<T>(this EcsRxApplicationBehaviour application) where T : ISystem
        {
            ISystem system;
            
            if(application.DependencyContainer.HasBinding<ISystem>(typeof(T).Name))
            { system = application.DependencyContainer.Resolve<ISystem>(typeof(T).Name); }
            else
            { system = application.DependencyContainer.Resolve<T>(); }
            
            application.SystemExecutor.AddSystem(system);
        }

        /// <summary>
        /// This will bind any ISystem implementations that are found within the namespaces provided
        /// </summary>
        /// <remarks>
        /// This is not recursive, so if you have child namespaces they will need to be provided,
        /// it is also advised you wrap this method with your own conventions like BindAllSystemsWithinApplicationScope does.
        /// </remarks>
        /// <param name="application">The application to act on</param>
        /// <param name="namespaces">The namespaces to be scanned for implementations</param>
        public static void BindAnySystemsInNamespace(this EcsRxApplicationBehaviour application, params string[] namespaces)
        {
            BindSystemsInNamespace.Bind(application.DependencyContainer.NativeContainer as DiContainer, namespaces);
        }
        
        /// <summary>
        /// This will bind any ISystem implementations found within Systems, ViewResolvers folders which are located
        /// in a child namespace of the application.
        /// </summary>
        /// <remarks>
        /// This is a conventional based binding, and if you need other conventions then look at wrapping BindAnySystemsInNamespace
        /// </remarks>
        /// <param name="application">The application to act on</param>
        public static void BindAllSystemsWithinApplicationScope(this EcsRxApplicationBehaviour application)
        {
            var applicationNamespace = application.GetType().Namespace;
            var namespaces = new[]
            {
                $"{applicationNamespace}.Systems",
                $"{applicationNamespace}.ViewResolvers"
            };
            
            BindSystemsInNamespace.Bind(application.DependencyContainer.NativeContainer as DiContainer, namespaces);
        }
    }
}