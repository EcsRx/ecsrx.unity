using System;
using Assets.EcsRx.Examples.RandomReactions.Components;
using EcsRx.Entities;
using EcsRx.Groups;
using EcsRx.Systems;
using EcsRx.Unity.Components;
using UniRx;
using UnityEngine;

namespace Assets.EcsRx.Examples.RandomReactions.Systems
{
    public class CubeColourChangerSystem : IReactToEntitySystem
    {
        public Group TargetGroup
        {
            get
            {
                return new GroupBuilder()
                    .WithComponent<ViewComponent>()
                    .WithComponent<RandomColorComponent>()
                    .Build();
            }
        }

        public IObservable<IEntity> ReactToEntity(IEntity entity)
        {
            var colorComponent = entity.GetComponent<RandomColorComponent>();
            return colorComponent.Color.DistinctUntilChanged().Select(x => entity);
        }

        public void Execute(IEntity entity)
        {
            var colorComponent = entity.GetComponent<RandomColorComponent>();
            var cubeComponent = entity.GetComponent<ViewComponent>();
            var renderer = cubeComponent.View.GetComponent<Renderer>();
            renderer.material.color = colorComponent.Color.Value;
        }
    }
}