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

        public virtual void StartSystem(GroupAccessor @group)
        {
            _subscription = EventSystem.Receive<T>().Subscribe(EventTriggered);
        }

        public virtual void StopSystem(GroupAccessor @group)
        {
            _subscription.Dispose();
        }
        
        public abstract void EventTriggered(T eventData);
    }
}