using System;
using System.Collections.Generic;
using System.Linq;
using EcsRx.Components;
using EcsRx.Events;
using EcsRx.Extensions;
using UniRx;

namespace EcsRx.Entities
{
    public class Entity : IEntity
    {
        private readonly Dictionary<Type, IComponent> _components;

        public IEventSystem EventSystem { get; private set; }

        public int Id { get; private set; }
        public IEnumerable<IComponent> Components { get { return _components.Values; } }

        public Entity(int id, IEventSystem eventSystem)
        {
            Id = id;
            EventSystem = eventSystem;
            _components = new Dictionary<Type, IComponent>();
        }

        public void AddComponent(IComponent component)
        {
            _components.Add(component.GetType(), component);
            EventSystem.Publish(new ComponentAddedEvent(this, component));
        }

        public void AddComponent<T>() where T : class, IComponent, new()
        { AddComponent(new T()); }

        public void RemoveComponent(IComponent component)
        {
            if(!_components.ContainsKey(component.GetType())) { return; }

            if(component is IDisposable)
            { (component as IDisposable).Dispose(); }

            _components.Remove(component.GetType());
            EventSystem.Publish(new ComponentRemovedEvent(this, component));
        }

        public void RemoveComponent<T>() where T : class, IComponent
        {
            if(!HasComponent<T>()) { return; }

            var component = GetComponent<T>();
            RemoveComponent(component);
        }

        public void RemoveAllComponents()
        {
            var components = Components.ToArray();
            components.ForEachRun(RemoveComponent);
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
