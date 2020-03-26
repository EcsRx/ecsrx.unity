using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using EcsRx.Collections;
using EcsRx.Groups;
using EcsRx.Groups.Observable;
using Zenject;

namespace EcsRx.Zenject.Extensions
{
    public static class ZenjectExtensions
    {
        public static IObservableGroup ResolveObservableGroup(this DiContainer container, IGroup group)
        {
            var observableGroupManager = container.Resolve<IObservableGroupManager>();
            return observableGroupManager.GetObservableGroup(group);
        }
        
        public static IObservableGroup ResolveObservableGroup(this DiContainer container, params Type[] componentTypes)
        {
            var observableGroupManager = container.Resolve<IObservableGroupManager>();
            var group = new Group(componentTypes);
            return observableGroupManager.GetObservableGroup(group);
        }
        
        public static IEnumerable ResolveAllOf(this DiContainer container, Type type)
        {
            return container.AllContracts
                .Where(bindingId => bindingId.Type == type)
                .Select(container.Resolve);
        }

        public static IEnumerable<T> ResolveAllOf<T>(this DiContainer container)
        { return container.ResolveAllOf(typeof(T)).Cast<T>(); }
    }
}