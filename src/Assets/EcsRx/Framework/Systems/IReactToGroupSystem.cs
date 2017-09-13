using System;
using EcsRx.Entities;
using EcsRx.Groups.Accessors;
using EcsRx.Groups;
using UniRx;

namespace EcsRx.Systems
{
    public interface IReactToGroupSystem : ISystem
    {
        IObservable<IGroupAccessor> ReactToGroup(IGroupAccessor group);
        void Execute(IEntity entity);
    }
}