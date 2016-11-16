using System;
using System.Collections.Generic;
using System.Linq;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Extensions;
using EcsRx.Pools;
using UniRx;

namespace EcsRx.Groups
{
    public class CacheableGroupAccessor : IGroupAccessor, IDisposable
    {
        private readonly IList<IEntity> _cachedEntities;
        private readonly IList<IDisposable> _subscriptions;

        public GroupAccessorToken AccessorToken { get; private set; }
        public IEnumerable<IEntity> Entities { get { return _cachedEntities; } }
        public IEventSystem EventSystem { get; private set; }

        public CacheableGroupAccessor(GroupAccessorToken accessorToken, IEnumerable<IEntity> initialEntities, IEventSystem eventSystem)
        {
            AccessorToken = accessorToken;
            EventSystem = eventSystem;

            _cachedEntities = new List<IEntity>(initialEntities);
            _subscriptions = new List<IDisposable>();

            MonitorEntityChanges();
        }

        public void MonitorEntityChanges()
        {
            var addEntitySubscription = EventSystem.Receive<EntityAddedEvent>()
                .Subscribe(OnEntityAddedToPool);

            var removeEntitySubscription = EventSystem.Receive<EntityRemovedEvent>()
                .Where(x => _cachedEntities.Contains(x.Entity))
                .Subscribe(OnEntityRemovedFromPool);

            var addComponentSubscription = EventSystem.Receive<ComponentAddedEvent>()
                .Subscribe(OnEntityComponentAdded);

            var removeComponentEntitySubscription = EventSystem.Receive<ComponentRemovedEvent>()
                .Where(x => _cachedEntities.Contains(x.Entity))
                .Subscribe(OnEntityComponentRemoved);

            _subscriptions.Add(addEntitySubscription);
            _subscriptions.Add(removeEntitySubscription);
            _subscriptions.Add(addComponentSubscription);
            _subscriptions.Add(removeComponentEntitySubscription);
        }

        public void OnEntityComponentRemoved(ComponentRemovedEvent args)
        {
            if(!args.Entity.HasComponents(AccessorToken.ComponentTypes))
            { _cachedEntities.Remove(args.Entity); }
        }

        public void OnEntityComponentAdded(ComponentAddedEvent args)
        {
            if(args.Entity.HasComponents(AccessorToken.ComponentTypes) && !_cachedEntities.Contains(args.Entity))
            { _cachedEntities.Add(args.Entity); }
        }

        public void OnEntityAddedToPool(EntityAddedEvent args)
        {
            if (!args.Entity.Components.Any()) { return; }
            if (!args.Entity.HasComponents(AccessorToken.ComponentTypes)) { return; }
            _cachedEntities.Add(args.Entity);
        }

        public void OnEntityRemovedFromPool(EntityRemovedEvent args)
        {
            if(_cachedEntities.Contains(args.Entity))
            { _cachedEntities.Remove(args.Entity); }
        }

        public void Dispose()
        { _subscriptions.DisposeAll(); }
    }
}