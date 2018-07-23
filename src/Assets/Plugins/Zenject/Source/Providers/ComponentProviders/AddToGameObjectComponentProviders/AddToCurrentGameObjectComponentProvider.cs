#if !NOT_UNITY3D

using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using UnityEngine;
using Zenject;

namespace Zenject
{
    public class AddToCurrentGameObjectComponentProvider : IProvider
    {
        readonly Type _componentType;
        readonly DiContainer _container;
        readonly List<TypeValuePair> _extraArguments;
        readonly object _concreteIdentifier;

        public AddToCurrentGameObjectComponentProvider(
            DiContainer container, Type componentType,
            List<TypeValuePair> extraArguments, object concreteIdentifier)
        {
            Assert.That(componentType.DerivesFrom<Component>());

            _extraArguments = extraArguments;
            _componentType = componentType;
            _container = container;
            _concreteIdentifier = concreteIdentifier;
        }

        public bool IsCached
        {
            get { return false; }
        }

        public bool TypeVariesBasedOnMemberType
        {
            get { return false; }
        }

        protected DiContainer Container
        {
            get { return _container; }
        }

        protected Type ComponentType
        {
            get { return _componentType; }
        }

        public Type GetInstanceType(InjectContext context)
        {
            return _componentType;
        }

        public List<object> GetAllInstancesWithInjectSplit(
            InjectContext context, List<TypeValuePair> args, out Action injectAction)
        {
            Assert.IsNotNull(context);

            Assert.That(context.ObjectType.DerivesFrom<Component>(),
                "Object '{0}' can only be injected into MonoBehaviour's since it was bound with 'FromNewComponentSibling'. Attempted to inject into non-MonoBehaviour '{1}'",
                context.MemberType, context.ObjectType);

            object instance;

            if (!_container.IsValidating || TypeAnalyzer.ShouldAllowDuringValidation(_componentType))
            {
                var gameObj = ((Component)context.ObjectInstance).gameObject;

                instance = gameObj.GetComponent(_componentType);

                if (instance != null)
                {
                    injectAction = null;
                    return new List<object>() { instance };
                }

                instance = gameObj.AddComponent(_componentType);
            }
            else
            {
                instance = new ValidationMarker(_componentType);
            }

            // Note that we don't just use InstantiateComponentOnNewGameObjectExplicit here
            // because then circular references don't work

            var injectArgs = new InjectArgs()
            {
                ExtraArgs = _extraArguments.Concat(args).ToList(),
                Context = context,
                ConcreteIdentifier = _concreteIdentifier
            };

            injectAction = () =>
            {
                _container.InjectExplicit(instance, _componentType, injectArgs);
                Assert.That(injectArgs.ExtraArgs.IsEmpty());
            };

            return new List<object>() { instance };
        }
    }
}

#endif
