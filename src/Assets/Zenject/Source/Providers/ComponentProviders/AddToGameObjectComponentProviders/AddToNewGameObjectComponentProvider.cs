#if !NOT_UNITY3D

using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using UnityEngine;
using Zenject;

namespace Zenject
{
    public class AddToNewGameObjectComponentProvider : AddToGameObjectComponentProviderBase
    {
        readonly string _gameObjectName;
        readonly string _groupName;

        public AddToNewGameObjectComponentProvider(
            DiContainer container, Type componentType,
            string concreteIdentifier, List<TypeValuePair> extraArguments, string gameObjectName, string groupName)
            : base(container, componentType, concreteIdentifier, extraArguments)
        {
            _gameObjectName = gameObjectName;
            _groupName = groupName;
        }

        protected override GameObject GetGameObject(InjectContext context)
        {
            return Container.CreateEmptyGameObject(
                _gameObjectName ?? ConcreteIdentifier ?? ComponentType.Name(), _groupName);
        }
    }
}

#endif
