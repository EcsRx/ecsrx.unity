using System.Linq;
using EcsRx.Executor;
using EcsRx.Extensions;
using EcsRx.Systems;
using EcsRx.Unity.Installers;
using EcsRx.Views.Systems;
using Zenject;

namespace EcsRx.Unity.Extensions
{
    public static class EcsRxApplicationBehaviourExtensions
    {
        public static void RegisterAllBoundSystems(this EcsRxApplicationBehaviour application)
        {
            var allSystems = application.DependencyContainer.ResolveAll<ISystem>();

            var orderedSystems = allSystems
                .OrderByDescending(x => x is ViewResolverSystem)
                .ThenByDescending(x => x is ISetupSystem);

            orderedSystems.ForEachRun(application.SystemExecutor.AddSystem);
        }

        public static void RegisterSystem<T>(this EcsRxApplicationBehaviour application) where T : ISystem
        {
            var system = application.DependencyContainer.Resolve<T>();
            application.SystemExecutor.AddSystem(system);
        }

        public static void RegisterSystemsInNamespace(this EcsRxApplicationBehaviour application, params string[] namespaces)
        {
            BindSystemsInNamespace.Bind(application.DependencyContainer.NativeContainer as DiContainer, namespaces);
        }
        
        public static void RegisterAllSystemsWithinApplicationScope(this EcsRxApplicationBehaviour application)
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