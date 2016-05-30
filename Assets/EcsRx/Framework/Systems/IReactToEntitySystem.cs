using EcsRx.Entities;
using UniRx;

namespace EcsRx.Systems
{
    public interface IReactToEntitySystem : ISystem
    {
        IObservable<IEntity> ReactToEntity(IEntity entity);

        void Execute(IEntity entity);
    }
}