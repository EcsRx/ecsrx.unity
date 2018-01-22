using System;
using UnityEngine;
using System.Linq;

namespace Zenject.SpaceFighter
{
    public class EnemyStateIdle : IEnemyState
    {
        readonly Settings _settings;
        readonly Enemy _enemy;

        Vector3 _startPos;
        float _theta;
        Vector3 _startLookDir;

        public EnemyStateIdle(
            Enemy enemy, Settings settings)
        {
            _settings = settings;
            _enemy = enemy;
        }

        public void EnterState()
        {
            _startPos = _enemy.Position;
            _theta = UnityEngine.Random.Range(0, 2.0f * Mathf.PI);
            _startLookDir = _enemy.LookDir;
        }

        public void ExitState()
        {
        }

        // Just add a bit of subtle movement
        public void Update()
        {
            _theta += Time.deltaTime * _settings.Frequency;

            _enemy.Position = _startPos + _enemy.RightDir * _settings.Amplitude * Mathf.Sin(_theta);
            _enemy.DesiredLookDir = _startLookDir;
        }

        public void FixedUpdate()
        {
        }

        [Serializable]
        public class Settings
        {
            public float Amplitude;
            public float Frequency;
        }
    }
}
