#if !NOT_UNITY3D

using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using UnityEngine;

namespace Zenject
{
    public class ScriptableObjectResourceProvider : IProvider
    {
        readonly DiContainer _container;
        readonly Type _resourceType;
        readonly string _resourcePath;
        readonly List<TypeValuePair> _extraArguments;
        readonly bool _createNew;
        readonly object _concreteIdentifier;

        public ScriptableObjectResourceProvider(
            string resourcePath, Type resourceType,
            DiContainer container, List<TypeValuePair> extraArguments,
            bool createNew, object concreteIdentifier)
        {
            _container = container;
            Assert.DerivesFromOrEqual<ScriptableObject>(resourceType);

            _extraArguments = extraArguments;
            _resourceType = resourceType;
            _resourcePath = resourcePath;
            _createNew = createNew;
            _concreteIdentifier = concreteIdentifier;
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
            Assert.IsNotNull(context);

            List<object> objects;

            if (_createNew)
            {
                objects = Resources.LoadAll(_resourcePath, _resourceType)
                    .Select(x => ScriptableObject.Instantiate(x)).Cast<object>().ToList();
            }
            else
            {
                objects = Resources.LoadAll(_resourcePath, _resourceType)
                    .Cast<object>().ToList();
            }

            Assert.That(!objects.IsEmpty(),
                "Could not find resource at path '{0}' with type '{1}'", _resourcePath, _resourceType);

            var injectArgs = new InjectArgs()
            {
                ExtraArgs = _extraArguments.Concat(args).ToList(),
                Context = context,
                ConcreteIdentifier = _concreteIdentifier,
            };

            injectAction = () =>
            {
                foreach (var obj in objects)
                {
                    _container.InjectExplicit(
                        obj, _resourceType, injectArgs);
                }
            };

            return objects;
        }
    }
}

#endif
