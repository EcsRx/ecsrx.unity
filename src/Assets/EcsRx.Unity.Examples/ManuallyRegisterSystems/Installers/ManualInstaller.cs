using EcsRx.Unity.Examples.ManuallyRegisterSystems.Systems;
using Zenject;

namespace EcsRx.Unity.Examples.ManuallyRegisterSystems.Installers
{
    public class ManualInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<DefaultViewResolver>().ToSelf().AsSingle();
            Container.Bind<RandomMovementSystem>().ToSelf().AsSingle();
        }
    }
}