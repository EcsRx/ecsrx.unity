using System;

namespace EcsRx.Persistence.Types
{
    public interface IComponentDescriptorRegistry
    {
        bool ContainsType(Type componentType);
        ComponentDataDescriptor GetDescriptorByType(Type componentType);
        ComponentDataDescriptor GetDescriptorFromName(string name);
    }
}