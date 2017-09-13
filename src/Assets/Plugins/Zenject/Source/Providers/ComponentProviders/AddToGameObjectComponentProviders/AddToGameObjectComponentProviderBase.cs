#if !NOT_UNITY3D

using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using UnityEngine;

namespace Zenject
{
    public abstract class AddToGameObjectComponentProviderBase : IProvider
    {
        readonly object _concreteIdentifier;
        readonly Type _componentType;
        readonly DiContainer _container;
        readonly List<TypeValuePair> _extraArguments;

        public AddToGameObjectComponentProviderBase(
            DiContainer container, Type componentType,
            object concreteIdentifier, List<TypeValuePair> extraArguments)
        {
            Assert.That(componentType.DerivesFrom<Component>());

            _concreteIdentifier = concreteIdentifier;
            _extraArguments = extraArguments;
            _componentType = componentType;
            _container = container;
        }

        protected DiContainer Container
        {
            get { return _container; }
        }

        protected Type ComponentType
        {
            get { return _componentType; }
        }

        protected object ConcreteIdentifier
        {
            get { return _concreteIdentifier; }
        }

        public Type GetInstanceType(InjectContext context)
        {
            return _componentType;
        }

        public IEnumerator<List<object>> GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args)
        {
            Assert.IsNotNull(context);

            object instance;

            // We still want to make sure we can get the game object during validation
            var gameObj = GetGameObject(context);

            if (!_container.IsValidating || DiContainer.CanCreateOrInjectDuringValidation(_componentType))
            {
                if (_componentType == typeof(Transform))
                // Treat transform as a special case because it's the one component that's always automatically added
                // Otherwise, calling AddComponent below will fail and return null
                // This is nice to allow doing things like
                //      Container.Bind<Transform>().FromNewComponentOnNewGameObject();
                {
                    instance = gameObj.transform;
                }
                else
                {
                    instance = gameObj.AddComponent(_componentType);
                }

                Assert.IsNotNull(instance);
            }
            else
            {
                instance = new ValidationMarker(_componentType);
            }

            // Note that we don't just use InstantiateComponentOnNewGameObjectExplicit here
            // because then circular references don't work
            yield return new List<object>() { instance };

            var injectArgs = new InjectArgs()
            {
                ExtraArgs = _extraArguments.Concat(args).ToList(),
                Context = context,
                ConcreteIdentifier = _concreteIdentifier,
            };

            _container.InjectExplicit(instance, _componentType, injectArgs);

            Assert.That(injectArgs.ExtraArgs.IsEmpty());
        }

        protected abstract GameObject GetGameObject(InjectContext context);
    }
}

#endif
