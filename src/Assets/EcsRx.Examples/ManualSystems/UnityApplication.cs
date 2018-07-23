using EcsRx.Unity;
using EcsRx.Unity.Extensions;

namespace EcsRx.Examples.ManualSystems
{
    public class UnityApplication : EcsRxApplicationBehaviour
    {
        protected override void ApplicationStarting()
        {
            this.RegisterAllBoundSystems();
        }

        protected override void ApplicationStarted() { }
    }
}
