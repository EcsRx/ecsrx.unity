using System;
using System.Collections.Generic;
using ModestTree;
using System.Linq;
using ModestTree.Util;

namespace Zenject
{
    public class MethodProviderMultiple<TReturn> : IProvider
    {
        readonly DiContainer _container;
        readonly Func<InjectContext, IEnumerable<TReturn>> _method;

        public MethodProviderMultiple(
            Func<InjectContext, IEnumerable<TReturn>> method,
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
            return typeof(TReturn);
        }

        public List<object> GetAllInstancesWithInjectSplit(
            InjectContext context, List<TypeValuePair> args, out Action injectAction)
        {
            Assert.IsEmpty(args);
            Assert.IsNotNull(context);

            Assert.That(typeof(TReturn).DerivesFromOrEqual(context.MemberType));

            injectAction = null;
            if (_container.IsValidating && !TypeAnalyzer.ShouldAllowDuringValidation(context.MemberType))
            {
                return new List<object>() { new ValidationMarker(typeof(TReturn)) };
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

                return result.Cast<object>().ToList();
            }
        }
    }
}
