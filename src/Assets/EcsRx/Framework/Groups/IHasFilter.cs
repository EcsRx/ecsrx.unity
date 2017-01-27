using System.Collections.Generic;
using EcsRx.Entities;

namespace EcsRx.Groups
{
    public interface IHasFilter
    {
        IEnumerable<IEntity> FilterEntities(IEnumerable<IEntity> entities);
    }
}