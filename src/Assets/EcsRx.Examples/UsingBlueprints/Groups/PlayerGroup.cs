using System;
using EcsRx.Examples.UsingBlueprints.Components;
using EcsRx.Groups;

namespace EcsRx.Examples.UsingBlueprints.Groups
{
    public class PlayerGroup : IGroup
    {
        public Type[] RequiredComponents { get; } = {
            typeof(HasName), typeof(WithHealthComponent)
        };

        public Type[] ExcludedComponents { get; } = new Type[0];
    }
}