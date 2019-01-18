using EcsRx.Entities;
using EcsRx.Examples.UsingBlueprints.Components;
using EcsRx.Examples.UsingBlueprints.Groups;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Plugins.ReactiveSystems.Systems;
using UnityEngine;

namespace EcsRx.Examples.UsingBlueprints.Systems
{
    public class PlayerReportingSystem : ISetupSystem
    {
        public IGroup Group { get { return new PlayerGroup();} }

        public void Setup(IEntity entity)
        {
            var nameComponent = entity.GetComponent<HasName>();
            var healthComponent = entity.GetComponent<WithHealthComponent>();

            var message = string.Format("{0} created with {1}/{2}",
                nameComponent.Name,
                healthComponent.CurrentHealth, healthComponent.MaxHealth);

            Debug.Log(message);
        }
    }
}