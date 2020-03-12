using EcsRx.Blueprints;
using EcsRx.Entities;
using EcsRx.Examples.UsingBlueprints.Components;
using EcsRx.Extensions;

namespace EcsRx.Examples.UsingBlueprints.Blueprints
{
    public class PlayerBlueprint : IBlueprint
    {
        public float DefaultHealth { get; }
        public string Name { get; }

        public PlayerBlueprint(string name, float defaultHealth = 100.0f)
        {
            DefaultHealth = defaultHealth;
            Name = name;
        }

        public void Apply(IEntity entity)
        {
            entity.AddComponents(new HasName { Name = Name }, 
                new WithHealthComponent { CurrentHealth = DefaultHealth, MaxHealth = DefaultHealth});
        }
    }
}