using System;
using System.Collections.Generic;
using EcsRx.Entities;

namespace EcsRx.Groups
{
    public interface IGroup
    {
        IEnumerable<Type> TargettedComponents { get; }
        Predicate<IEntity> TargettedEntities { get; }
    }
}