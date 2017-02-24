using System;
using System.Collections.Generic;

namespace EcsRx.Persistence.Types
{
    public interface IComponentDescriptorRegistry
    {
        IDictionary<Type, ComponentDataDescriptor> AllComponentDescriptors { get; }
        ComponentDataDescriptor GetDescriptorFromName(string name);
    }
}