using System.Collections.Generic;
using Assets.EcsRx.Framework.Filtration;
using EcsRx.Entities;
using EcsRx.Pools;

namespace EcsRx.Groups
{
    public class GroupAccessor : IGroupAccessor
    {
        public GroupAccessorToken AccessorToken { get; private set; }
        public IEnumerable<IEntity> Entities { get; private set; }

        public GroupAccessor(GroupAccessorToken accessorToken, IEnumerable<IEntity> entities)
        {
            AccessorToken = accessorToken;
            Entities = entities;
        }
    }
}