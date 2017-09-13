using Zenject;

namespace EcsRx.Unity.Installers
{
    public class DefaultEcsRxInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            EcsRxInstaller.Install(Container);
        }
    }
}