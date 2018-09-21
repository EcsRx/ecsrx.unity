using EcsRx.Infrastructure.Dependencies;
using EcsRx.Infrastructure.Extensions;
using EcsRx.Infrastructure.Modules;
using EcsRx.Persistence.Modules;
using EcsRx.Zenject.Dependencies;
using Zenject;

namespace EcsRx.Persistence.Editor.Infrastructure
{
    public static class EditorContext
    {
        private static readonly IDependencyContainer _container;
        
        static EditorContext()
        {
            var internalContainer = new DiContainer();
            _container = new ZenjectDependencyContainer(internalContainer);
            _container.LoadModule<FrameworkModule>();
            _container.LoadModule<PersistenceModule>();
        }
        
        public static IDependencyContainer Container => _container;
    }
}