using System;
using ModestTree;

namespace Zenject
{
    public static class ExecutionOrderExtensions
    {
        public static void BindExecutionOrder<T>(
            this DiContainer container, int order)
        {
            container.BindExecutionOrder(typeof(T), order);
        }

        public static void BindExecutionOrder(
            this DiContainer container, Type type, int order)
        {
            Assert.That(type.DerivesFrom<ITickable>() || type.DerivesFrom<IInitializable>() || type.DerivesFrom<IDisposable>() || type.DerivesFrom<ILateDisposable>() || type.DerivesFrom<IFixedTickable>() || type.DerivesFrom<ILateTickable>(),
                "Expected type '{0}' to derive from one or more of the following interfaces: ITickable, IInitializable, ILateTickable, IFixedTickable, IDisposable, ILateDisposable", type);

            if (type.DerivesFrom<ITickable>())
            {
                container.BindTickableExecutionOrder(type, order);
            }

            if (type.DerivesFrom<IInitializable>())
            {
                container.BindInitializableExecutionOrder(type, order);
            }

            if (type.DerivesFrom<IDisposable>())
            {
                container.BindDisposableExecutionOrder(type, order);
            }

            if (type.DerivesFrom<ILateDisposable>())
            {
                container.BindLateDisposableExecutionOrder(type, order);
            }

            if (type.DerivesFrom<IFixedTickable>())
            {
                container.BindFixedTickableExecutionOrder(type, order);
            }

            if (type.DerivesFrom<ILateTickable>())
            {
                container.BindLateTickableExecutionOrder(type, order);
            }
        }

        public static void BindTickableExecutionOrder<T>(
            this DiContainer container, int order)
            where T : ITickable
        {
            container.BindTickableExecutionOrder(typeof(T), order);
        }

        public static void BindTickableExecutionOrder(
            this DiContainer container, Type type, int order)
        {
            Assert.That(type.DerivesFrom<ITickable>(),
                "Expected type '{0}' to derive from ITickable", type);

            container.BindInstance(
                ModestTree.Util.ValuePair.New(type, order)).WhenInjectedInto<TickableManager>();
        }

        public static void BindInitializableExecutionOrder<T>(
            this DiContainer container, int order)
            where T : IInitializable
        {
            container.BindInitializableExecutionOrder(typeof(T), order);
        }

        public static void BindInitializableExecutionOrder(
            this DiContainer container, Type type, int order)
        {
            Assert.That(type.DerivesFrom<IInitializable>(),
                "Expected type '{0}' to derive from IInitializable", type);

            container.BindInstance(
                ModestTree.Util.ValuePair.New(type, order)).WhenInjectedInto<InitializableManager>();
        }

        public static void BindDisposableExecutionOrder<T>(
            this DiContainer container, int order)
            where T : IDisposable
        {
            container.BindDisposableExecutionOrder(typeof(T), order);
        }

        public static void BindLateDisposableExecutionOrder<T>(
            this DiContainer container, int order)
            where T : ILateDisposable
        {
            container.BindLateDisposableExecutionOrder(typeof(T), order);
        }

        public static void BindDisposableExecutionOrder(
            this DiContainer container, Type type, int order)
        {
            Assert.That(type.DerivesFrom<IDisposable>(),
                "Expected type '{0}' to derive from IDisposable", type);

            container.BindInstance(
                ModestTree.Util.ValuePair.New(type, order)).WhenInjectedInto<DisposableManager>();
        }

        public static void BindLateDisposableExecutionOrder(
            this DiContainer container, Type type, int order)
        {
            Assert.That(type.DerivesFrom<ILateDisposable>(),
            "Expected type '{0}' to derive from ILateDisposable", type);

            container.BindInstance(
                ModestTree.Util.ValuePair.New(type, order)).WithId("Late").WhenInjectedInto<DisposableManager>();
        }

        public static void BindFixedTickableExecutionOrder<T>(
            this DiContainer container, int order)
            where T : IFixedTickable
        {
            container.BindFixedTickableExecutionOrder(typeof(T), order);
        }

        public static void BindFixedTickableExecutionOrder(
            this DiContainer container, Type type, int order)
        {
            Assert.That(type.DerivesFrom<IFixedTickable>(),
                "Expected type '{0}' to derive from IFixedTickable", type);

            container.Bind<ModestTree.Util.ValuePair<Type, int>>().WithId("Fixed")
                .FromInstance(ModestTree.Util.ValuePair.New(type, order)).WhenInjectedInto<TickableManager>();
        }

        public static void BindLateTickableExecutionOrder<T>(
            this DiContainer container, int order)
            where T : ILateTickable
        {
            container.BindLateTickableExecutionOrder(typeof(T), order);
        }

        public static void BindLateTickableExecutionOrder(
            this DiContainer container, Type type, int order)
        {
            Assert.That(type.DerivesFrom<ILateTickable>(),
                "Expected type '{0}' to derive from ILateTickable", type);

            container.Bind<ModestTree.Util.ValuePair<Type, int>>().WithId("Late")
                .FromInstance(ModestTree.Util.ValuePair.New(type, order)).WhenInjectedInto<TickableManager>();
        }
    }
}
