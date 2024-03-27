using SystemsRx.Infrastructure.Dependencies;
using SystemsRx.Infrastructure.Extensions;
using SystemsRx.Scheduling;
using EcsRx.Unity.Scheduling;

namespace EcsRx.Unity.Modules
{
    public class UnityOverrideModule : IDependencyModule 
    {
        public void Setup(IDependencyRegistry  registry)
        {
            registry.Unbind<IUpdateScheduler>();
            registry.Bind<IUpdateScheduler, UnityUpdateScheduler>(x => x.AsSingleton());
        }
    }
}