using EcsRx.Attributes;
using EcsRx.Groups;
using EcsRx.Systems;

namespace EcsRx.Tests.Systems
{
    [Priority(-100)]
    public class LowPrioritySystem : ISystem
    {
        public Group TargetGroup { get { return null; } }
    }
}