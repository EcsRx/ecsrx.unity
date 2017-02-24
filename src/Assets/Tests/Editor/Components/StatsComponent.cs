using EcsRx.Persistence.Attributes;

namespace EcsRx.Tests.Components
{
    [Persist]
    public class StatsComponent
    {
        [PersistData]
        public float Health { get; set; }   
    }
}