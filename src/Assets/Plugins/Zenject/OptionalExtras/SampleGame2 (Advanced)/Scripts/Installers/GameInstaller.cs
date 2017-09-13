using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Zenject.SpaceFighter
{
    // Main installer for our game
    public class GameInstaller : MonoInstaller
    {
        [Inject]
        Settings _settings = null;

        public override void InstallBindings()
        {
            Container.DeclareSignal<EnemyKilledSignal>();
            Container.DeclareSignal<PlayerDiedSignal>();

            Container.BindInterfacesTo<EnemySpawner>().AsSingle();

            Container.BindMemoryPool<EnemyFacade, EnemyFacade.Pool>()
                .FromSubContainerResolve()
                .ByNewPrefab(_settings.EnemyFacadePrefab)
                .UnderTransformGroup("Enemies");

            Container.BindMemoryPool<Bullet, Bullet.Pool>().WithInitialSize(10).ExpandByDoubling()
                .FromComponentInNewPrefab(_settings.BulletPrefab)
                .UnderTransformGroup("Bullets");

            Container.Bind<LevelBoundary>().AsSingle();

            Container.BindMemoryPool<Explosion, Explosion.Pool>().WithInitialSize(3)
                .FromComponentInNewPrefab(_settings.ExplosionPrefab)
                .UnderTransformGroup("Explosions");

            Container.Bind<AudioPlayer>().AsSingle();

            Container.BindInterfacesTo<GameRestartHandler>().AsSingle();
        }

        [Serializable]
        public class Settings
        {
            public GameObject EnemyFacadePrefab;
            public GameObject BulletPrefab;
            public GameObject ExplosionPrefab;
        }
    }
}
