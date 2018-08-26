using System;
using System.Collections.Generic;
using ModestTree;
using System.Linq;

namespace Zenject
{
    public class CachedOpenTypeProvider : IProvider
    {
        readonly IProvider _creator;
        readonly Dictionary<Type, CachedProvider> _providerMap = new Dictionary<Type, CachedProvider>();

        public CachedOpenTypeProvider(IProvider creator)
        {
            Assert.That(creator.TypeVariesBasedOnMemberType);
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
            get { return _providerMap.Values.Select(x => x.NumInstances).Sum(); }
        }

        // This method can be called if you want to clear the memory for an AsSingle instance,
        // See isssue https://github.com/modesttree/Zenject/issues/441
        public void ClearCache()
        {
            _providerMap.Clear();
        }

        public Type GetInstanceType(InjectContext context)
        {
            return _creator.GetInstanceType(context);
        }

        public List<object> GetAllInstancesWithInjectSplit(
            InjectContext context, List<TypeValuePair> args, out Action injectAction)
        {
            Assert.IsNotNull(context);

            CachedProvider provider;

            if (!_providerMap.TryGetValue(context.MemberType, out provider))
            {
                provider = new CachedProvider(_creator);
                _providerMap.Add(context.MemberType, provider);
            }

            return provider.GetAllInstancesWithInjectSplit(
                context, args, out injectAction);
        }
    }
}

