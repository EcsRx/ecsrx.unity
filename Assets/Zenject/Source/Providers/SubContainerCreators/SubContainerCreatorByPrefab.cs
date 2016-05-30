#if !NOT_UNITY3D

using System;
using System.Collections.Generic;
using ModestTree;
using UnityEngine;
using Zenject.Internal;
using System.Linq;

namespace Zenject
{
    public class SubContainerCreatorByPrefab : ISubContainerCreator
    {
        readonly string _groupName;
        readonly string _gameObjectName;
        readonly IPrefabProvider _prefabProvider;
        readonly DiContainer _container;

        public SubContainerCreatorByPrefab(
            DiContainer container, IPrefabProvider prefabProvider, string gameObjectName, string groupName)
        {
            _prefabProvider = prefabProvider;
            _groupName = groupName;
            _gameObjectName = gameObjectName;
            _container = container;
        }

        public DiContainer CreateSubContainer(List<TypeValuePair> args)
        {
            Assert.That(args.IsEmpty());

            var prefab = _prefabProvider.GetPrefab();
            var gameObject = _container.InstantiatePrefab(
                prefab, new object[0], _groupName);

            if (_gameObjectName != null)
            {
                gameObject.name = _gameObjectName;
            }

            var context = gameObject.GetComponent<GameObjectContext>();

            Assert.IsNotNull(context,
                "Expected prefab with name '{0}' to container a component of type 'GameObjectContext'", prefab.name);

            return context.Container;
        }
    }
}

#endif
