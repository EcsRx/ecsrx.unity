using System;
using System.Collections.Generic;
using EcsRx.Groups;
using EcsRx.Unity.Examples.UsingBlueprints.Components;

namespace EcsRx.Unity.Examples.UsingBlueprints.Groups
{
    public class PlayerGroup : IGroup
    {
        public IEnumerable<Type> MatchesComponents { get; } = new[]
        {
            typeof(HasName), typeof(WithHealthComponent)
        };
    }
}