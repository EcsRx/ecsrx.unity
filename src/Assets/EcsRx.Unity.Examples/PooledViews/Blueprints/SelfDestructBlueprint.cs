using EcsRx.Blueprints;
using EcsRx.Entities;
using EcsRx.Unity.Examples.PooledViews.Components;
using EcsRx.Views.Components;
using UnityEngine;

namespace EcsRx.Unity.Examples.PooledViews.Blueprints
{
    public class SelfDestructBlueprint : IBlueprint
    {
        private readonly float _minLifetime = 1.0f;
        private readonly float _maxLifetime = 10.0f;
        private readonly Vector3 _startPosition;

        public SelfDestructBlueprint(Vector3 startPosition)
        {
            _startPosition = startPosition;
        }

        public void Apply(IEntity entity)
        {
            var selfDestructComponent = new SelfDestructComponent
            {
                Lifetime = Random.Range(_minLifetime, _maxLifetime),
                StartingPosition = _startPosition
            };

            entity.AddComponent(selfDestructComponent);
            entity.AddComponent<ViewComponent>();
        }
    }
}