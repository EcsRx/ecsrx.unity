using System;
using EcsRx.Events;
using EcsRx.Groups;
using UniRx;

namespace EcsRx.Systems.Custom
{
    public abstract class EventReactionSystem<T> : IManualSystem
    {
        public virtual IGroup TargetGroup { get { return new EmptyGroup();} }
        public IEventSystem EventSystem { get; private set; }

        private IDisposable _subscription;

        protected EventReactionSystem(IEventSystem eventSystem)
        {
            EventSystem = eventSystem;
        }

        public virtual void StartSystem(IGroupAccessor @group)
        {
            _subscription = EventSystem.Receive<T>().Subscribe(EventTriggered);
        }

        public virtual void StopSystem(IGroupAccessor @group)
        {
            _subscription.Dispose();
        }
        
        public abstract void EventTriggered(T eventData);
    }
}