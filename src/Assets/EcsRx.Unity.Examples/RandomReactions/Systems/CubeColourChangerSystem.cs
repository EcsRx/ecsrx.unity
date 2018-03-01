using System;
using EcsRx.Entities;
using EcsRx.Groups;
using EcsRx.Systems;
using EcsRx.Unity.Examples.RandomReactions.Components;
using EcsRx.Views.Components;
using UniRx;
using UnityEngine;

namespace EcsRx.Unity.Examples.RandomReactions.Systems
{
    public class CubeColourChangerSystem : IReactToEntitySystem
    {
        public IGroup TargetGroup => new GroupBuilder()
            .WithComponent<ViewComponent>()
            .WithComponent<RandomColorComponent>()
            .Build();

        public IObservable<IEntity> ReactToEntity(IEntity entity)
        {
            var colorComponent = entity.GetComponent<RandomColorComponent>();
            return colorComponent.Color.DistinctUntilChanged().Select(x => entity);
        }

        public void Execute(IEntity entity)
        {
            var colorComponent = entity.GetComponent<RandomColorComponent>();
            var cubeComponent = entity.GetComponent<ViewComponent>();
            var view = cubeComponent.View as GameObject;
            var renderer = view.GetComponent<Renderer>();
            renderer.material.color = colorComponent.Color.Value;
        }
    }
}