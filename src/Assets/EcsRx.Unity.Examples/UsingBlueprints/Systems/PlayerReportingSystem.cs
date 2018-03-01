using EcsRx.Entities;
using EcsRx.Groups;
using EcsRx.Systems;
using EcsRx.Unity.Examples.UsingBlueprints.Components;
using EcsRx.Unity.Examples.UsingBlueprints.Groups;
using UnityEngine;

namespace EcsRx.Unity.Examples.UsingBlueprints.Systems
{
    public class PlayerReportingSystem : ISetupSystem
    {
        public IGroup TargetGroup { get { return new PlayerGroup();} }

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