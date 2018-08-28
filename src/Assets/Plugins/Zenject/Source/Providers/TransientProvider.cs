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
        readonly Action<InjectContext, object> _instantiateCallback;

        public TransientProvider(
            Type concreteType, DiContainer container,
            List<TypeValuePair> extraArguments, string bindingContext,
            object concreteIdentifier,
            Action<InjectContext, object> instantiateCallback)
        {
            Assert.That(!concreteType.IsAbstract(),
                "Expected non-abstract type for given binding but instead found type '{0}'{1}",
                concreteType, bindingContext == null ? "" : " when binding '{0}'".Fmt(bindingContext) );

            _container = container;
            _concreteType = concreteType;
            _extraArguments = extraArguments ?? new List<TypeValuePair>();
            _concreteIdentifier = concreteIdentifier;
            _instantiateCallback = instantiateCallback;
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

            var instanceType = GetTypeToCreate(context.MemberType);

            var injectArgs = new InjectArgs()
            {
                ExtraArgs = _extraArguments.Concat(args).ToList(),
                Context = context,
                ConcreteIdentifier = _concreteIdentifier,
            };

            var instance = _container.InstantiateExplicit(
                instanceType, false, injectArgs);

            injectAction = () =>
                {
                    _container.InjectExplicit(
                        instance, instanceType, injectArgs);

                    if (_instantiateCallback != null)
                    {
                        _instantiateCallback(context, instance);
                    }
                };

            return new List<object>() { instance };
        }

        Type GetTypeToCreate(Type contractType)
        {
            return ProviderUtil.GetTypeToInstantiate(contractType, _concreteType);
        }
    }
}
