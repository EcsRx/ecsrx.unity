#if !NOT_UNITY3D

using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using UnityEngine;
using Zenject.Internal;

namespace Zenject
{
    public class SubContainerSingletonProviderCreatorByPrefab
    {
        readonly SingletonMarkRegistry _markRegistry;
        readonly DiContainer _container;
        readonly Dictionary<CustomSingletonId, CreatorInfo> _subContainerCreators =
            new Dictionary<CustomSingletonId, CreatorInfo>();

        public SubContainerSingletonProviderCreatorByPrefab(
            DiContainer container,
            SingletonMarkRegistry markRegistry)
        {
            _markRegistry = markRegistry;
            _container = container;
        }

        public IProvider CreateProvider(
            Type resultType, string concreteIdentifier, GameObject prefab, object identifier,
            string gameObjectName, string gameObjectGroupName)
        {
            _markRegistry.MarkSingleton(
                resultType, concreteIdentifier,
                SingletonTypes.ToSubContainerPrefab);

            var customSingletonId = new CustomSingletonId(
                concreteIdentifier, prefab);

            CreatorInfo creatorInfo;

            if (_subContainerCreators.TryGetValue(customSingletonId, out creatorInfo))
            {
                Assert.IsEqual(creatorInfo.GameObjectName, gameObjectName,
                    "Ambiguous creation parameters (gameObjectName) when using ToSubContainerPrefab with AsSingle");

                Assert.IsEqual(creatorInfo.GameObjectGroupName, gameObjectGroupName,
                    "Ambiguous creation parameters (gameObjectGroupName) when using ToSubContainerPrefab with AsSingle");
            }
            else
            {
                var creator = new SubContainerCreatorCached(
                    new SubContainerCreatorByPrefab(_container, new PrefabProvider(prefab), gameObjectName, gameObjectGroupName));

                creatorInfo = new CreatorInfo(gameObjectName, gameObjectGroupName, creator);

                _subContainerCreators.Add(customSingletonId, creatorInfo);
            }

            return new SubContainerDependencyProvider(
                resultType, identifier, creatorInfo.Creator);
        }

        class CustomSingletonId : IEquatable<CustomSingletonId>
        {
            public readonly string ConcreteIdentifier;
            public readonly GameObject Prefab;

            public CustomSingletonId(string concreteIdentifier, GameObject prefab)
            {
                ConcreteIdentifier = concreteIdentifier;
                Prefab = prefab;
            }

            public override int GetHashCode()
            {
                unchecked // Overflow is fine, just wrap
                {
                    int hash = 17;
                    hash = hash * 29 + (this.ConcreteIdentifier == null ? 0 : this.ConcreteIdentifier.GetHashCode());
                    hash = hash * 29 + (ZenUtilInternal.IsNull(this.Prefab) ? 0 : this.Prefab.GetHashCode());
                    return hash;
                }
            }

            public override bool Equals(object other)
            {
                if (other is CustomSingletonId)
                {
                    CustomSingletonId otherId = (CustomSingletonId)other;
                    return otherId == this;
                }
                else
                {
                    return false;
                }
            }

            public bool Equals(CustomSingletonId that)
            {
                return this == that;
            }

            public static bool operator ==(CustomSingletonId left, CustomSingletonId right)
            {
                return object.Equals(left.Prefab, right.Prefab)
                    && object.Equals(left.ConcreteIdentifier, right.ConcreteIdentifier);
            }

            public static bool operator !=(CustomSingletonId left, CustomSingletonId right)
            {
                return !left.Equals(right);
            }
        }

        class CreatorInfo
        {
            public CreatorInfo(
                string gameObjectName, string gameObjectGroupName, ISubContainerCreator creator)
            {
                GameObjectName = gameObjectName;
                GameObjectGroupName = gameObjectGroupName;
                Creator = creator;
            }

            public string GameObjectName
            {
                get;
                private set;
            }

            public string GameObjectGroupName
            {
                get;
                private set;
            }

            public ISubContainerCreator Creator
            {
                get;
                private set;
            }
        }
    }
}

#endif
