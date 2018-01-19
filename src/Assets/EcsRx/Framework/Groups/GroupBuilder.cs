using System;
using System.Collections.Generic;
using System.Linq;
using EcsRx.Components;
using EcsRx.Entities;

namespace EcsRx.Groups
{
    public class GroupBuilder
    {
        private List<Type> _components;
        private Predicate<IEntity> _predicate;

        public GroupBuilder()
        {
            _components = new List<Type>();
        }

        public static GroupBuilder Create()
        {
            return new GroupBuilder
            {
                _components = new List<Type>()
            };
        }

        public static GroupBuilder Create(Group template)
        {
            return new GroupBuilder
            {
                _components = template.TargettedComponents.ToList(),
                _predicate = template.TargettedEntities
            };
        }

        public GroupBuilder WithComponent<T>() where T : class, IComponent
        {
            _components.Add(typeof(T));
            return this;
        }

        public GroupBuilder WithPredicate(Predicate<IEntity> predicate)
        {
            _predicate = predicate;
            return this;
        }

        public IGroup Build()
        { return new Group(_predicate, _components.ToArray()); }
    }
}