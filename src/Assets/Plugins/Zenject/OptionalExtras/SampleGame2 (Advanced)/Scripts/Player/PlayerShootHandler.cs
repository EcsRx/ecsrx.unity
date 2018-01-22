using System;
using UnityEngine;
using Zenject;

namespace Zenject.SpaceFighter
{
    public class PlayerShootHandler : ITickable
    {
        readonly IAudioPlayer _audioPlayer;
        readonly Player _player;
        readonly Settings _settings;
        readonly Bullet.Pool _bulletPool;
        readonly PlayerInputState _inputState;

        float _lastFireTime;

        public PlayerShootHandler(
            PlayerInputState inputState,
            Bullet.Pool bulletPool,
            Settings settings,
            Player player,
            IAudioPlayer audioPlayer)
        {
            _audioPlayer = audioPlayer;
            _player = player;
            _settings = settings;
            _bulletPool = bulletPool;
            _inputState = inputState;
        }

        public void Tick()
        {
            if (_player.IsDead)
            {
                return;
            }

            if (_inputState.IsFiring && Time.realtimeSinceStartup - _lastFireTime > _settings.MaxShootInterval)
            {
                _lastFireTime = Time.realtimeSinceStartup;
                Fire();
            }
        }

        void Fire()
        {
            _audioPlayer.Play(_settings.Laser, _settings.LaserVolume);

            var bullet = _bulletPool.Spawn(
                _settings.BulletSpeed, _settings.BulletLifetime, BulletTypes.FromPlayer);

            bullet.transform.position = _player.Position + _player.LookDir * _settings.BulletOffsetDistance;
            bullet.transform.rotation = _player.Rotation;
        }

        [Serializable]
        public class Settings
        {
            public AudioClip Laser;
            public float LaserVolume = 1.0f;

            public float BulletLifetime;
            public float BulletSpeed;
            public float MaxShootInterval;
            public float BulletOffsetDistance;
        }
    }
}
