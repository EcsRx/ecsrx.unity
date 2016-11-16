using System;
using System.Linq;
using EcsRx.Blueprints;
using EcsRx.Entities;
using EcsRx.Groups;
using UniRx;

namespace EcsRx.Extensions
{
    public static class IEntityExtensions
    {
        public static IObservable<IEntity> ObserveProperty<T>(this IEntity entity, Func<IEntity, T> propertyLocator)
        {
            return Observable.EveryUpdate()
                .DistinctUntilChanged(x => propertyLocator(entity))
                .Select(x => entity);
        }

        public static IObservable<IEntity> WaitForPredicateMet(this IEntity entity, Predicate<IEntity> predicate)
        {
            return Observable.EveryUpdate()
                .First(x => predicate(entity))
                .Select(x => entity);
        }

        public static bool MatchesGroup(this IEntity entity, IGroup group)
        { return entity.HasComponents(group.TargettedComponents.ToArray()); }

        public static IEntity ApplyBlueprint(this IEntity entity, IBlueprint blueprint)
        {
            blueprint.Apply(entity);
            return entity;
        }
    }
}