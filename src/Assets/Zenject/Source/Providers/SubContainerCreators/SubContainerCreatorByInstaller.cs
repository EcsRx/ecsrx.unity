using System;
using System.Collections.Generic;
using ModestTree;

namespace Zenject
{
    public class SubContainerCreatorByInstaller : ISubContainerCreator
    {
        readonly Type _installerType;
        readonly DiContainer _container;

        public SubContainerCreatorByInstaller(
            DiContainer container, Type installerType)
        {
            _installerType = installerType;
            _container = container;
        }

        public DiContainer CreateSubContainer(List<TypeValuePair> args)
        {
            var subContainer = _container.CreateSubContainer();

            subContainer.InstallExplicit(_installerType, args);

            return subContainer;
        }
    }
}
