#if !NOT_UNITY3D

using System;
using System.Collections.Generic;
using ModestTree;
using UnityEngine;

namespace Zenject
{
    public class AddToExistingGameObjectComponentProvider : AddToGameObjectComponentProviderBase
    {
        readonly GameObject _gameObject;

        public AddToExistingGameObjectComponentProvider(
            GameObject gameObject, DiContainer container, Type componentType,
            object concreteIdentifier, List<TypeValuePair> extraArguments)
            : base(container, componentType, concreteIdentifier, extraArguments)
        {
            _gameObject = gameObject;
        }

        protected override GameObject GetGameObject(InjectContext context)
        {
            return _gameObject;
        }
    }

    public class AddToExistingGameObjectComponentProviderGetter : AddToGameObjectComponentProviderBase
    {
        readonly Func<InjectContext, GameObject> _gameObjectGetter;

        public AddToExistingGameObjectComponentProviderGetter(
            Func<InjectContext, GameObject> gameObjectGetter, DiContainer container, Type componentType,
            object concreteIdentifier, List<TypeValuePair> extraArguments)
            : base(container, componentType, concreteIdentifier, extraArguments)
        {
            _gameObjectGetter = gameObjectGetter;
        }

        protected override GameObject GetGameObject(InjectContext context)
        {
            var gameObj = _gameObjectGetter(context);
            Assert.IsNotNull(gameObj, "Provided Func<InjectContext, GameObject> returned null value for game object when using FromComponentOn");
            return gameObj;
        }
    }
}

#endif
