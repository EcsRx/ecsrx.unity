using EcsRx.Unity.Extensions;
using UnityEngine;

namespace EcsRx.Unity.Examples.GameObjectLinking
{
    public class UnityApplication : EcsRxApplicationBehaviour
    {
        protected override void ApplicationStarting()
        {
            RegisterAllBoundSystems();
        }

        protected override void ApplicationStarted()
        {
            var defaultPool = PoolManager.GetPool();
            var entity = defaultPool.CreateEntity();

            var existingGameObject = GameObject.Find("ExistingGameObject");
            existingGameObject.LinkEntity(entity, defaultPool);
        }
    }
}