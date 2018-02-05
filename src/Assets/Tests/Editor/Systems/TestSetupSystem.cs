﻿using EcsRx.Entities;
using EcsRx.Groups;
using EcsRx.Systems;
using EcsRx.Tests.Components;

namespace EcsRx.Tests.Systems
{
    public class TestSetupSystem : ISetupSystem
    {
        public Group TargetGroup { get { return new Group(typeof(TestComponentOne));} }

        public void Setup(IEntity entity)
        {
            var testComponent = entity.GetComponent<TestComponentOne>();
            testComponent.Data = "woop";
        }
    }
}