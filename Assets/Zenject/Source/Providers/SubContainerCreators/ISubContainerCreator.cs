using System;
using System.Collections.Generic;

namespace Zenject
{
    public interface ISubContainerCreator
    {
        DiContainer CreateSubContainer(List<TypeValuePair> args);
    }
}
