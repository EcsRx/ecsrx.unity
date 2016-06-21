using System.Collections.Generic;
using EcsRx.Entities;
using EcsRx.Extensions;
using EcsRx.Pools;
using UniRx;

namespace EcsRx.Systems.Executor.Handlers
{
    public class ReactToDataSystemHandler : IReactToDataSystemHandler
    {
        public IPoolManager PoolManager { get; private set; }

        public ReactToDataSystemHandler(IPoolManager poolManager)
        {
            PoolManager = poolManager;
        }

        // TODO: This is REALLY bad but currently no other way around the dynamic invocation lookup stuff
        public IEnumerable<SubscriptionToken> SetupWithoutType(ISystem system)
        {
            var method = GetType().GetMethod("Setup");
            var genericDataType = system.GetGenericDataType();
            var genericMethod = method.MakeGenericMethod(genericDataType);
            return (IEnumerable<SubscriptionToken>)genericMethod.Invoke(this, new[] { system });
        }

        // TODO: This is REALLY bad but currently no other way around the dynamic invocation lookup stuff
        public SubscriptionToken ProcessEntityWithoutType(ISystem system, IEntity entity)
        {
            var method = GetType().GetMethod("ProcessEntity");
            var genericDataType = system.GetGenericDataType();
            var genericMethod = method.MakeGenericMethod(genericDataType);
            return (SubscriptionToken)genericMethod.Invoke(this, new object[] { system, entity });
        }

        public IEnumerable<SubscriptionToken> Setup<T>(IReactToDataSystem<T> system)
        {
            var groupAccessor = PoolManager.CreateGroupAccessor(system.TargetGroup);
            return groupAccessor.Entities.ForEachRun(x => ProcessEntity(system, x));
        }

        public SubscriptionToken ProcessEntity<T>(IReactToDataSystem<T> system, IEntity entity)
        {
            var hasEntityPredicate = system.TargetGroup.TargettedEntities != null;
            var subscription = system.ReactToEntity(entity)
                    .Subscribe(x =>
                    {
                        if (hasEntityPredicate)
                        {
                            if(system.TargetGroup.TargettedEntities(entity))
                            { system.Execute(entity, x); }
                            return;
                        }

                        system.Execute(entity, x);
                    });

            return new SubscriptionToken(entity, subscription);
        }
    }
}