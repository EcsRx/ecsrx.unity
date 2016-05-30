using System;
using System.Collections.Generic;
using System.Linq;
using EcsRx.Components;
using EcsRx.EventHandlers;
using EcsRx.Events;

namespace EcsRx.Entities
{
    public class Entity : IEntity
    {
        private readonly Dictionary<Type, IComponent> _components;

        public event ComponentHandler OnComponentAdded;
        public event ComponentHandler OnComponentRemoved;

        public int Id { get; private set; }
        public IEnumerable<IComponent> Components { get { return _components.Values; } }

        public Entity(int id)
        {
            Id = id;
            _components = new Dictionary<Type, IComponent>();
        }

        public void AddComponent(IComponent component)
        {
            _components.Add(component.GetType(), component);
            if(OnComponentAdded == null) { return; }
            OnComponentAdded(this, new ComponentEvent<IComponent>(component));
        }

        public void RemoveComponent(IComponent component)
        {
            if(!_components.ContainsKey(component.GetType())) { return; }

            _components.Remove(component.GetType());
            if (OnComponentRemoved == null) { return; }
            OnComponentRemoved(this, new ComponentEvent<IComponent>(component));
        }

        public void RemoveComponent<T>() where T : class, IComponent
        {
            if(!HasComponent<T>()) { return; }

            var component = GetComponent<T>();
            RemoveComponent(component);
        }

        public bool HasComponent<T>() where T : class, IComponent
        { return _components.ContainsKey(typeof(T)); }

        public bool HasComponents(params Type[] componentTypes)
        {
            if(_components.Count == 0)
            { return false; }

            return componentTypes.All(x => _components.ContainsKey(x));
        }

        public T GetComponent<T>() where T : class, IComponent
        { return _components[typeof(T)] as T; }
    }
}