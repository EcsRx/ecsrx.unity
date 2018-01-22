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

        public Type GetInstanceType(InjectContext context)
        {
            return _instanceType;
        }

        public IEnumerator<List<object>> GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args)
        {
            Assert.IsEmpty(args);
            Assert.IsNotNull(context);

            Assert.That(_instanceType.DerivesFromOrEqual(context.MemberType));

            yield return new List<object>() { _instance };

            _container.OnInstanceResolved(_instance);
        }
    }
}
