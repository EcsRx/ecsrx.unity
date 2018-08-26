using System;
using System.Collections.Generic;
using ModestTree;

namespace Zenject
{
    public class MethodProviderUntyped : IProvider
    {
        readonly DiContainer _container;
        readonly Func<InjectContext, object> _method;

        public MethodProviderUntyped(
            Func<InjectContext, object> method,
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
                    Assert.That(!context.MemberType.IsPrimitive(),
                        "Invalid value returned from FromMethod.  Expected non-null.");
                }
                else
                {
                    Assert.That(result.GetType().DerivesFromOrEqual(context.MemberType));
                }

                return new List<object>() { result };
            }
        }
    }
}

