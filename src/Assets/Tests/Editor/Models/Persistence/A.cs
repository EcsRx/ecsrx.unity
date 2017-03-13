using EcsRx.Persistence.Attributes;

namespace Assets.Tests.Editor
{
    [Persist]
    public class A
    {
        [PersistData]
        public string TestValue { get; set; }

        [PersistData]
        public B NestedValue { get; set; }

        [PersistData]
        public B[] NestedArray { get; set; }
    }
}
