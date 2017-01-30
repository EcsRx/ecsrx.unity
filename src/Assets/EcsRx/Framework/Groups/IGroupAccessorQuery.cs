using System.Collections.Generic;
using EcsRx.Entities;
using EcsRx.Groups;

namespace Assets.EcsRx.Framework.Groups
{
    public interface IGroupAccessorQuery
    {
        IEnumerable<IEntity> Execute(IGroupAccessor groupAccessor);
    }
}