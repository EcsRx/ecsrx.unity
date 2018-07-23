using System;
using System.Collections.Generic;
using ModestTree;

namespace Zenject
{
    public class CachedProvider : IProvider
    {
        readonly IProvider _creator;

        List<object> _instances;

#if !ZEN_MULTITHREADING
        bool _isCreatingInstance;
#endif

        public CachedProvider(IProvider creator)
        {
            _creator = creator;
        }

        public bool IsCached
        {
            get { return true; }
        }

        public bool TypeVariesBasedOnMemberType
        {
            get
            {
                // Should not call this
                throw Assert.CreateException();
            }
        }

        public int NumInstances
        {
            get { return _instances == null ? 0 : _instances.Count; }
        }

        // This method can be called if you want to clear the memory for an AsSingle instance,
        // See isssue https://github.com/modesttree/Zenject/issues/441
        public void ClearCache()
        {
            _instances = null;
        }

        public Type GetInstanceType(InjectContext context)
        {
            return _creator.GetInstanceType(context);
        }

        public List<object> GetAllInstancesWithInjectSplit(
            InjectContext context, List<TypeValuePair> args, out Action injectAction)
        {
            Assert.IsNotNull(context);

            if (_instances != null)
            {
                injectAction = null;
                return _instances;
            }

#if !ZEN_MULTITHREADING
            // This should only happen with constructor injection
            // Field or property injection should allow circular dependencies
            if (_isCreatingInstance)
            {
                var instanceType = _creator.GetInstanceType(context);
                throw Assert.CreateException(
                    "Found circular dependency when creating type '{0}'. Object graph:\n {1}{2}\n",
                    instanceType, context.GetObjectGraphString(), instanceType.PrettyName());
            }

            _isCreatingInstance = true;
#endif

            _instances = _creator.GetAllInstancesWithInjectSplit(context, args, out injectAction);
            Assert.IsNotNull(_instances);

#if !ZEN_MULTITHREADING
            _isCreatingInstance = false;
#endif
            return _instances;
        }
    }
}
