﻿using EcsRx.Attributes;
using EcsRx.Groups;
using EcsRx.Systems;

namespace EcsRx.Tests.Systems
{
    [Priority(1)]
    public class HighPrioritySystem : ISystem
    {
        public Group TargetGroup { get { return null; } }
    }
}