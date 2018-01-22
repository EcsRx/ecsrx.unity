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
            Container.BindInterfacesTo<EnemySpawner>().AsSingle();

            Container.BindMemoryPool<EnemyFacade, EnemyFacade.Pool>()
                .WithInitialSize(10)
                .FromSubContainerResolve()
                .ByNewPrefab(_settings.EnemyFacadePrefab)
                .UnderTransformGroup("Enemies");

            Container.BindMemoryPool<Bullet, Bullet.Pool>()
                .WithInitialSize(25)
                .FromComponentInNewPrefab(_settings.BulletPrefab)
                .UnderTransformGroup("Bullets");

            Container.Bind<LevelBoundary>().AsSingle();

            Container.BindMemoryPool<Explosion, Explosion.Pool>()
                .WithInitialSize(4)
                .FromComponentInNewPrefab(_settings.ExplosionPrefab)
                .UnderTransformGroup("Explosions");

            Container.Bind<IAudioPlayer>().To<AudioPlayer>().AsSingle();

            Container.BindInterfacesTo<GameRestartHandler>().AsSingle();

            GameSignalsInstaller.Install(Container);
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
