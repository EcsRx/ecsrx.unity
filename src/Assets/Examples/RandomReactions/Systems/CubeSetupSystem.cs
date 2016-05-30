using Assets.Examples.RandomReactions.Components;
using EcsRx.Entities;
using EcsRx.Groups;
using EcsRx.Systems;
using UnityEngine;

namespace Assets.Examples.RandomReactions.Systems
{
    public class CubeSetupSystem : ISetupSystem
    {
        private const float _spacing = 2.0f;
        private const int _perRow = 10;

        private Vector3 _nextPosition = Vector3.zero;
        private int _currentOnRow = 0;

        private GameObject _coloredCubePrefab;

        public CubeSetupSystem()
        {
            _coloredCubePrefab = (GameObject)Resources.Load("colored-cube");
        }

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

        public void Setup(IEntity entity)
        {
            var cubeComponent = entity.GetComponent<HasCubeComponent>();
            cubeComponent.Cube = (GameObject)Object.Instantiate(_coloredCubePrefab, _nextPosition, Quaternion.identity);
            IncrementRow();
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