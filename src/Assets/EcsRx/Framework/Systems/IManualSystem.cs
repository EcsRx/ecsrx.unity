using EcsRx.Groups;

namespace EcsRx.Systems
{
    public interface IManualSystem : ISystem
    {
        void StartSystem(GroupAccessor group);
        void StopSystem(GroupAccessor group);
    }
}