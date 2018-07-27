using EcsRx.Examples.CustomGameObjectHandling.Components;
using EcsRx.Examples.CustomGameObjectHandling.Systems;
using EcsRx.Unity;
using EcsRx.Unity.Extensions;
using Zenject;

namespace EcsRx.Examples.CustomGameObjectHandling
{
    public class UnityApplication : EcsRxApplicationBehaviour
    {        
        protected override void ApplicationStarted()
        {
            this.BindAllSystemsWithinApplicationScope();
            this.RegisterAllBoundSystems();

            var defaultPool = CollectionManager.GetCollection();
            var viewEntity = defaultPool.CreateEntity();
            viewEntity.AddComponents(new CustomViewComponent(), new PlayerControlledComponent(), new CameraFollowsComponent());
        }
    }
}