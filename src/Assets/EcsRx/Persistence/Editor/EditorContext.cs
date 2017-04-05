using EcsRx.Persistence.Installers;
using EcsRx.Unity.Installers;
using Zenject;

namespace EcsRx.Persistence.Editor
{
    public static class EditorContext
    {
        private static readonly DiContainer _container;

        static EditorContext()
        {
            _container = new DiContainer();
            _container.Install(new DefaultEcsRxInstaller());
            _container.Install(new DefaultPersistanceInstaller());
        }

        public static DiContainer Container
        {
            get { return _container; }
        }
    }
}