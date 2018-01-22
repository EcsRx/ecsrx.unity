using UnityEngine;
using Zenject;

namespace Zenject.SpaceFighter
{
    public interface IPlayer
    {
        bool IsDead { get; }
        Vector3 Position { get; }
        Quaternion Rotation { get; }

        void TakeDamage(Vector3 moveDirection);
    }

    public class PlayerFacade : MonoBehaviour, IPlayer
    {
        Player _model;
        PlayerDamageHandler _hitHandler;

        [Inject]
        public void Construct(Player player, PlayerDamageHandler hitHandler)
        {
            _model = player;
            _hitHandler = hitHandler;
        }

        public bool IsDead
        {
            get { return _model.IsDead; }
        }

        public Vector3 Position
        {
            get { return _model.Position; }
        }

        public Quaternion Rotation
        {
            get { return _model.Rotation; }
        }

        public void TakeDamage(Vector3 moveDirection)
        {
            _hitHandler.TakeDamage(moveDirection);
        }
    }
}
