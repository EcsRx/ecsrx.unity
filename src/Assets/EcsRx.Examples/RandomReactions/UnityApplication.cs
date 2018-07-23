using EcsRx.Examples.RandomReactions.Components;
using EcsRx.Unity;
using EcsRx.Views.Components;

namespace EcsRx.Examples.RandomReactions
{
    public class UnityApplication : EcsRxApplicationBehaviour
    {
        private readonly int _cubeCount = 500;
        
        protected override void ApplicationStarting()
        {
            RegisterAllBoundSystems();
        }

        protected override void ApplicationStarted()
        {
            var defaultPool = CollectionManager.GetCollection();

            for (var i = 0; i < _cubeCount; i++)
            {
                var viewEntity = defaultPool.CreateEntity();
                viewEntity.AddComponents(new ViewComponent(), new RandomColorComponent());
            }
        }
    }
}