using System;
using UnityEngine;
using Zenject;

namespace Zenject.SpaceFighter
{
    public class EnemyDeathHandler
    {
        readonly EnemyKilledSignal _enemyKilledSignal;
        readonly EnemyFacade.Pool _selfFactory;
        readonly Settings _settings;
        readonly Explosion.Pool _explosionPool;
        readonly IAudioPlayer _audioPlayer;
        readonly Enemy _enemy;
        readonly EnemyFacade _facade;

        public EnemyDeathHandler(
            Enemy enemy,
            IAudioPlayer audioPlayer,
            Explosion.Pool explosionPool,
            Settings settings,
            EnemyFacade.Pool selfFactory,
            EnemyFacade facade,
            EnemyKilledSignal enemyKilledSignal)
        {
            _enemyKilledSignal = enemyKilledSignal;
            _facade = facade;
            _selfFactory = selfFactory;
            _settings = settings;
            _explosionPool = explosionPool;
            _audioPlayer = audioPlayer;
            _enemy = enemy;
        }

        public void Die()
        {
            var explosion = _explosionPool.Spawn();
            explosion.transform.position = _enemy.Position;

            _audioPlayer.Play(_settings.DeathSound, _settings.DeathSoundVolume);

            _enemyKilledSignal.Fire();

            _selfFactory.Despawn(_facade);
        }

        [Serializable]
        public class Settings
        {
            public AudioClip DeathSound;
            public float DeathSoundVolume = 1.0f;
        }
    }
}
