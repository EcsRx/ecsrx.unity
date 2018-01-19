using System;
using UnityEngine;
using Zenject;

#pragma warning disable 649

namespace Zenject.SpaceFighter
{
    public class Explosion : MonoBehaviour
    {
        [SerializeField]
        float _lifeTime;

        [SerializeField]
        ParticleSystem _particleSystem;

        [SerializeField]
        AudioClip _sound;

        [SerializeField]
        float _soundVolume;

        float _startTime;

        [Inject]
        Pool _pool;

        public void Update()
        {
            if (Time.realtimeSinceStartup - _startTime > _lifeTime)
            {
                _pool.Despawn(this);
            }
        }

        public class Pool : MonoMemoryPool<Explosion>
        {
            protected override void Reinitialize(Explosion explosion)
            {
                explosion._particleSystem.Clear();
                explosion._particleSystem.Play();

                explosion._startTime = Time.realtimeSinceStartup;
            }
        }
    }
}

