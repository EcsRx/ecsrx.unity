using EcsRx.Examples.SimpleMovement.Components;
using EcsRx.Extensions;
using EcsRx.Infrastructure.Extensions;
using EcsRx.Unity;
using EcsRx.Unity.Extensions;
using EcsRx.Plugins.Views.Components;
using EcsRx.Zenject;
using EcsRx.Zenject.Extensions;
using UnityEngine;

namespace EcsRx.Examples.SimpleMovement
{
    public class Application : EcsRxApplicationBehaviour
    {        
        protected override void ApplicationStarted()
        {
            var defaultPool = EntityDatabase.GetCollection();
            var viewEntity = defaultPool.CreateEntity();
            viewEntity.AddComponents(new ViewComponent(), 
                new PlayerControlledComponent(), new CameraFollowsComponent());
        }
    }
}