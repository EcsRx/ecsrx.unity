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
        public IGroup Group => new PlayerGroup();

        public void Setup(IEntity entity)
        {
            var nameComponent = entity.GetComponent<HasName>();
            var healthComponent = entity.GetComponent<WithHealthComponent>();
            var message = $"{nameComponent.Name} created with {healthComponent.CurrentHealth}/{healthComponent.MaxHealth}";

            Debug.Log(message);
        }
    }
}