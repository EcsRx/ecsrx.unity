using Persistity.Attributes;

namespace EcsRx.Tests.Components
{
    [Persist]
    public class StatsComponent
    {
        [PersistData]
        public float Health { get; set; }

        [PersistData]
        public float Magic { get; set; }
    }
}