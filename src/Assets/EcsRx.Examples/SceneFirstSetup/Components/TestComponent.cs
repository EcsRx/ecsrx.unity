using EcsRx.Components;

namespace Assets.EcsRx.Examples.SceneFirstSetup.Components
{
    public class TestComponent : IComponent
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public bool IsHappy { get; set; }
        public float Roundness { get; set; }
    }
}