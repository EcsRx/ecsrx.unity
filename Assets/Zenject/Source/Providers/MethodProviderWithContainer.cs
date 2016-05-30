using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using Zenject;

namespace Zenject
{
    // Zero params

    public class MethodProviderWithContainer<TValue> : IProvider
    {
        readonly Func<DiContainer, TValue> _method;

        public MethodProviderWithContainer(Func<DiContainer, TValue> method)
        {
            _method = method;
        }

        public Type GetInstanceType(InjectContext context)
        {
            return typeof(TValue);
        }

        public IEnumerator<List<object>> GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args)
        {
            Assert.IsEmpty(args);
            Assert.IsNotNull(context);

            Assert.That(typeof(TValue).DerivesFromOrEqual(context.MemberType));

            yield return new List<object>() { _method(context.Container) };
        }
    }

    // One params

    public class MethodProviderWithContainer<TParam1, TValue> : IProvider
    {
        readonly Func<DiContainer, TParam1, TValue> _method;

        public MethodProviderWithContainer(Func<DiContainer, TParam1, TValue> method)
        {
            _method = method;
        }

        public Type GetInstanceType(InjectContext context)
        {
            return typeof(TValue);
        }

        public IEnumerator<List<object>> GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args)
        {
            Assert.IsEqual(args.Count, 1);
            Assert.IsNotNull(context);

            Assert.That(typeof(TValue).DerivesFromOrEqual(context.MemberType));
            Assert.IsEqual(args[0].Type, typeof(TParam1));

            yield return new List<object>()
            {
                _method(
                    context.Container,
                    (TParam1)args[0].Value)
            };
        }
    }

    // Two params

    public class MethodProviderWithContainer<TParam1, TParam2, TValue> : IProvider
    {
        readonly Func<DiContainer, TParam1, TParam2, TValue> _method;

        public MethodProviderWithContainer(Func<DiContainer, TParam1, TParam2, TValue> method)
        {
            _method = method;
        }

        public Type GetInstanceType(InjectContext context)
        {
            return typeof(TValue);
        }

        public IEnumerator<List<object>> GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args)
        {
            Assert.IsEqual(args.Count, 2);
            Assert.IsNotNull(context);

            Assert.That(typeof(TValue).DerivesFromOrEqual(context.MemberType));
            Assert.IsEqual(args[0].Type, typeof(TParam1));
            Assert.IsEqual(args[1].Type, typeof(TParam2));

            yield return new List<object>()
            {
                _method(
                    context.Container,
                    (TParam1)args[0].Value,
                    (TParam2)args[1].Value)
            };
        }
    }

    // Three params

    public class MethodProviderWithContainer<TParam1, TParam2, TParam3, TValue> : IProvider
    {
        readonly Func<DiContainer, TParam1, TParam2, TParam3, TValue> _method;

        public MethodProviderWithContainer(Func<DiContainer, TParam1, TParam2, TParam3, TValue> method)
        {
            _method = method;
        }

        public Type GetInstanceType(InjectContext context)
        {
            return typeof(TValue);
        }

        public IEnumerator<List<object>> GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args)
        {
            Assert.IsEqual(args.Count, 3);
            Assert.IsNotNull(context);

            Assert.That(typeof(TValue).DerivesFromOrEqual(context.MemberType));
            Assert.IsEqual(args[0].Type, typeof(TParam1));
            Assert.IsEqual(args[1].Type, typeof(TParam2));
            Assert.IsEqual(args[2].Type, typeof(TParam3));

            yield return new List<object>()
            {
                _method(
                    context.Container,
                    (TParam1)args[0].Value,
                    (TParam2)args[1].Value,
                    (TParam3)args[2].Value)
            };
        }
    }

    // Four params

    public class MethodProviderWithContainer<TParam1, TParam2, TParam3, TParam4, TValue> : IProvider
    {
        readonly ModestTree.Util.Func<DiContainer, TParam1, TParam2, TParam3, TParam4, TValue> _method;

        public MethodProviderWithContainer(ModestTree.Util.Func<DiContainer, TParam1, TParam2, TParam3, TParam4, TValue> method)
        {
            _method = method;
        }

        public Type GetInstanceType(InjectContext context)
        {
            return typeof(TValue);
        }

        public IEnumerator<List<object>> GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args)
        {
            Assert.IsEqual(args.Count, 4);
            Assert.IsNotNull(context);

            Assert.That(typeof(TValue).DerivesFromOrEqual(context.MemberType));
            Assert.IsEqual(args[0].Type, typeof(TParam1));
            Assert.IsEqual(args[1].Type, typeof(TParam2));
            Assert.IsEqual(args[2].Type, typeof(TParam3));
            Assert.IsEqual(args[3].Type, typeof(TParam4));

            yield return new List<object>()
            {
                _method(
                    context.Container,
                    (TParam1)args[0].Value,
                    (TParam2)args[1].Value,
                    (TParam3)args[2].Value,
                    (TParam4)args[3].Value)
            };
        }
    }

    // Five params

    public class MethodProviderWithContainer<TParam1, TParam2, TParam3, TParam4, TParam5, TValue> : IProvider
    {
        readonly ModestTree.Util.Func<DiContainer, TParam1, TParam2, TParam3, TParam4, TParam5, TValue> _method;

        public MethodProviderWithContainer(ModestTree.Util.Func<DiContainer, TParam1, TParam2, TParam3, TParam4, TParam5, TValue> method)
        {
            _method = method;
        }

        public Type GetInstanceType(InjectContext context)
        {
            return typeof(TValue);
        }

        public IEnumerator<List<object>> GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args)
        {
            Assert.IsEqual(args.Count, 5);
            Assert.IsNotNull(context);

            Assert.That(typeof(TValue).DerivesFromOrEqual(context.MemberType));
            Assert.IsEqual(args[0].Type, typeof(TParam1));
            Assert.IsEqual(args[1].Type, typeof(TParam2));
            Assert.IsEqual(args[2].Type, typeof(TParam3));
            Assert.IsEqual(args[3].Type, typeof(TParam4));
            Assert.IsEqual(args[4].Type, typeof(TParam5));

            yield return new List<object>()
            {
                _method(
                    context.Container,
                    (TParam1)args[0].Value,
                    (TParam2)args[1].Value,
                    (TParam3)args[2].Value,
                    (TParam4)args[3].Value,
                    (TParam5)args[4].Value)
            };
        }
    }
}

