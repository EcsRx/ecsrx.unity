#if !NOT_UNITY3D

using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using UnityEngine;
using Zenject.Internal;

namespace Zenject
{
    public class PrefabResourceSingletonProviderCreator
    {
        readonly SingletonMarkRegistry _markRegistry;
        readonly DiContainer _container;
        readonly Dictionary<PrefabId, IPrefabInstantiator> _prefabCreators =
            new Dictionary<PrefabId, IPrefabInstantiator>();

        public PrefabResourceSingletonProviderCreator(
            DiContainer container,
            SingletonMarkRegistry markRegistry)
        {
            _markRegistry = markRegistry;
            _container = container;
        }

        public IProvider CreateProvider(
            string resourcePath, Type resultType, string gameObjectName, string gameObjectGroupName,
            List<TypeValuePair> extraArguments, string concreteIdentifier)
        {
            IPrefabInstantiator creator;

            _markRegistry.MarkSingleton(
                resultType, concreteIdentifier, SingletonTypes.ToPrefabResource);

            var prefabId = new PrefabId(concreteIdentifier, resourcePath);

            if (_prefabCreators.TryGetValue(prefabId, out creator))
            {
                // TODO: Check the arguments are the same?
                Assert.That(creator.ExtraArguments.IsEmpty() && extraArguments.IsEmpty(),
                    "Ambiguous creation parameters (arguments) when using ToPrefabResource with AsSingle");

                Assert.IsEqual(creator.GameObjectName, gameObjectName,
                    "Ambiguous creation parameters (gameObjectName) when using ToPrefabResource with AsSingle");

                Assert.IsEqual(creator.GameObjectGroupName, gameObjectGroupName,
                    "Ambiguous creation parameters (gameObjectGroupName) when using ToPrefabResource with AsSingle");
            }
            else
            {
                creator = new PrefabInstantiatorCached(
                    new PrefabInstantiator(
                        _container, gameObjectName,
                        gameObjectGroupName, extraArguments,
                        new PrefabProviderResource(resourcePath)));

                _prefabCreators.Add(prefabId, creator);
            }

            if (resultType == typeof(GameObject))
            {
                return new PrefabGameObjectProvider(creator);
            }

            return new GetFromPrefabComponentProvider(resultType, creator);
        }

        class PrefabId : IEquatable<PrefabId>
        {
            public readonly string ConcreteIdentifier;
            public readonly string ResourcePath;

            public PrefabId(string concreteIdentifier, string resourcePath)
            {
                Assert.IsNotNull(resourcePath);

                ConcreteIdentifier = concreteIdentifier;
                ResourcePath = resourcePath;
            }

            public override int GetHashCode()
            {
                unchecked // Overflow is fine, just wrap
                {
                    int hash = 17;
                    hash = hash * 29 + (this.ConcreteIdentifier == null ? 0 : this.ConcreteIdentifier.GetHashCode());
                    hash = hash * 29 + this.ResourcePath.GetHashCode();
                    return hash;
                }
            }

            public override bool Equals(object other)
            {
                if (other is PrefabId)
                {
                    PrefabId otherId = (PrefabId)other;
                    return otherId == this;
                }
                else
                {
                    return false;
                }
            }

            public bool Equals(PrefabId that)
            {
                return this == that;
            }

            public static bool operator ==(PrefabId left, PrefabId right)
            {
                return object.Equals(left.ResourcePath, right.ResourcePath)
                    && object.Equals(left.ConcreteIdentifier, right.ConcreteIdentifier);
            }

            public static bool operator !=(PrefabId left, PrefabId right)
            {
                return !left.Equals(right);
            }
        }
    }
}

#endif

