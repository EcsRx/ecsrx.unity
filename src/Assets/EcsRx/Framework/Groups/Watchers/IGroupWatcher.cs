using System;
using EcsRx.Entities;
using UniRx;

namespace EcsRx.Groups.Watchers
{
    public interface IGroupWatcher
    {
        Type[] ComponentTypes { get; }

        Subject<IEntity> OnEntityAdded { get; }
        Subject<IEntity> OnEntityRemoved { get; }
    }
}