#if !NOT_UNITY3D

using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using UnityEngine;

namespace Zenject
{
    public class ResourceProvider : IProvider
    {
        readonly Type _resourceType;
        readonly string _resourcePath;
        readonly bool _matchSingle;

        public ResourceProvider(
            string resourcePath, Type resourceType, bool matchSingle)
        {
            _resourceType = resourceType;
            _resourcePath = resourcePath;
            _matchSingle = matchSingle;
        }

        public bool IsCached
        {
            get { return false; }
        }

        public bool TypeVariesBasedOnMemberType
        {
            get { return false; }
        }

        public Type GetInstanceType(InjectContext context)
        {
            return _resourceType;
        }

        public List<object> GetAllInstancesWithInjectSplit(
            InjectContext context, List<TypeValuePair> args, out Action injectAction)
        {
            Assert.IsEmpty(args);

            Assert.IsNotNull(context);

            if (_matchSingle)
            {
                var obj = Resources.Load(_resourcePath, _resourceType);

                Assert.That(obj != null,
                    "Could not find resource at path '{0}' with type '{1}'", _resourcePath, _resourceType);

                // Are there any resource types which can be injected?
                injectAction = null;
                return new List<object>() { obj };
            }

            var objects = Resources.LoadAll(_resourcePath, _resourceType).Cast<object>().ToList();

            Assert.That(!objects.IsEmpty(),
                "Could not find resource at path '{0}' with type '{1}'", _resourcePath, _resourceType);

            // Are there any resource types which can be injected?
            injectAction = null;
            return objects;
        }
    }
}

#endif


