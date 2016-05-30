using System;
using UnityEngine;
using Zenject;

namespace Zenject.TestFramework
{
    public static class FixtureUtil
    {
        public static void AddInitMethod(
            DiContainer container, Action method)
        {
            container.Bind<IInitializable>()
                .To<InitMethodHandler>().WithArguments(method);
        }

        public static void AddInitMethod<TParam1>(
            DiContainer container, Action<TParam1> method)
        {
            container.Bind<IInitializable>()
                .To<InitMethodHandler<TParam1>>().WithArguments(method);
        }

        public static void AddInitMethod<TParam1, TParam2>(
            DiContainer container, Action<TParam1, TParam2> method)
        {
            container.Bind<IInitializable>()
                .To<InitMethodHandler<TParam1, TParam2>>().WithArguments(method);
        }

        public static void AssertNumGameObjects(
            DiContainer container, int expectedNumGameObjects)
        {
            container.Bind<IInitializable>()
                .To<GameObjectCountAsserter>().WithArguments(expectedNumGameObjects);
        }

        public static void AssertResolveCount<TContract>(
            DiContainer container, int expectedNum)
        {
            container.Bind<IInitializable>()
                .To<ResolveCountAsserter<TContract>>().WithArguments(expectedNum);
        }

        public static void AssertComponentCount<TComponent>(
            DiContainer container, int expectedNumComponents)
        {
            container.Bind<IInitializable>()
                .To<ComponentCountAsserter<TComponent>>().WithArguments(expectedNumComponents);
        }

        public static void AssertNumGameObjectsWithName(
            DiContainer container, string name, int expectedNumGameObjects)
        {
            container.Bind<IInitializable>()
                .To<GameObjectNameCountAsserter>().WithArguments(name, expectedNumGameObjects);
        }

        public static void CallFactoryCreateMethod<TValue, TFactory>(DiContainer container)
            where TFactory : Factory<TValue>
        {
            container.Bind<IInitializable>().To<FactoryUser<TValue, TFactory>>().AsCached();

            // Always create first so that you can use the asserts above
            container.BindInitializableExecutionOrder<FactoryUser<TValue, TFactory>>(-1);
        }
    }
}
