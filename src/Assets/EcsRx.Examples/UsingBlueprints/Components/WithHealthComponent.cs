using EcsRx.Components;

namespace Assets.EcsRx.Examples.UsingBlueprints.Components
{
    public class WithHealthComponent : IComponent
    {
        public float MaxHealth { get; set; }
        public float CurrentHealth { get; set; }
    }
}