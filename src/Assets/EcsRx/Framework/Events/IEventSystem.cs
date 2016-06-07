using UniRx;

namespace EcsRx.Events
{
    public interface IEventSystem
    {
        void Publish<T>(T message);
        IObservable<T> Receive<T>();
    }
}