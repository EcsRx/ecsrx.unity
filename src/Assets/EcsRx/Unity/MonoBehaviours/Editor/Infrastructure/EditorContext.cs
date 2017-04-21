using EcsRx.Unity.Installers;
using Zenject;

namespace EcsRx.Unity.MonoBehaviours.Editor.Infrastructure
{
    public static class EditorContext
    {
        private static readonly DiContainer _container;
        
        static EditorContext()
        {
            _container = new DiContainer();
            _container.Install<EcsRxInstaller>();
        }
        
        public static DiContainer Container
        {
            get { return _container; }
        }
    }
}