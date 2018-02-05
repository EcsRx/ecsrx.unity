﻿using Assets.EcsRx.Examples.UsingBlueprints.Components;
using Assets.EcsRx.Examples.UsingBlueprints.Groups;
using EcsRx.Entities;
using EcsRx.Groups;
using EcsRx.Systems;
using UnityEngine;

namespace Assets.EcsRx.Examples.UsingBlueprints.Systems
{
    public class PlayerReportingSystem : ISetupSystem
    {
        public Group TargetGroup { get { return new PlayerGroup();} }

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