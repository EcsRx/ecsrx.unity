using EcsRx.Unity;

namespace Assets.EcsRx.Examples.ManualSystems
{
    public class Application : EcsRxApplication
    {
        protected override void ApplicationStarting()
        {
            RegisterAllBoundSystems();
        }

        protected override void ApplicationStarted() { }
    }
}
