using EcsRx.Unity;
using EcsRx.Unity.Extensions;
using UnityEngine;

namespace EcsRx.Examples.GameObjectLinking
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
            
            var defaultPool = CollectionManager.GetCollection();
            var entity = defaultPool.CreateEntity();

            var existingGameObject = GameObject.Find("ExistingGameObject");
            existingGameObject.LinkEntity(entity, defaultPool);
        }
    }
}