using Assets.EcsRx.Examples.RandomReactions.Components;
using EcsRx.Entities;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Pools;
using EcsRx.Unity.Systems;
using UnityEngine;

namespace Assets.EcsRx.Examples.RandomReactions.ViewResolvers
{
    public class CubeViewResolver : ViewResolverSystem
    {
        private const float _spacing = 2.0f;
        private const int _perRow = 10;

        private Vector3 _nextPosition = Vector3.zero;
        private int _currentOnRow = 0;

        private GameObject _coloredCubePrefab;

        public CubeViewResolver(IPoolManager poolManager) : base(poolManager)
        {
            _coloredCubePrefab = (GameObject)Resources.Load("colored-cube");
        }

        public override IGroup TargetGroup
        {
            get { return base.TargetGroup.WithComponent<RandomColorComponent>(); }
        }

        public override GameObject ResolveView(IEntity entity)
        {
            var view = (GameObject)Object.Instantiate(_coloredCubePrefab, _nextPosition, Quaternion.identity);
            IncrementRow();
            return view;
        }
        
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
    }
}