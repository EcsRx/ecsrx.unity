using EcsRx.Entities;
using EcsRx.Examples.RandomReactions.Components;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Plugins.ReactiveSystems.Systems;
using EcsRx.Systems;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EcsRx.Examples.RandomReactions.Systems
{
    public class ColorChangingSystem : IBasicEntitySystem, ISetupSystem
    {
        private readonly float MaxDelay = 5.0f;
        private readonly float MinDelay = 1.0f;

        public IGroup Group => new Group(typeof(RandomColorComponent));

        public void Setup(IEntity entity)
        {
            var randomColorComponent = entity.GetComponent<RandomColorComponent>();
            randomColorComponent.NextChangeIn = Random.Range(MinDelay, MaxDelay);
        }
        
        public void Process(IEntity entity)
        {
            var randomColorComponent = entity.GetComponent<RandomColorComponent>();
            randomColorComponent.Elapsed += Time.deltaTime;

            if (!(randomColorComponent.Elapsed >= randomColorComponent.NextChangeIn))
            { return; }

            randomColorComponent.Elapsed -= randomColorComponent.NextChangeIn;
            randomColorComponent.NextChangeIn = Random.Range(MinDelay, MaxDelay);

            var r = Random.Range(0.0f, 1.0f);
            var g = Random.Range(0.0f, 1.0f);
            var b = Random.Range(0.0f, 1.0f);
            randomColorComponent.Color.Value = new Color(r,g,b);
        }
    }
}