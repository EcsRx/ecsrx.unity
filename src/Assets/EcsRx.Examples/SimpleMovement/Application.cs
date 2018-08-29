using EcsRx.Examples.SimpleMovement.Components;
using EcsRx.Unity;
using EcsRx.Unity.Extensions;
using EcsRx.Views.Components;
using EcsRx.Zenject;
using UnityEngine;

namespace EcsRx.Examples.SimpleMovement
{
    public class Application : EcsRxApplicationBehaviour
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
            viewEntity.AddComponents(new ViewComponent(), 
                new PlayerControlledComponent(), new CameraFollowsComponent());
        }
    }
}