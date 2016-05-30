using EcsRx.Components;
using EcsRx.Events;

namespace EcsRx.EventHandlers
{
    public delegate void EntityComponentHandler(object sender, EntityComponentEvent args);
}