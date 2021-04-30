using EcsRx.Entities;
using EcsRx.Examples.CustomGameObjectHandling.Components;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Systems;
using UnityEngine;

namespace EcsRx.Examples.CustomGameObjectHandling.Systems
{
    public class PlayerControlSystem : IBasicEntitySystem
    {
        public readonly float MovementSpeed = 2.0f;

        public IGroup Group => new GroupBuilder()
            .WithComponent<CustomViewComponent>()
            .WithComponent<PlayerControlledComponent>()
            .Build();

        public void Process(IEntity entity)
        {
            var strafeMovement = 0f;
            var forardMovement = 0f;

            if (Input.GetKey(KeyCode.A)) { strafeMovement = -1.0f; }
            if (Input.GetKey(KeyCode.D)) { strafeMovement = 1.0f; }
            if (Input.GetKey(KeyCode.W)) { forardMovement = 1.0f; }
            if (Input.GetKey(KeyCode.S)) { forardMovement = -1.0f; }

            var viewComponent = entity.GetComponent<CustomViewComponent>();
            var transform = viewComponent.CustomView.transform;
            var newPosition = transform.position;

            newPosition.x += strafeMovement * MovementSpeed * Time.deltaTime;
            newPosition.z += forardMovement * MovementSpeed * Time.deltaTime;

            transform.position = newPosition;
        }
    }
}