using Assets.Examples.SimpleMovement.Systems;
using Zenject;

namespace Assets.Examples.SimpleMovement.Installers
{
    public class SimpleInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ViewSetupSystem>().ToSelf().AsSingle();
            Container.Bind<PlayerControlSystem>().ToSelf().AsSingle();
            Container.Bind<CameraFollowSystem>().ToSelf().AsSingle();
        }
    }
}