using System;
using System.Collections.Generic;
using EcsRx.Blueprints;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Framework.Exceptions;

namespace EcsRx.Pools
{
    public class Pool : IPool
    {
        private readonly IList<IEntity> _entities;

        public string Name { get; private set; }
        public IEnumerable<IEntity> Entities { get { return _entities;} }
        public IEventSystem EventSystem { get; private set; }
        public IEntityFactory EntityFactory { get; private set; }

        public Pool(string name, IEntityFactory entityFactory, IEventSystem eventSystem)
        {
            _entities = new List<IEntity>();
            Name = name;
            EventSystem = eventSystem;
            EntityFactory = entityFactory;
        }

        public IEntity CreateEntity(IBlueprint blueprint = null)
        {
            var entity = EntityFactory.Create(null);

            _entities.Add(entity);

            EventSystem.Publish(new EntityAddedEvent(entity, this));

            if (blueprint != null)
            { blueprint.Apply(entity); }

            return entity;
        }

        public void RemoveEntity(IEntity entity)
        {
            _entities.Remove(entity);

            entity.Dispose();

            EventSystem.Publish(new EntityRemovedEvent(entity, this));
        }

        public void AddEntity(IEntity entity)
        {
            if(entity.Id == Guid.Empty)
            { throw new InvalidEntityException("Entity provided does not have an assigned Id"); }

            _entities.Add(entity);
            EventSystem.Publish(new EntityAddedEvent(entity, this));
        }
    }
}
