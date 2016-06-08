using EcsRx.Components;

namespace Assets.EcsRx.Examples.ViewBinding.Components
{
    public class CubeComponent : IComponent
    {
        public string Name { get; set; }
        public int Age { get; set; }
				public bool IsHappy { get; set; }
    }
}
