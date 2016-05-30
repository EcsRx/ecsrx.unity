#if !NOT_UNITY3D

using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using UnityEngine;
using Zenject;

namespace Zenject
{
    public class ResourceProvider : IProvider
    {
        readonly Type _resourceType;
        readonly string _resourcePath;

        public ResourceProvider(
            string resourcePath, Type resourceType)
        {
            _resourceType = resourceType;
            _resourcePath = resourcePath;
        }

        public Type GetInstanceType(InjectContext context)
        {
            return _resourceType;
        }

        public IEnumerator<List<object>> GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args)
        {
            Assert.IsEmpty(args);

            Assert.IsNotNull(context);

            var obj = Resources.Load(_resourcePath, _resourceType);

            Assert.IsNotNull(obj,
                "Could not find resource at path '{0}' with type '{1}'", _resourcePath, _resourceType);

            yield return new List<object>() { obj };

            // Are there any resource types which can be injected?
        }
    }
}

#endif



