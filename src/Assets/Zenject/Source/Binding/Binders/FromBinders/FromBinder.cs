using System;
using System.Collections.Generic;
using ModestTree;
using System.Linq;

#if !NOT_UNITY3D
using UnityEngine;
#endif

using Zenject.Internal;

namespace Zenject
{
    public abstract class FromBinder : ScopeArgBinder
    {
        public FromBinder(
            BindInfo bindInfo,
            BindFinalizerWrapper finalizerWrapper)
            : base(bindInfo)
        {
            FinalizerWrapper = finalizerWrapper;
        }

        protected BindFinalizerWrapper FinalizerWrapper
        {
            get;
            private set;
        }

        protected IBindingFinalizer SubFinalizer
        {
            set
            {
                FinalizerWrapper.SubFinalizer = value;
            }
        }

        protected IEnumerable<Type> AllParentTypes
        {
            get
            {
                return BindInfo.ContractTypes.Concat(BindInfo.ToTypes);
            }
        }

        protected IEnumerable<Type> ConcreteTypes
        {
            get
            {
                if (BindInfo.ToChoice == ToChoices.Self)
                {
                    return BindInfo.ContractTypes;
                }

                Assert.IsNotEmpty(BindInfo.ToTypes);
                return BindInfo.ToTypes;
            }
        }

        // This is the default if nothing else is called
        public ScopeArgBinder FromNew()
        {
            BindingUtil.AssertTypesAreNotComponents(ConcreteTypes);
            BindingUtil.AssertTypesAreNotAbstract(ConcreteTypes);

            return this;
        }

        public ScopeBinder FromResolve()
        {
            return FromResolve(null);
        }

        public ScopeBinder FromResolve(object subIdentifier)
        {
            SubFinalizer = new ScopableBindingFinalizer(
                BindInfo,
                SingletonTypes.ToResolve, subIdentifier,
                (container, type) => new ResolveProvider(type, container, subIdentifier, false));

            return new ScopeBinder(BindInfo);
        }

        public SubContainerBinder FromSubContainerResolve()
        {
            return FromSubContainerResolve(null);
        }

        public SubContainerBinder FromSubContainerResolve(object subIdentifier)
        {
            return new SubContainerBinder(
                BindInfo, FinalizerWrapper, subIdentifier);
        }

#if !NOT_UNITY3D

        public ScopeArgBinder FromComponent(GameObject gameObject)
        {
            BindingUtil.AssertIsValidGameObject(gameObject);
            BindingUtil.AssertIsComponent(ConcreteTypes);
            BindingUtil.AssertTypesAreNotAbstract(ConcreteTypes);

            SubFinalizer = new ScopableBindingFinalizer(
                BindInfo, SingletonTypes.ToComponentGameObject, gameObject,
                (container, type) => new AddToExistingGameObjectComponentProvider(
                    gameObject, container, type, BindInfo.ConcreteIdentifier, BindInfo.Arguments));

            return new ScopeArgBinder(BindInfo);
        }

        public GameObjectNameGroupNameScopeArgBinder FromGameObject()
        {
            BindingUtil.AssertIsAbstractOrComponentOrGameObject(BindInfo.ContractTypes);
            BindingUtil.AssertIsComponentOrGameObject(ConcreteTypes);

            var gameObjectInfo = new GameObjectBindInfo();

            if (ConcreteTypes.All(x => x == typeof(GameObject)))
            {
                SubFinalizer = new ScopableBindingFinalizer(
                    BindInfo, SingletonTypes.ToGameObject, gameObjectInfo,
                    (container, type) =>
                    {
                        Assert.That(BindInfo.Arguments.IsEmpty(), "Cannot inject arguments into empty game object");
                        return new EmptyGameObjectProvider(
                            container, gameObjectInfo.Name, gameObjectInfo.GroupName);
                    });
            }
            else
            {
                BindingUtil.AssertIsComponent(ConcreteTypes);
                BindingUtil.AssertTypesAreNotAbstract(ConcreteTypes);

                SubFinalizer = new ScopableBindingFinalizer(
                    BindInfo, SingletonTypes.ToGameObject, gameObjectInfo,
                    (container, type) => new AddToNewGameObjectComponentProvider(
                        container,
                        type,
                        BindInfo.ConcreteIdentifier,
                        BindInfo.Arguments,
                        gameObjectInfo.Name,
                        gameObjectInfo.GroupName));
            }

            return new GameObjectNameGroupNameScopeArgBinder(BindInfo, gameObjectInfo);
        }

