using System;
using System.Collections.Generic;
using ModestTree;

namespace Zenject
{
    public class InstanceProvider : IProvider
    {
        readonly object _instance;
        readonly Type _instanceType;
        readonly DiContainer _container;

        public InstanceProvider(
            Type instanceType, object instance, DiContainer container)
        {
            _instanceType = instanceType;
            _instance = instance;
            _container = container;
        }

        public bool IsCached
        {
            get { return true; }
        }

        public bool TypeVariesBasedOnMemberType
        {
            get { return false; }
        }

        public Type GetInstanceType(InjectContext context)
        {
            return _instanceType;
        }

        public List<object> GetAllInstancesWithInjectSplit(
            InjectContext context, List<TypeValuePair> args, out Action injectAction)
        {
            Assert.IsEmpty(args);
            Assert.IsNotNull(context);

            Assert.That(_instanceType.DerivesFromOrEqual(context.MemberType));

            injectAction = () => _container.LazyInject(_instance);
            return new List<object>() { _instance };
        }
    }
}
