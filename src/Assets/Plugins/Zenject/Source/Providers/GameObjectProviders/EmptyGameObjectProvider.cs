#if !NOT_UNITY3D

using System;
using System.Collections.Generic;
using ModestTree;
using UnityEngine;

namespace Zenject
{
    public class EmptyGameObjectProvider : IProvider
    {
        readonly DiContainer _container;
        readonly GameObjectCreationParameters _gameObjectBindInfo;

        public EmptyGameObjectProvider(
            DiContainer container, GameObjectCreationParameters gameObjectBindInfo)
        {
            _gameObjectBindInfo = gameObjectBindInfo;
            _container = container;
        }

        public Type GetInstanceType(InjectContext context)
        {
            return typeof(GameObject);
        }

        public IEnumerator<List<object>> GetAllInstancesWithInjectSplit(
            InjectContext context, List<TypeValuePair> args)
        {
            Assert.IsEmpty(args);

            yield return new List<object>()
            {
                _container.CreateEmptyGameObject(_gameObjectBindInfo, context)
            };
        }
    }
}

#endif

