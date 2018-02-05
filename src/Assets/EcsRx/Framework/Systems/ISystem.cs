using EcsRx.Entities;
using EcsRx.Groups;

namespace EcsRx.Systems
{
    public interface ISystem
    {
        Group TargetGroup { get; }
    }
}