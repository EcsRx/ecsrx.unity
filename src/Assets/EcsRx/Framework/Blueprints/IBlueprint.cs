using EcsRx.Entities;

namespace Assets.EcsRx.Framework.Blueprints
{
    public interface IBlueprint
    {
        void Apply(IEntity entity);
    }
}