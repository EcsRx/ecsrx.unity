using System;
using EcsRx.Components;

namespace EcsRx.Unity.MonoBehaviours.Editor.Events
{
    public class ComponentEvent : EventArgs
    {
        public IComponent Component { get; set; }

        public ComponentEvent(IComponent component)
        {
            Component = component;
        }
    }

    public delegate void ComponentEventHandler(object sender, ComponentEvent eventArgs);
}