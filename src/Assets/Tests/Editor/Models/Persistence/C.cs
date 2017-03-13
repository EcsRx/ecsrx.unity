using EcsRx.Persistence.Attributes;

namespace Assets.Tests.Editor
{
    [Persist]
    public class C
    {
        [PersistData]
        public float FloatValue { get; set; }
    }
}