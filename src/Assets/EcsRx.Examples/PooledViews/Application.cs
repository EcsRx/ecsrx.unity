using EcsRx.Unity;

namespace Assets.EcsRx.Examples.PooledViews
{
    public class Application : EcsRxApplication
    {
        protected override void ApplicationStarting()
        {
            RegisterAllBoundSystems();
        }

        protected override void ApplicationStarted()
        {}
    }
}
