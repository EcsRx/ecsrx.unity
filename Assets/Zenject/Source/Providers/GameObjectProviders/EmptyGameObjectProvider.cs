#if !NOT_UNITY3D

using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using UnityEngine;
using Zenject;

namespace Zenject
{
    public class EmptyGameObjectProvider : IProvider
    {
        readonly DiContainer _container;
        readonly string _gameObjectName;
        readonly string _groupName;

        public EmptyGameObjectProvider(
            DiContainer container, string gameObjectName, string groupName)
        {
            _gameObjectName = gameObjectName;
            _groupName = groupName;
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
                _container.CreateEmptyGameObject(_gameObjectName, _groupName)
            };
        }
    }
}

#endif

