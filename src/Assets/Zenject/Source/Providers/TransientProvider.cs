using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using Zenject;

namespace Zenject
{
    public class TransientProvider : IProvider
    {
        readonly DiContainer _container;
        readonly Type _concreteType;
        readonly string _concreteIdentifier;
        readonly List<TypeValuePair> _extraArguments;

        public TransientProvider(
            Type concreteType, DiContainer container,
            List<TypeValuePair> extraArguments, string concreteIdentifier)
        {
            _container = container;
            _concreteType = concreteType;
            _concreteIdentifier = concreteIdentifier;
            _extraArguments = extraArguments;
        }

        public TransientProvider(
            Type concreteType, DiContainer container,
            List<TypeValuePair> extraArguments)
            : this(concreteType, container, extraArguments, null)
        {
        }

        public TransientProvider(
            Type concreteType, DiContainer container)
            : this(concreteType, container, new List<TypeValuePair>())
        {
        }

        public Type GetInstanceType(InjectContext context)
        {
            return _concreteType;
        }

        public IEnumerator<List<object>> GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args)
        {
            Assert.IsNotNull(context);

            bool autoInject = false;

            var injectArgs = new InjectArgs()
            {
                TypeInfo = TypeAnalyzer.GetInfo(GetTypeToCreate(context.MemberType)),
                ExtraArgs = _extraArguments.Concat(args).ToList(),
                Context = context,
                ConcreteIdentifier = _concreteIdentifier,
                UseAllArgs = false,
            };

            var instance = _container.InstantiateExplicit(
                autoInject, injectArgs);

            // Return before property/field/method injection to allow circular dependencies
            yield return new List<object>() { instance };

            injectArgs.UseAllArgs = true;

            _container.InjectExplicit(instance, injectArgs);
        }

        Type GetTypeToCreate(Type contractType)
        {
            return ProviderUtil.GetTypeToInstantiate(contractType, _concreteType);
        }
    }
}
