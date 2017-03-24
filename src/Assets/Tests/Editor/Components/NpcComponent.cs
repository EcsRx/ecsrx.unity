using Persistity.Attributes;

namespace EcsRx.Tests.Components
{
    [Persist]
    public class NpcComponent
    {
        [PersistData]
        public string Name { get; set; }
    }
}