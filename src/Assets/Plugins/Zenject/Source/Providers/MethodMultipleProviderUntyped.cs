using System;
using System.Collections.Generic;
using ModestTree;
using System.Linq;
using ModestTree.Util;

namespace Zenject
{
    public class MethodMultipleProviderUntyped : IProvider
    {
        readonly DiContainer _container;
        readonly Func<InjectContext, IEnumerable<object>> _method;

        public MethodMultipleProviderUntyped(
            Func<InjectContext, IEnumerable<object>> method,
            DiContainer container)
        {
            _container = container;
            _method = method;
        }

        public bool IsCached
        {
            get { return false; }
        }

        public bool TypeVariesBasedOnMemberType
        {
            get { return false; }
        }

        public Type GetInstanceType(InjectContext context)
        {
            return context.MemberType;
        }

        public List<object> GetAllInstancesWithInjectSplit(
            InjectContext context, List<TypeValuePair> args, out Action injectAction)
        {
            Assert.IsEmpty(args);
            Assert.IsNotNull(context);

            injectAction = null;
            if (_container.IsValidating && !TypeAnalyzer.ShouldAllowDuringValidation(context.MemberType))
            {
                return new List<object>() { new ValidationMarker(context.MemberType) };
            }
            else
            {
                var result = _method(context);

                if (result == null)
                {
                    throw Assert.CreateException(
                        "Method '{0}' returned null when list was expected. Object graph:\n {1}",
                        _method.ToDebugString(), context.GetObjectGraphString());
                }

                return result.ToList();
            }
        }
    }
}

