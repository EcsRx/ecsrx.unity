using System;
using UnityEngine;
using Zenject;

namespace Zenject.SpaceFighter
{
    public enum BulletTypes
    {
        FromEnemy,
        FromPlayer,
    }

    public class Bullet : MonoBehaviour
    {
        float _startTime;
        BulletTypes _type;
        float _speed;
        float _lifeTime;

        [SerializeField]
        MeshRenderer _renderer = null;

        [SerializeField]
        Material _playerMaterial = null;

        [SerializeField]
        Material _enemyMaterial = null;

        [Inject]
        Pool _bulletPool;

        public BulletTypes Type
        {
            get { return _type; }
        }

        public Vector3 MoveDirection
        {
            get { return transform.right; }
        }

        public void OnTriggerEnter(Collider other)
        {
            var enemy = other.GetComponent<EnemyFacade>();

            if (enemy != null && _type == BulletTypes.FromPlayer)
            {
                enemy.Die();
                this.Despawn();
            }

            var player = other.GetComponent<PlayerFacade>();

            if (player != null && _type == BulletTypes.FromEnemy)
            {
                player.TakeDamage(this.MoveDirection);
                this.Despawn();
            }
        }

        public void Update()
        {
            transform.position -= transform.right * _speed * Time.deltaTime;

            if (Time.realtimeSinceStartup - _startTime > _lifeTime)
            {
                Despawn();
            }
        }

        public void Despawn()
        {
            _bulletPool.Despawn(this);
        }

        public class Pool : MonoMemoryPool<float, float, BulletTypes, Bullet>
        {
            protected override void Reinitialize(float speed, float lifeTime, BulletTypes type, Bullet bullet)
            {
                bullet._type = type;
                bullet._speed = speed;
                bullet._lifeTime = lifeTime;

                bullet._renderer.material = type == BulletTypes.FromEnemy ? bullet._enemyMaterial : bullet._playerMaterial;

                bullet._startTime = Time.realtimeSinceStartup;
            }
        }
    }
}
