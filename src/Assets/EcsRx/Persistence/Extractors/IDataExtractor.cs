using EcsRx.Entities;
using EcsRx.Persistence.Data;

namespace EcsRx.Persistence.Extractors
{
    public interface IDataExtractor
    {
        EntityData Extract(IEntity entity);
    }
}