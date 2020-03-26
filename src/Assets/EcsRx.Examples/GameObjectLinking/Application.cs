using EcsRx.Infrastructure.Extensions;
using EcsRx.Unity;
using EcsRx.Unity.Extensions;
using EcsRx.Zenject;
using EcsRx.Zenject.Extensions;
using UnityEngine;

namespace EcsRx.Examples.GameObjectLinking
{
    public class Application : EcsRxApplicationBehaviour
    {
        protected override void ApplicationStarted()
        {
            var defaultPool = EntityDatabase.GetCollection();
            var entity = defaultPool.CreateEntity();

            var existingGameObject = GameObject.Find("ExistingGameObject");
            existingGameObject.LinkEntity(entity, defaultPool);
        }
    }
}