using System.Collections.Generic;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Pools.Identifiers;
using UniRx;

namespace EcsRx.Pools
{
    public class Pool : IPool
    {
        private readonly IList<IEntity> _entities;
        
        public string Name { get; private set; }
        public IEnumerable<IEntity> Entities { get { return _entities;} }
        public IIdentifyGenerator IdentityGenerator { get; private set; }
        public IMessageBroker MessageBroker { get; private set; }

        public Pool(string name, IIdentifyGenerator identityGenerator, IMessageBroker messageBroker)
        {
            _entities = new List<IEntity>();
            Name = name;
            IdentityGenerator = identityGenerator;
            MessageBroker = messageBroker;
        }

        public IEntity CreateEntity()
        {
            var newId = IdentityGenerator.GenerateId();
            var entity = new Entity(newId, MessageBroker);
            _entities.Add(entity);
            
            MessageBroker.Publish(new EntityAddedEvent(entity, this));

            return entity;
        }

        public void RemoveEntity(IEntity entity)
        {
            _entities.Remove(entity);

            MessageBroker.Publish(new EntityRemovedEvent(entity, this));
        }
    }
}