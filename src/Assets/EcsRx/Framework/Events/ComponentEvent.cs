using System;
using EcsRx.Components;

namespace EcsRx.Events
{
    public class ComponentEvent<T> : EventArgs where T : class, IComponent
    {
        public T Component { get; set; }

        public ComponentEvent(T component)
        {
            Component = component;
        }
    }
}