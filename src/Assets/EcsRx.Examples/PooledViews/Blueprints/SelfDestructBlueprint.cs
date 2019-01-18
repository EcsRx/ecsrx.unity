using EcsRx.Blueprints;
using EcsRx.Entities;
using EcsRx.Examples.PooledViews.Components;
using EcsRx.Extensions;
using EcsRx.Plugins.Views.Components;
using UnityEngine;

namespace EcsRx.Examples.PooledViews.Blueprints
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

            var viewComponent = new ViewComponent();
            entity.AddComponents(selfDestructComponent, viewComponent);
        }
    }
}