using EcsRx.Events;
using EcsRx.Pools.Identifiers;

namespace EcsRx.Entities
{
    public class DefaultEntityFactory : IEntityFactory
    {
        private readonly IEventSystem _eventSystem;
        private readonly IIdentityGenerator _identityGenerator;

        public DefaultEntityFactory(IIdentityGenerator identityGenerator, IEventSystem eventSystem)
        {
            _eventSystem = eventSystem;
            _identityGenerator = identityGenerator;
        }

        public IEntity Create(int? id = null)
        {
            if (!id.HasValue)
            { id = _identityGenerator.GenerateId(); }

            return new Entity(id.Value, _eventSystem);
        }
    }
}