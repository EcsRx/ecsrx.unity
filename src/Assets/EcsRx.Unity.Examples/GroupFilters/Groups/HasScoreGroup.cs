using System;
using System.Collections.Generic;
using EcsRx.Groups;
using EcsRx.Unity.Examples.GroupFilters.Components;

namespace EcsRx.Unity.Examples.GroupFilters.Groups
{
    public class HasScoreGroup : IGroup
    {
        public IEnumerable<Type> MatchesComponents => new[] { typeof(HasScoreComponent) };
    }
}