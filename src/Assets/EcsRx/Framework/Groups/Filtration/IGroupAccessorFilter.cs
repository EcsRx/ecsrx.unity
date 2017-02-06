using System;
using System.Collections.Generic;
using EcsRx.Entities;
using EcsRx.Groups;

namespace Assets.EcsRx.Framework.Groups.Filtration
{
    public interface IGroupAccessorFilter
    {
        IGroupAccessor GroupAccessor { get; }
    }

    public interface IGroupAccessorFilter<T> : IGroupAccessorFilter
    {
        IEnumerable<T> Filter();
    }

    public interface IGroupAccessorFilter<TOutput, TInput> : IGroupAccessorFilter
    {
        IEnumerable<TOutput> Filter(TInput input);
    }
}