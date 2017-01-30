using System.Collections.Generic;
using Assets.EcsRx.Framework.Groups;
using EcsRx.Entities;
using EcsRx.Groups;

namespace EcsRx.Extensions
{
    public static class IGroupAccessorExtensions
    {
        public static IEnumerable<IEntity> Query(this IGroupAccessor groupAccesssor, IGroupAccessorQuery query)
        { return query.Execute(groupAccesssor); }
    }
}