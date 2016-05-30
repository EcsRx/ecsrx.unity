using System.Collections.Generic;
using System.Linq;
using EcsRx.Components;
using EcsRx.Entities;
using EcsRx.EventHandlers;
using EcsRx.Events;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Pools.Identifiers;

namespace EcsRx.Pools
{
    public class Pool : IPool
    {
        private readonly IList<IEntity> _entities;

        public event EntityHandler OnEntityAdded;
        public event EntityHandler OnEntityRemoved;
        public event EntityComponentHandler OnEntityComponentAdded;
        public event EntityComponentHandler OnEntityComponentRemoved;
        
        public string Name { get; private set; }
        public IEnumerable<IEntity> Entities { get { return _entities;} }
        public IIdentifyGenerator IdentityGenerator { get; private set; }

        public Pool(string name, IIdentifyGenerator identityGenerator)
        {
            _entities = new List<IEntity>();
            Name = name;
            IdentityGenerator = identityGenerator;
        }

        private void EntityComponentRemoved(object sender, ComponentEvent<IComponent> args)
        {
            if(OnEntityComponentRemoved != null)
            { OnEntityComponentRemoved(this, new EntityComponentEvent(sender as IEntity, args.Component));}
        }

        private void EntityComponentAdded(object sender, ComponentEvent<IComponent> args)
        {
            if(OnEntityComponentAdded != null)
            { OnEntityComponentAdded(this, new EntityComponentEvent(sender as IEntity, args.Component)); }
        }

        public IEntity CreateEntity()
        {
            var newId = IdentityGenerator.GenerateId();
            var entity = new Entity(newId);
            _entities.Add(entity);

            entity.OnComponentAdded += EntityComponentAdded;
            entity.OnComponentRemoved += EntityComponentRemoved; ;

            if(OnEntityAdded != null)
            { OnEntityAdded(this, new EntityEvent(entity)); }

            return entity;
        }

        public void RemoveEntity(IEntity entity)
        {
            _entities.Remove(entity);

            entity.OnComponentAdded -= EntityComponentAdded;
            entity.OnComponentRemoved -= EntityComponentRemoved;

            if (OnEntityRemoved == null) { return; }
            OnEntityRemoved(this, new EntityEvent(entity));
        }
    }
}