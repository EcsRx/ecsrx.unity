using Assets.EcsRx.Examples.CustomGameObjectHandling.Systems;
using Zenject;
using CameraFollowSystem = Assets.Examples.SimpleMovement.Systems.CameraFollowSystem;
using PlayerControlSystem = Assets.Examples.SimpleMovement.Systems.PlayerControlSystem;

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