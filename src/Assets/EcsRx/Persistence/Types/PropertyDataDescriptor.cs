using System;
using EcsRx.Components;

namespace EcsRx.Persistence.Types
{
    public class PropertyDataDescriptor
    {
        public Type DataType { get; set; }
        public Func<IComponent, object> GetValue { get; set; }
        public Action<IComponent, object> SetValue { get; set; }
    }
}