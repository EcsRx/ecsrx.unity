using SystemsRx.Events;
using EcsRx.Collections.Database;
using EcsRx.Entities;
using EcsRx.Unity.Dependencies;
using EcsRx.Unity.Systems;
using UnityEngine;

namespace EcsRx.Examples.RandomReactions.ViewResolvers
{
    public class CubeViewResolver : PrefabViewResolverSystem
    {
        private const float _spacing = 2.0f;
        private const int _perRow = 20;
        private Vector3 _nextPosition = Vector3.zero;
        private int _currentOnRow = 0;

        protected override GameObject PrefabTemplate { get; } = Resources.Load<GameObject>("colored-cube");

        public CubeViewResolver(IEntityDatabase entityDatabase, IEventSystem eventSystem, IUnityInstantiator instantiator)
            : base(entityDatabase, eventSystem, instantiator)
        {}
        
        private void IncrementRow()
        {
            _currentOnRow++;

            if (_currentOnRow < _perRow)
            {
                _nextPosition.x += _spacing;
                return;
            }

            _currentOnRow = 0;
            _nextPosition.x = 0.0f;
            _nextPosition.z += _spacing;
        }

        protected override void OnViewCreated(IEntity entity, GameObject view)
        {
            view.transform.position = _nextPosition;
            IncrementRow();
        }
    }
}