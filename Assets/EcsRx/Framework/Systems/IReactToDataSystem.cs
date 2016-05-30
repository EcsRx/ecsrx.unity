using EcsRx.Entities;
using EcsRx.Events;
using UniRx;

namespace EcsRx.Systems
{
    public interface IReactToDataSystem<T> : ISystem
    {
        IObservable<T> ReactToEntity(IEntity entity);

        void Execute(IEntity entity, T reactionData);
    }
}