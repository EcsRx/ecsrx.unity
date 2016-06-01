using System;
using EcsRx.Entities;

namespace EcsRx.Events
{
    public class EntityReactionEvent<T>
    {
        public IEntity Entity { get; private set; }
        public T ReactionData { get; private set; }

        public EntityReactionEvent(IEntity entity)
        {
            Entity = entity;
        }

        public EntityReactionEvent(IEntity entity, T reactionData)
        {
            Entity = entity;
            ReactionData = reactionData;
        }
    }
}