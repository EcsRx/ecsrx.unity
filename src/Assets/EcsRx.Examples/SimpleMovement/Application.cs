using EcsRx.Examples.SimpleMovement.Components;
using EcsRx.Infrastructure.Extensions;
using EcsRx.Unity;
using EcsRx.Unity.Extensions;
using EcsRx.Views.Components;
using EcsRx.Zenject;
using EcsRx.Zenject.Extensions;
using UnityEngine;

namespace EcsRx.Examples.SimpleMovement
{
    public class Application : EcsRxApplicationBehaviour
    {        
        protected override void ApplicationStarted()
        {
            var defaultPool = EntityCollectionManager.GetCollection();
            var viewEntity = defaultPool.CreateEntity();
            viewEntity.AddComponents(new ViewComponent(), 
                new PlayerControlledComponent(), new CameraFollowsComponent());
        }
    }
}