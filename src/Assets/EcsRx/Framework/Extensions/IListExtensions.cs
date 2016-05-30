using System.Collections.Generic;
using System.Linq;
using EcsRx.Entities;
using EcsRx.Systems.Executor;

namespace EcsRx.Extensions
{
    public static class IListExtensions
    {
        public static IEnumerable<SubscriptionToken> GetTokensFor(this IList<SubscriptionToken> subscriptionTokens, IEntity entity)
        { return subscriptionTokens.Where(x => x.AssociatedObject == entity); }
    }
}