using EcsRx.Collections;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Unity.Systems;
using UnityEngine;
using Zenject;

namespace EcsRx.Examples.RandomReactions.ViewResolvers
{
    public class CubeViewResolver : UnityViewResolverSystem
    {
        private const float _spacing = 2.0f;
        private const int _perRow = 10;
        private Vector3 _nextPosition = Vector3.zero;
        private int _currentOnRow = 0;

        protected override GameObject PrefabTemplate => Resources.Load("colored-cube") as GameObject;

        public CubeViewResolver(IEntityCollectionManager collectionManager, IEventSystem eventSystem, IInstantiator instantiator) : base(collectionManager, eventSystem, instantiator)
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