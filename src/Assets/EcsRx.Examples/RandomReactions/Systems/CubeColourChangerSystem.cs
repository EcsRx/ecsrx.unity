using System;
using EcsRx.Entities;
using EcsRx.Examples.RandomReactions.Components;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Plugins.ReactiveSystems.Systems;
using EcsRx.Plugins.Views.Components;
using UniRx;
using UnityEngine;

namespace EcsRx.Examples.RandomReactions.Systems
{
    public class CubeColourChangerSystem : IReactToEntitySystem
    {
        public IGroup Group => new Group(typeof(ViewComponent), typeof(RandomColorComponent));

        private static int count;
        public IObservable<IEntity> ReactToEntity(IEntity entity)
        {
            var colorComponent = entity.GetComponent<RandomColorComponent>();
            return colorComponent.Color.DistinctUntilChanged().Select(x => entity);
        }

        public void Process(IEntity entity)
        {
            var colorComponent = entity.GetComponent<RandomColorComponent>();
            var cubeComponent = entity.GetComponent<ViewComponent>();
            var view = cubeComponent.View as GameObject;
            var renderer = view.GetComponent<Renderer>();
            renderer.material.color = colorComponent.Color.Value;
        }
    }
}