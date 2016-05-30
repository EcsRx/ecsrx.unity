using System.Collections.Generic;
using System.Linq;
using EcsRx.Entities;
using EcsRx.EventHandlers;
using EcsRx.Events;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Pools.Identifiers;

namespace EcsRx.Pools
{
    public class PoolManager : IPoolManager
    {
        public const string DefaultPoolName = "default";

        public event PooledEntityHandler OnEntityAdded;
        public event PooledEntityHandler OnEntityRemoved;
        public event EntityComponentHandler OnEntityComponentAdded;
        public event EntityComponentHandler OnEntityComponentRemoved;

        private IDictionary<GroupAccessorToken, IEnumerable<IEntity>> _groupAccessors;
        private IDictionary<string, IPool> _pools;
        
        public IEnumerable<IPool> Pools { get { return _pools.Values; } }
        public IIdentifyGenerator IdentityGenerator { get; private set; }

        public PoolManager(IIdentifyGenerator identityGenerator)
        {
            IdentityGenerator = identityGenerator;
            _groupAccessors = new Dictionary<GroupAccessorToken, IEnumerable<IEntity>>();
            _pools = new Dictionary<string, IPool>();
            CreatePool(DefaultPoolName);
        }

        public IPool CreatePool(string name)
        {
            var pool = new Pool(name, IdentityGenerator);
            _pools.Add(name, pool);
            pool.OnEntityAdded += EntityAddedToPool;
            pool.OnEntityRemoved += EntityRemovedFromPool;
            pool.OnEntityComponentAdded += EntityComponentAdded;
            pool.OnEntityComponentRemoved += EntityComponentRemoved;
            return pool;
        }

        public IPool GetPool(string name = null)
        { return _pools[name ?? DefaultPoolName]; }

        public void RemovePool(string name)
        {
            if(!_pools.ContainsKey(name)) { return; }

            var pool = _pools[name];
            _pools.Remove(name);

            pool.OnEntityAdded += EntityAddedToPool;
            pool.OnEntityRemoved += EntityRemovedFromPool;
            pool.OnEntityComponentAdded += EntityComponentAdded;
            pool.OnEntityComponentRemoved += EntityComponentRemoved;
        }

        private void EntityComponentAdded(object sender, EntityComponentEvent args)
        {
            if (OnEntityComponentAdded != null)
            { OnEntityComponentAdded(sender, args); }
        }

        private void EntityAddedToPool(object sender, EntityEvent args)
        {
            if(OnEntityAdded == null) { return; }
            var poolName = (sender as IPool).Name;
            OnEntityAdded(this, new PooledEntityEvent(poolName, args.Entity));
        }

        private void EntityComponentRemoved(object sender, EntityComponentEvent args)
        {
            if(OnEntityComponentRemoved != null)
            { OnEntityComponentRemoved(sender, args); }
        }

        private void EntityRemovedFromPool(object sender, EntityEvent args)
        {
            if (OnEntityRemoved == null) { return; }
            var poolName = (sender as IPool).Name;
            OnEntityRemoved(this, new PooledEntityEvent(poolName, args.Entity));
        }
        
        public IEnumerable<IEntity> GetEntitiesFor(IGroup group, string poolName = null)
        {
            if (poolName != null)
            { return _pools[poolName].Entities.MatchingGroup(group); }

            return Pools.GetAllEntities().MatchingGroup(group);
        }

        public GroupAccessor CreateGroupAccessor(IGroup group, string poolName = null)
        {
            var groupAccessorToken = new GroupAccessorToken(group.TargettedComponents.ToArray(), poolName);
            if (!_groupAccessors.ContainsKey(groupAccessorToken))
            {
                var entityMatches = GetEntitiesFor(group, poolName);
                _groupAccessors.Add(groupAccessorToken, entityMatches);
            }

            return new GroupAccessor(groupAccessorToken, _groupAccessors[groupAccessorToken]);
        }
    }
}