using System;
using System.Collections.Generic;
using Assets.EcsRx.Examples.GroupFilters.Components;
using EcsRx.Groups;

namespace Assets.EcsRx.Examples.GroupFilters.Groups
{
    public class HasScoreGroup : Group
    {
        public IEnumerable<Type> TargettedComponents
        {
            get
            {
                return new[] { typeof (HasScoreComponent) };
            }
        }
    }
}