using EcsRx.Unity;

namespace EcsRx.Examples.ManualSystems
{
    public class UnityApplication : EcsRxApplicationBehaviour
    {
        protected override void ApplicationStarting()
        {
            RegisterAllBoundSystems();
        }

        protected override void ApplicationStarted() { }
    }
}
