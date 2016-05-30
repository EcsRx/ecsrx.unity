using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;

namespace Zenject
{
    public class CachedProvider : IProvider
    {
        readonly IProvider _creator;

        List<object> _instances;
        bool _isCreatingInstance;

        public CachedProvider(IProvider creator)
        {
            _creator = creator;
        }

        public Type GetInstanceType(InjectContext context)
        {
            return _creator.GetInstanceType(context);
        }

        public IEnumerator<List<object>> GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args)
        {
            Assert.IsNotNull(context);

            if (_instances != null)
            {
                yield return _instances;
                yield break;
            }

            // This should only happen with constructor injection
            // Field or property injection should allow circular dependencies
            Assert.That(!_isCreatingInstance,
            "Found circular dependency when creating type '{0}'",
            _creator.GetInstanceType(context));

            _isCreatingInstance = true;

            var runner = _creator.GetAllInstancesWithInjectSplit(context, args);

            // First get instance
            bool hasMore = runner.MoveNext();

            _instances = runner.Current;
            Assert.IsNotNull(_instances);
            _isCreatingInstance = false;

            yield return _instances;

            // Now do injection
            while (hasMore)
            {
                hasMore = runner.MoveNext();
            }
        }
    }
}
