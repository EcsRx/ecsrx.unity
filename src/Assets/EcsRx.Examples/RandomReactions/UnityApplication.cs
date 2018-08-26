using EcsRx.Examples.RandomReactions.Components;
using EcsRx.Unity;
using EcsRx.Unity.Extensions;
using EcsRx.Views.Components;

namespace EcsRx.Examples.RandomReactions
{
    public class UnityApplication : EcsRxApplicationBehaviour
    {
        private readonly int _cubeCount = 5000;
        
        protected override void ApplicationStarting()
        {
            this.BindAllSystemsWithinApplicationScope();
            this.RegisterAllBoundSystems();
        }

        protected override void ApplicationStarted()
        {
            var collection = CollectionManager.GetCollection();

            for (var i = 0; i < _cubeCount; i++)
            {
                var viewEntity = collection.CreateEntity();
                viewEntity.AddComponents(new ViewComponent(), new RandomColorComponent());
            }
        }
    }
}