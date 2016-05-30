using EcsRx.Components;
using EcsRx.Events;

namespace EcsRx.EventHandlers
{
    public delegate void EntityHandler(object sender, EntityEvent args);
}