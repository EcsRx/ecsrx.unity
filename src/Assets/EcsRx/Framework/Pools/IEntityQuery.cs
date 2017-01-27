using System.Collections.Generic;
using EcsRx.Entities;

namespace EcsRx.Pools
{
    public interface IEntityQuery
    {
        IEnumerable<IEntity> Execute(IEnumerable<IEntity> entityList);
    }
}