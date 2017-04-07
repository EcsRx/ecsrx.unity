using Zenject;

namespace EcsRx.Unity.Installers
{
    public class DefaultEcsRxInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            new EcsRxInstaller().InstallBindings();
        }
    }
}