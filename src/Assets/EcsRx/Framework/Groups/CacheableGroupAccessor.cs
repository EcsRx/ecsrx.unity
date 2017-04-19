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
        public readonly IDictionary<Guid, IEntity> CachedEntities;
        public readonly IList<IDisposable> Subscriptions;

        public GroupAccessorToken AccessorToken { get; private set; }
        public IEnumerable<IEntity> Entities { get { return CachedEntities.Values; } }
        public IEventSystem EventSystem { get; private set; }

        public CacheableGroupAccessor(GroupAccessorToken accessorToken, IEnumerable<IEntity> initialEntities, IEventSystem eventSystem)
        {
            AccessorToken = accessorToken;
            EventSystem = eventSystem;

            CachedEntities = initialEntities.ToDictionary(x => x.Id, x => x);
            Subscriptions = new List<IDisposable>();
        }

        public void MonitorEntityChanges()
        {
            var addEntitySubscription = EventSystem.Receive<EntityAddedEvent>()
                .Subscribe(OnEntityAddedToPool);

            var removeEntitySubscription = EventSystem.Receive<EntityRemovedEvent>()
                .Where(x => CachedEntities.ContainsKey(x.Entity.Id))
                .Subscribe(OnEntityRemovedFromPool);

            var addComponentSubscription = EventSystem.Receive<ComponentAddedEvent>()
                .Subscribe(OnEntityComponentAdded);

            var removeComponentEntitySubscription = EventSystem.Receive<ComponentRemovedEvent>()
                .Where(x => CachedEntities.ContainsKey(x.Entity.Id))
                .Subscribe(OnEntityComponentRemoved);

            Subscriptions.Add(addEntitySubscription);
            Subscriptions.Add(removeEntitySubscription);
            Subscriptions.Add(addComponentSubscription);
            Subscriptions.Add(removeComponentEntitySubscription);
        }

        public void OnEntityComponentRemoved(ComponentRemovedEvent args)
        {
            if(!args.Entity.HasComponents(AccessorToken.ComponentTypes))
            { CachedEntities.Remove(args.Entity.Id); }
        }

        public void OnEntityComponentAdded(ComponentAddedEvent args)
        {
            if(CachedEntities.ContainsKey(args.Entity.Id)) { return; }

            if(args.Entity.HasComponents(AccessorToken.ComponentTypes))
            { CachedEntities.Add(args.Entity.Id, args.Entity); }
        }

        public void OnEntityAddedToPool(EntityAddedEvent args)
        {
            if (!args.Entity.Components.Any()) { return; }
            if (!args.Entity.HasComponents(AccessorToken.ComponentTypes)) { return; }
            CachedEntities.Add(args.Entity.Id, args.Entity);
        }

        public void OnEntityRemovedFromPool(EntityRemovedEvent args)
        {
            if(CachedEntities.ContainsKey(args.Entity.Id))
            { CachedEntities.Remove(args.Entity.Id); }
        }

        public void Dispose()
        { Subscriptions.DisposeAll(); }
    }
}