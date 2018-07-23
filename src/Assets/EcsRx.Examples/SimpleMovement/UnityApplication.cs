using EcsRx.Examples.SimpleMovement.Components;
using EcsRx.Unity;
using EcsRx.Unity.Extensions;
using EcsRx.Views.Components;
using UnityEngine;

namespace EcsRx.Examples.SimpleMovement
{
    public class UnityApplication : EcsRxApplicationBehaviour
    {        
        protected override void ApplicationStarting()
        {
            this.BindAllSystemsWithinApplicationScope();
            this.RegisterAllBoundSystems();
        }

        protected override void ApplicationStarted()
        {
            var defaultPool = CollectionManager.GetCollection();
            var viewEntity = defaultPool.CreateEntity();
            viewEntity.AddComponent(new ViewComponent());
            viewEntity.AddComponent(new PlayerControlledComponent());
            viewEntity.AddComponent(new CameraFollowsComponent());
        }
    }
}