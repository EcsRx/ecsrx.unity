using UniRx;

namespace EcsRx.Events
{
    public class EventSystem : IEventSystem
    {
        public IMessageBroker MessageBroker { get; private set; }

        public EventSystem(IMessageBroker messageBroker)
        { MessageBroker = messageBroker; }

        public void Publish<T>(T message)
        { MessageBroker.Publish(message); }

        public IObservable<T> Receive<T>()
        { return MessageBroker.Receive<T>(); }

    }
}