#if !NOT_UNITY3D

using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using UnityEngine;
using Zenject.Internal;

namespace Zenject
{
    public class SubContainerSingletonProviderCreatorByPrefabResource
    {
        readonly SingletonMarkRegistry _markRegistry;
        readonly DiContainer _container;
        readonly Dictionary<CustomSingletonId, CreatorInfo> _subContainerCreators =
            new Dictionary<CustomSingletonId, CreatorInfo>();

        public SubContainerSingletonProviderCreatorByPrefabResource(
            DiContainer container,
            SingletonMarkRegistry markRegistry)
        {
            _markRegistry = markRegistry;
            _container = container;
        }

        public IProvider CreateProvider(
            Type resultType, string concreteIdentifier, string resourcePath, object identifier, string gameObjectName, string gameObjectGroupName)
        {
            _markRegistry.MarkSingleton(
                resultType, concreteIdentifier,
                SingletonTypes.ToSubContainerPrefabResource);

            var customSingletonId = new CustomSingletonId(
                concreteIdentifier, resourcePath);

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
                    new SubContainerCreatorByPrefab(
                        _container, new PrefabProviderResource(resourcePath), gameObjectName, gameObjectGroupName));

                creatorInfo = new CreatorInfo(
                    gameObjectName, gameObjectGroupName, creator);

                _subContainerCreators.Add(customSingletonId, creatorInfo);
            }

            return new SubContainerDependencyProvider(
                resultType, identifier, creatorInfo.Creator);
        }

        class CustomSingletonId : IEquatable<CustomSingletonId>
        {
            public readonly string ConcreteIdentifier;
            public readonly string ResourcePath;

            public CustomSingletonId(string concreteIdentifier, string resourcePath)
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
                return object.Equals(left.ResourcePath, right.ResourcePath)
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
