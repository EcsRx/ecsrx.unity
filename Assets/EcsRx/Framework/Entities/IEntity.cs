using System;
using System.Collections.Generic;
using EcsRx.Components;
using EcsRx.EventHandlers;

namespace EcsRx.Entities
{
    public interface IEntity
    {
        event ComponentHandler OnComponentAdded;
        event ComponentHandler OnComponentRemoved;
        
        int Id { get; }
        IEnumerable<IComponent> Components { get; }

        void AddComponent(IComponent component);
        void RemoveComponent(IComponent component);
        void RemoveComponent<T>() where T : class, IComponent;
        T GetComponent<T>() where T : class, IComponent;

        bool HasComponent<T>() where T : class, IComponent;
        bool HasComponents(params Type[] component);
    }
}
