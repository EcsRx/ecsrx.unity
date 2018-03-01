using EcsRx.Components;

namespace EcsRx.Unity.Examples.UsingBlueprints.Components
{
    public class WithHealthComponent : IComponent
    {
        public float MaxHealth { get; set; }
        public float CurrentHealth { get; set; }
    }
}