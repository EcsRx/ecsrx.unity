using EcsRx.Unity;
using EcsRx.Unity.Extensions;

namespace EcsRx.Examples.PooledViews
{
    public class UnityApplication : EcsRxApplicationBehaviour
    {
        protected override void ApplicationStarting()
        {
            this.BindAllSystemsWithinApplicationScope();
        }

        protected override void ApplicationStarted()
        {
            this.RegisterAllBoundSystems();
        }
    }
}
