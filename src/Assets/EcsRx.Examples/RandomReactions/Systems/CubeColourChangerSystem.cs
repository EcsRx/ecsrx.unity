using System;
using EcsRx.Entities;
using EcsRx.Examples.RandomReactions.Components;
using EcsRx.Groups;
using EcsRx.Systems;
using EcsRx.Views.Components;
using UniRx;
using UnityEngine;

namespace EcsRx.Examples.RandomReactions.Systems
{
    public class CubeColourChangerSystem : IReactToEntitySystem
    {
        public IGroup Group => new GroupBuilder()
            .WithComponent<ViewComponent>()
            .WithComponent<RandomColorComponent>()
            .Build();

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