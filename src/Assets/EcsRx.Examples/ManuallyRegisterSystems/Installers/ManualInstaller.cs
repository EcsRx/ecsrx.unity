using EcsRx.Examples.ManuallyRegisterSystems.Systems;
using Zenject;

namespace EcsRx.Examples.ManuallyRegisterSystems.Installers
{
    public class ManualInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            // Manually bind RandomMovementSystem
            Container.Bind<RandomMovementSystem>().ToSelf().AsSingle();
        }
    }
}