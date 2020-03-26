using EcsRx.Examples.RandomReactions.Components;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Infrastructure.Extensions;
using EcsRx.Unity;
using EcsRx.Unity.Extensions;
using EcsRx.Plugins.Views.Components;
using EcsRx.Zenject;
using EcsRx.Zenject.Extensions;
using UnityEngine;

namespace EcsRx.Examples.RandomReactions
{
    public class Application : EcsRxApplicationBehaviour
    {
        private readonly int _cubeCount = 5000;
        
        protected override void ApplicationStarted()
        {
            var collection = EntityDatabase.GetCollection();

            for (var i = 0; i < _cubeCount; i++)
            {
                var viewEntity = collection.CreateEntity();
                viewEntity.AddComponents(new ViewComponent(), new RandomColorComponent());
            }

            var group = ObservableGroupManager.GetObservableGroup(new Group(typeof(ViewComponent), typeof(RandomColorComponent)));
            Debug.Log($"There are {group.Count} entities out of {collection.Count} matching");
        }
    }
}