using System;
using UnityEngine;
using Zenject;

namespace Zenject.SpaceFighter
{
    // Responsibilities:
    // - Interpolate rotation of enemy towards the current desired
    // direction
    public class EnemyRotationHandler : IFixedTickable
    {
        readonly Settings _settings;
        readonly Enemy _enemy;

        public EnemyRotationHandler(
            Enemy enemy,
            Settings settings)
        {
            _settings = settings;
            _enemy = enemy;
        }

        public void FixedTick()
        {
            var lookDir = _enemy.LookDir;
            var goalDir = _enemy.DesiredLookDir;

            var error = Vector3.Angle(lookDir, goalDir);

            if (Vector3.Cross(lookDir, goalDir).z < 0)
            {
                error *= -1;
            }

            _enemy.AddTorque(error * _settings.TurnSpeed);
        }

        [Serializable]
        public class Settings
        {
            public float TurnSpeed;
        }
    }
}