        public GameObjectNameGroupNameScopeArgBinder FromPrefab(GameObject prefab)
        {
            BindingUtil.AssertIsValidPrefab(prefab);
            BindingUtil.AssertIsAbstractOrComponentOrGameObject(AllParentTypes);

            var gameObjectInfo = new GameObjectBindInfo();

            SubFinalizer = new PrefabBindingFinalizer(
                BindInfo, gameObjectInfo, prefab);

            return new GameObjectNameGroupNameScopeArgBinder(BindInfo, gameObjectInfo);
        }

        public GameObjectNameGroupNameScopeArgBinder FromPrefabResource(string resourcePath)
        {
            BindingUtil.AssertIsValidResourcePath(resourcePath);
            BindingUtil.AssertIsAbstractOrComponentOrGameObject(AllParentTypes);

            var gameObjectInfo = new GameObjectBindInfo();

            SubFinalizer = new PrefabResourceBindingFinalizer(
                BindInfo, gameObjectInfo, resourcePath);

            return new GameObjectNameGroupNameScopeArgBinder(BindInfo, gameObjectInfo);
        }

        public ScopeBinder FromResource(string resourcePath)
        {
            BindingUtil.AssertDerivesFromUnityObject(ConcreteTypes);

            SubFinalizer = new ScopableBindingFinalizer(
                BindInfo,
                SingletonTypes.ToResource,
                resourcePath.ToLower(),
                (_, type) => new ResourceProvider(resourcePath, type));

            return new ScopeBinder(BindInfo);
        }

#endif

        protected ScopeArgBinder FromMethodBase<TConcrete>(Func<InjectContext, TConcrete> method)
        {
            BindingUtil.AssertIsDerivedFromTypes(typeof(TConcrete), AllParentTypes);

            SubFinalizer = new ScopableBindingFinalizer(
                BindInfo,
                SingletonTypes.ToMethod, new SingletonImplIds.ToMethod(method),
                (container, type) => new MethodProvider<TConcrete>(method, container));

            return this;
        }

        protected ScopeBinder FromFactoryBase<TConcrete, TFactory>()
            where TFactory : IFactory<TConcrete>
        {
            BindingUtil.AssertIsDerivedFromTypes(typeof(TConcrete), AllParentTypes);

            SubFinalizer = new ScopableBindingFinalizer(
                BindInfo,
                SingletonTypes.ToFactory, typeof(TFactory),
                (container, type) => new FactoryProvider<TConcrete, TFactory>(container));

            return new ScopeBinder(BindInfo);
        }

        protected ScopeBinder FromGetterBase<TObj, TResult>(
            object identifier, Func<TObj, TResult> method)
        {
            BindingUtil.AssertIsDerivedFromTypes(typeof(TResult), AllParentTypes);

            SubFinalizer = new ScopableBindingFinalizer(
                BindInfo,
                SingletonTypes.ToGetter,
                new SingletonImplIds.ToGetter(identifier, method),
                (container, type) => new GetterProvider<TObj, TResult>(identifier, method, container));

            return new ScopeBinder(BindInfo);
        }

        protected ScopeBinder FromInstanceBase(object instance)
        {
            BindingUtil.AssertInstanceDerivesFromOrEqual(instance, AllParentTypes);

            SubFinalizer = new ScopableBindingFinalizer(
                BindInfo, SingletonTypes.ToInstance, instance,
                (_, type) => new InstanceProvider(type, instance));

            return new ScopeBinder(BindInfo);
        }
    }
}
