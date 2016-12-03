using EcsRx.Factories;

namespace EcsRx.Entities
{
    public interface IEntityFactory : IFactory<int?, IEntity> {}
}