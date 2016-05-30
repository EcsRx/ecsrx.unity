using EcsRx.Components;
using EcsRx.Events;

namespace EcsRx.EventHandlers
{
    public delegate void PooledEntityHandler(object sender, PooledEntityEvent args);
}