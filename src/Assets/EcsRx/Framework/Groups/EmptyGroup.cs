using System;
using System.Collections.Generic;
using EcsRx.Entities;

namespace EcsRx.Groups
{
    public class EmptyGroup : Group
    {
        public IEnumerable<Type> TargettedComponents { get { return new Type[0]; } }
        public Predicate<IEntity> TargettedEntities { get { return null; } }
    }
}