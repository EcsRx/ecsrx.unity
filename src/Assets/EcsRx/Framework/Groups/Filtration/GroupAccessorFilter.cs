using System;
using System.Collections.Generic;
using EcsRx.Entities;
using EcsRx.Groups;

namespace Assets.EcsRx.Framework.Groups.Filtration
{
    public interface IGroupAccessorFilter
    {
        IGroupAccessor GroupAccessor { get; }
        IEnumerable<IEntity> Filter();
    }

    public interface IGroupAccessorFilter<T>
    {
        IGroupAccessor GroupAccessor { get; }
        IEnumerable<T> Filter();
    }

    public interface IGroupAccessorFilter<TOutput, TInput>
    {
        IGroupAccessor GroupAccessor { get; }
        IEnumerable<TOutput> Filter(TInput input);
    }
}