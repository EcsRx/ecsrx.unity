using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Groups.Accessors;
using EcsRx.Groups;
using EcsRx.Pools;
using EcsRx.Systems.Executor;
using EcsRx.Systems.Executor.Handlers;
using EcsRx.Unity.Systems;
using UniRx;
using Zenject;

namespace EcsRx.Unity.Installers
{
    public class DefaultEcsRxInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IMessageBroker>().To<MessageBroker>().AsSingle();
            Container.Bind<IEventSystem>().To<EventSystem>().AsSingle();

            Container.Bind<IEntityFactory>().To<DefaultEntityFactory>().AsSingle();
            Container.Bind<IPoolFactory>().To<DefaultPoolFactory>().AsSingle();
            Container.Bind<IGroupAccessorFactory>().To<DefaultGroupAccessorFactory>().AsSingle();

            Container.Bind<IPoolManager>().To<PoolManager>().AsSingle();
            Container.Bind<IViewHandler>().To<ViewHandler>().AsSingle();

            Container.Bind<IReactToDataSystemHandler>().To<ReactToDataSystemHandler>().AsSingle();
            Container.Bind<IReactToEntitySystemHandler>().To<ReactToEntitySystemHandler>().AsSingle();
            Container.Bind<IReactToGroupSystemHandler>().To<ReactToGroupSystemHandler>().AsSingle();
            Container.Bind<ISetupSystemHandler>().To<SetupSystemHandler>().AsSingle();
            Container.Bind<IManualSystemHandler>().To<ManualSystemHandler>().AsSingle();

            Container.Bind<ISystemExecutor>().To<SystemExecutor>().AsSingle();
        }
    }
}