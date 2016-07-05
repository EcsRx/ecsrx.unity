using System;
using System.Collections.Generic;
using UniRx;

namespace EcsRx.Events
{
    public class OptimisedEventSystem : IEventSystem
    {
        private readonly IDictionary<Type, IMessageBroker> _messageBrokers = new Dictionary<Type, IMessageBroker>();

        public void Publish<T>(T message)
        {
            var typeKey = typeof (T);
            if (!_messageBrokers.ContainsKey(typeKey))
            { _messageBrokers.Add(typeKey, new MessageBroker()); }

            _messageBrokers[typeKey].Publish(message);
        }

        public IObservable<T> Receive<T>()
        {
            var typeKey = typeof (T);
            if(!_messageBrokers.ContainsKey(typeKey))
            { _messageBrokers.Add(typeKey, new MessageBroker()); }

            return _messageBrokers[typeKey].Receive<T>();
        }
    }
}