using Assets.EcsRx.Examples.SceneFirstSetup.Components;
using EcsRx.Unity;
using EcsRx.Unity.Components;

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
