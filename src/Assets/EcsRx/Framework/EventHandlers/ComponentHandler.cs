using EcsRx.Components;
using EcsRx.Events;

namespace EcsRx.EventHandlers
{
    public delegate void ComponentHandler(object sender, ComponentEvent<IComponent> args);
}