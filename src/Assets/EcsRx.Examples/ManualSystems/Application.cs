using EcsRx.Unity;

namespace Assets.EcsRx.Examples.ManualSystems
{
    public class Application : EcsRxApplication
    {
        protected override void GameStarting()
        {
            RegisterAllBoundSystems();
        }

        protected override void GameStarted() { }
    }
}
