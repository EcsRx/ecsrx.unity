using System;
using System.Collections.Generic;
using EcsRx.Groups;
using UniRx;

namespace Assets.EcsRx.Framework.Groups.Filtration
{
    public abstract class CacheableGroupAccessorFilter<T> : IGroupAccessorFilter<T>, IDisposable
    {
        private readonly IDisposable _triggerSubscription;

        protected IEnumerable<T> FilteredCache { get; set; }
        protected abstract IObservable<Unit> TriggerOnChange();
        protected abstract IEnumerable<T> FilterQuery();

        public IGroupAccessor GroupAccessor { get; private set; }

        protected CacheableGroupAccessorFilter(IGroupAccessor groupAccessor)
        {
            GroupAccessor = groupAccessor;
            _triggerSubscription = TriggerOnChange().Subscribe(x => UpdateCache());
        }

        private void UpdateCache()
        { FilteredCache = FilterQuery(); }

        public IEnumerable<T> Filter()
        {
            if (FilteredCache != null)
            { return FilteredCache; }

            UpdateCache();
            return FilteredCache;
        }

        public void Dispose()
        { _triggerSubscription.Dispose(); }
    }
}