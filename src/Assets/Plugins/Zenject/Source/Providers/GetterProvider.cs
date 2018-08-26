using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;

namespace Zenject
{
    public class GetterProvider<TObj, TResult> : IProvider
    {
        readonly DiContainer _container;
        readonly object _identifier;
        readonly Func<TObj, TResult> _method;
        readonly bool _matchAll;
        readonly InjectSources _sourceType;

        public GetterProvider(
            object identifier, Func<TObj, TResult> method,
            DiContainer container, InjectSources sourceType, bool matchAll)
        {
            _container = container;
            _identifier = identifier;
            _method = method;
            _matchAll = matchAll;
            _sourceType = sourceType;
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
            return typeof(TResult);
        }

        InjectContext GetSubContext(InjectContext parent)
        {
            var subContext = parent.CreateSubContext(
                typeof(TObj), _identifier);

            subContext.Optional = false;
            subContext.SourceType = _sourceType;

            return subContext;
        }

        public List<object> GetAllInstancesWithInjectSplit(
            InjectContext context, List<TypeValuePair> args, out Action injectAction)
        {
            Assert.IsEmpty(args);
            Assert.IsNotNull(context);

            Assert.That(typeof(TResult).DerivesFromOrEqual(context.MemberType));

            injectAction = null;

            if (_container.IsValidating)
            {
                // All we can do is validate that the getter object can be resolved
                if (_matchAll)
                {
                    _container.ResolveAll(GetSubContext(context));
                }
                else
                {
                    _container.Resolve(GetSubContext(context));
                }

                return new List<object>() { new ValidationMarker(typeof(TResult)) };
            }

            if (_matchAll)
            {
                return _container.ResolveAll(GetSubContext(context))
                    .Cast<TObj>().Select(x => _method(x)).Cast<object>().ToList();
            }
            else
            {
                return new List<object>() { _method(
                    (TObj)_container.Resolve(GetSubContext(context))) };
            }
        }
    }
}
