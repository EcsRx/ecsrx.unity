using EcsRx.Components;

namespace EcsRx.Examples.DataPipeline.Components
{
    public class DataComponent : IComponent
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}