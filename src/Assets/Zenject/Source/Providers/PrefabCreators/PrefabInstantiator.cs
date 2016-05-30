#if !NOT_UNITY3D

using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using UnityEngine;

namespace Zenject
{
    public class PrefabInstantiator : IPrefabInstantiator
    {
        readonly IPrefabProvider _prefabProvider;
        readonly DiContainer _container;
        readonly string _gameObjectName;
        readonly string _gameObjectGroupName;
        readonly List<TypeValuePair> _extraArguments;

        public PrefabInstantiator(
            DiContainer container,
            string gameObjectName,
            string gameObjectGroupName,
            List<TypeValuePair> extraArguments,
            IPrefabProvider prefabProvider)
        {
            _prefabProvider = prefabProvider;
            _extraArguments = extraArguments;
            _container = container;
            _gameObjectName = gameObjectName;
            _gameObjectGroupName = gameObjectGroupName;
        }

        public string GameObjectGroupName
        {
            get
            {
                return _gameObjectGroupName;
            }
        }

        public string GameObjectName
        {
            get
            {
                return _gameObjectName;
            }
        }

        public List<TypeValuePair> ExtraArguments
        {
            get
            {
                return _extraArguments;
            }
        }

        public GameObject GetPrefab()
        {
            return _prefabProvider.GetPrefab();
        }

        public IEnumerator<GameObject> Instantiate(List<TypeValuePair> args)
        {
            var gameObject = _container.CreateAndParentPrefab(GetPrefab(), _gameObjectGroupName);
            Assert.IsNotNull(gameObject);

            if (_gameObjectName != null)
            {
                gameObject.name = _gameObjectName;
            }

            // Return it before inject so we can do circular dependencies
            yield return gameObject;

            _container.InjectGameObjectExplicit(
                gameObject, true, _extraArguments.Concat(args).ToList());
        }
    }
}

#endif
