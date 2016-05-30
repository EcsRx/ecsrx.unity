using EcsRx.Entities;
using EcsRx.Groups;
using UniRx;

namespace EcsRx.Systems
{
    public interface IReactToGroupSystem : ISystem
    {
        IObservable<GroupAccessor> ReactToGroup(GroupAccessor group);
        void Execute(IEntity entity);
    }
}