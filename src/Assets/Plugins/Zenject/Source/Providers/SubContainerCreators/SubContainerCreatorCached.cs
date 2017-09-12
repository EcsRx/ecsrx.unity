using System.Collections.Generic;
using ModestTree;

namespace Zenject
{
    public class SubContainerCreatorCached : ISubContainerCreator
    {
        readonly ISubContainerCreator _subCreator;
        bool _isLookingUp;
        DiContainer _subContainer;

        public SubContainerCreatorCached(ISubContainerCreator subCreator)
        {
            _subCreator = subCreator;
        }

        public DiContainer CreateSubContainer(List<TypeValuePair> args, InjectContext context)
        {
            // We can't really support arguments if we are using the cached value since
            // the arguments might change when called after the first time
            Assert.IsEmpty(args);

            if (_subContainer == null)
            {
                Assert.That(!_isLookingUp,
                    "Found unresolvable circular dependency when looking up sub container!  Object graph: {0}", context.GetObjectGraphString());
                _isLookingUp = true;
                _subContainer = _subCreator.CreateSubContainer(new List<TypeValuePair>(), context);
                _isLookingUp = false;
                Assert.IsNotNull(_subContainer);
            }

            return _subContainer;
        }
    }
}
