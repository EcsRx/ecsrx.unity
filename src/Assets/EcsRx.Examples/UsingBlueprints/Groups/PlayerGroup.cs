using System;
using System.Collections.Generic;
using Assets.EcsRx.Examples.UsingBlueprints.Components;
using EcsRx.Entities;
using EcsRx.Groups;

namespace Assets.EcsRx.Examples.UsingBlueprints.Groups
{
    public class PlayerGroup : Group
    {
        private readonly IEnumerable<Type> _components = new[]
        {
            typeof (HasName), typeof (WithHealthComponent)
        };

        public IEnumerable<Type> TargettedComponents { get { return _components; } }
        public Predicate<IEntity> TargettedEntities { get { return null; } }
    }
}