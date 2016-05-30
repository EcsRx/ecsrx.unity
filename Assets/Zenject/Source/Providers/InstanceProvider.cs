using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using Zenject;

namespace Zenject
{
    public class InstanceProvider : IProvider
    {
        readonly object _instance;
        readonly Type _instanceType;

        public InstanceProvider(
            Type instanceType, object instance)
        {
            _instanceType = instanceType;
            _instance = instance;
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
        }
    }
}
