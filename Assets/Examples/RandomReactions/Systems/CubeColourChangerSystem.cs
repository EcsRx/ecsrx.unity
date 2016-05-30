using Assets.Examples.RandomReactions.Components;
using EcsRx.Entities;
using EcsRx.Groups;
using EcsRx.Systems;
using UniRx;
using UnityEngine;

namespace Assets.Examples.RandomReactions.Systems
{
    public class CubeColourChangerSystem : IReactToEntitySystem
    {
        public IGroup TargetGroup
        {
            get
            {
                return new GroupBuilder()
                    .WithComponent<HasCubeComponent>()
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
            var cubeComponent = entity.GetComponent<HasCubeComponent>();
            var renderer = cubeComponent.Cube.GetComponent<Renderer>();
            renderer.material.color = colorComponent.Color.Value;
        }
    }
}