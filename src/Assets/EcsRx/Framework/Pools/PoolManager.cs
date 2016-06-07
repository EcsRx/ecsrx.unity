using System.Collections.Generic;
using System.Linq;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Pools.Identifiers;
using UniRx;

namespace EcsRx.Pools
{
    public class PoolManager : IPoolManager
    {
        public const string DefaultPoolName = "default";
        
        private IDictionary<GroupAccessorToken, IEnumerable<IEntity>> _groupAccessors;
        private IDictionary<string, IPool> _pools;

        public IEventSystem EventSystem { get; private set; }
        public IEnumerable<IPool> Pools { get { return _pools.Values; } }
        public IIdentifyGenerator IdentityGenerator { get; private set; }

        public PoolManager(IIdentifyGenerator identityGenerator, IEventSystem eventSystem)
        {
            IdentityGenerator = identityGenerator;
            EventSystem = eventSystem;
            _groupAccessors = new Dictionary<GroupAccessorToken, IEnumerable<IEntity>>();
            _pools = new Dictionary<string, IPool>();
            CreatePool(DefaultPoolName);
        }
        
        public IPool CreatePool(string name)
        {
            var pool = new Pool(name, IdentityGenerator, EventSystem);
            _pools.Add(name, pool);

            EventSystem.Publish(new PoolAddedEvent(pool));

            return pool;
        }

        public IPool GetPool(string name = null)
        { return _pools[name ?? DefaultPoolName]; }

        public void RemovePool(string name)
        {
            if(!_pools.ContainsKey(name)) { return; }

            var pool = _pools[name];
            _pools.Remove(name);

            EventSystem.Publish(new PoolRemovedEvent(pool));
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