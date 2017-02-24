using System;
using System.Collections.Generic;

namespace EcsRx.Persistence.Types
{
    public interface IComponentTypeRegistry
    {
        IList<Type> AllComponentTypes { get; }
        Type GetComponentFromName(string name);
    }
}