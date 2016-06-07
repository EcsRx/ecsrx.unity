using EcsRx.Components;

namespace Assets.Examples.ViewBinding.Components
{
    public class SphereComponent : IComponent
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public float Roundness { get; set; }
        // public UnityEngine.Vector3 Position { get; set; }
    }
}
