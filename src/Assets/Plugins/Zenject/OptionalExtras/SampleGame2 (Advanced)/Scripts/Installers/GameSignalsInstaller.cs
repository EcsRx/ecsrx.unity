using Zenject;

namespace Zenject.SpaceFighter
{
    public class GameSignalsInstaller : Installer<GameSignalsInstaller>
    {
        public override void InstallBindings()
        {
            Container.DeclareSignal<EnemyKilledSignal>();
            Container.DeclareSignal<PlayerDiedSignal>();
        }
    }

}
