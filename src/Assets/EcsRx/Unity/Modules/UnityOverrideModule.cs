using EcsRx.Infrastructure.Dependencies;
using EcsRx.Infrastructure.Extensions;
using EcsRx.Scheduling;
using EcsRx.Unity.Scheduling;

namespace EcsRx.Unity.Modules
{
    public class UnityOverrideModule : IDependencyModule 
    {
        public void Setup(IDependencyContainer container)
        {
            container.Unbind<IUpdateScheduler>();
            container.Bind<IUpdateScheduler, UnityUpdateScheduler>(x => x.AsSingleton());
        }
    }
}