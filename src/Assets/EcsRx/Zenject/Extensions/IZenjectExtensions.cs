using System;
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
            var collectionManager = container.Resolve<IEntityCollectionManager>();
            return collectionManager.GetObservableGroup(group);
        }
        
        public static IObservableGroup ResolveObservableGroup(this DiContainer container, params Type[] componentTypes)
        {
            var collectionManager = container.Resolve<IEntityCollectionManager>();
            var group = new Group(componentTypes);
            return collectionManager.GetObservableGroup(group);
        }
    }
}