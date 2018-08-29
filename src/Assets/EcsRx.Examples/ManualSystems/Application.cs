using EcsRx.Unity;
using EcsRx.Unity.Extensions;
using EcsRx.Zenject;

namespace EcsRx.Examples.ManualSystems
{
    public class Application : EcsRxApplicationBehaviour
    {
        protected override void ApplicationStarting()
        {
            this.BindAllSystemsWithinApplicationScope();
            this.RegisterAllBoundSystems();
        }

        protected override void ApplicationStarted() { }
    }
}
