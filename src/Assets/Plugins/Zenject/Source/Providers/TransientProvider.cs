using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;

namespace Zenject
{
    public class TransientProvider : IProvider
    {
        readonly DiContainer _container;
        readonly Type _concreteType;
        readonly List<TypeValuePair> _extraArguments;
        readonly object _concreteIdentifier;

        public TransientProvider(
            Type concreteType, DiContainer container,
            List<TypeValuePair> extraArguments, string bindingContext, object concreteIdentifier)
        {
            Assert.That(!concreteType.IsAbstract(),
                "Expected non-abstract type for given binding but instead found type '{0}'{1}",
                concreteType, bindingContext == null ? "" : " when binding '{0}'".Fmt(bindingContext) );

            _container = container;
            _concreteType = concreteType;
            _extraArguments = extraArguments ?? new List<TypeValuePair>();
            _concreteIdentifier = concreteIdentifier;
        }

        public bool IsCached
        {
            get { return false; }
        }

        public bool TypeVariesBasedOnMemberType
        {
            get { return _concreteType.IsOpenGenericType(); }
        }

        public Type GetInstanceType(InjectContext context)
        {
            if (!_concreteType.DerivesFromOrEqual(context.MemberType))
            {
                return null;
            }

            return GetTypeToCreate(context.MemberType);
        }

        public List<object> GetAllInstancesWithInjectSplit(
            InjectContext context, List<TypeValuePair> args, out Action injectAction)
        {
            Assert.IsNotNull(context);

            bool autoInject = false;

            var instanceType = GetTypeToCreate(context.MemberType);

            var injectArgs = new InjectArgs()
            {
                ExtraArgs = _extraArguments.Concat(args).ToList(),
                Context = context,
                ConcreteIdentifier = _concreteIdentifier,
            };

            var instance = _container.InstantiateExplicit(
                instanceType, autoInject, injectArgs);

            injectAction = () => _container.InjectExplicit(instance, instanceType, injectArgs);
            return new List<object>() { instance };
        }

        Type GetTypeToCreate(Type contractType)
        {
            return ProviderUtil.GetTypeToInstantiate(contractType, _concreteType);
        }
    }
}
