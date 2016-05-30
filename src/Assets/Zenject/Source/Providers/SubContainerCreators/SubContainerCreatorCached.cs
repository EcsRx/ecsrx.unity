using System;
using System.Collections.Generic;
using ModestTree;

namespace Zenject
{
    public class SubContainerCreatorCached : ISubContainerCreator
    {
        readonly ISubContainerCreator _subCreator;

        DiContainer _subContainer;

        public SubContainerCreatorCached(ISubContainerCreator subCreator)
        {
            _subCreator = subCreator;
        }

        public DiContainer CreateSubContainer(List<TypeValuePair> args)
        {
            // We can't really support arguments if we are using the cached value since
            // the arguments might change when called after the first time
            Assert.IsEmpty(args);

            if (_subContainer == null)
            {
                _subContainer = _subCreator.CreateSubContainer(new List<TypeValuePair>());
                Assert.IsNotNull(_subContainer);
            }

            return _subContainer;
        }
    }
}
