using EcsRx.Entities;
using EcsRx.Persistence.Data;

namespace EcsRx.Persistence.Extractors
{
    public interface IEntityConvertor
    {
        EntityData ConvertToData(IEntity entity);
        IEntity ConvertFromData(EntityData entityData);
    }
}