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
    public abstract class FromBinder : ScopeConcreteIdArgConditionCopyNonLazyBinder
    {
        public FromBinder(
            DiContainer bindContainer, BindInfo bindInfo,
            BindFinalizerWrapper finalizerWrapper)
            : base(bindInfo)
        {
            FinalizerWrapper = finalizerWrapper;
            BindContainer = bindContainer;
        }

        protected DiContainer BindContainer
        {
            get; private set;
        }

        protected BindFinalizerWrapper FinalizerWrapper
        {
            get;
            private set;
        }

        protected IBindingFinalizer SubFinalizer
        {
            set { FinalizerWrapper.SubFinalizer = value; }
        }

        protected IEnumerable<Type> AllParentTypes
        {
            get { return BindInfo.ContractTypes.Concat(BindInfo.ToTypes); }
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
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromNew()
        {
            BindingUtil.AssertTypesAreNotComponents(ConcreteTypes);
            BindingUtil.AssertTypesAreNotAbstract(ConcreteTypes);

            return this;
        }

        public ScopeConditionCopyNonLazyBinder FromResolve()
        {
            return FromResolve(null);
        }

        public ScopeConditionCopyNonLazyBinder FromResolve(object subIdentifier)
        {
            return FromResolve(subIdentifier, InjectSources.Any);
        }

        public ScopeConditionCopyNonLazyBinder FromResolve(object subIdentifier, InjectSources source)
        {
            return FromResolveInternal(subIdentifier, false, source);
        }

        public ScopeConditionCopyNonLazyBinder FromResolveAll()
        {
            return FromResolveAll(null);
        }

        public ScopeConditionCopyNonLazyBinder FromResolveAll(object subIdentifier)
        {
            return FromResolveAll(subIdentifier, InjectSources.Any);
        }

        public ScopeConditionCopyNonLazyBinder FromResolveAll(object subIdentifier, InjectSources source)
        {
            return FromResolveInternal(subIdentifier, true, source);
        }

        ScopeConditionCopyNonLazyBinder FromResolveInternal(object subIdentifier, bool matchAll, InjectSources source)
        {
            BindInfo.RequireExplicitScope = false;
            // Don't know how it's created so can't assume here that it violates AsSingle
            BindInfo.MarkAsCreationBinding = false;

            SubFinalizer = new ScopableBindingFinalizer(
                BindInfo,
                (container, type) => new ResolveProvider(
                    type, container, subIdentifier, false, source, matchAll));

            return new ScopeConditionCopyNonLazyBinder(BindInfo);
        }

        public SubContainerBinder FromSubContainerResolveAll()
        {
            return FromSubContainerResolveAll(null);
        }

        public SubContainerBinder FromSubContainerResolveAll(object subIdentifier)
        {
            return FromSubContainerResolveInternal(subIdentifier, true);
        }

        public SubContainerBinder FromSubContainerResolve()
        {
            return FromSubContainerResolve(null);
        }

        public SubContainerBinder FromSubContainerResolve(object subIdentifier)
        {
            return FromSubContainerResolveInternal(subIdentifier, false);
        }

        SubContainerBinder FromSubContainerResolveInternal(
            object subIdentifier, bool resolveAll)
        {
            // It's unlikely they will want to create the whole subcontainer with each binding
            // (aka transient) which is the default so require that they specify it
            BindInfo.RequireExplicitScope = true;
            // Don't know how it's created so can't assume here that it violates AsSingle
            BindInfo.MarkAsCreationBinding = false;

            return new SubContainerBinder(
                BindInfo, FinalizerWrapper, subIdentifier, resolveAll);
        }

        protected ScopeConcreteIdArgConditionCopyNonLazyBinder FromIFactoryBase<TContract>(
            Action<ConcreteBinderGeneric<IFactory<TContract>>> factoryBindGenerator)
        {
            // Use a random ID so that our provider is the only one that can find it and so it doesn't
            // conflict with anything else
            var factoryId = Guid.NewGuid();

            // Important to use NoFlush here otherwise the main binding will finalize early
            var subBinder = BindContainer.BindNoFlush<IFactory<TContract>>()
                .WithId(factoryId);

            factoryBindGenerator(subBinder);

            // This is kind of like a look up method like FromMethod so don't enforce specifying scope
            // The internal binding will require an explicit scope so should be obvious enough
            BindInfo.RequireExplicitScope = false;
            // Don't know how it's created so can't assume here that it violates AsSingle
            BindInfo.MarkAsCreationBinding = false;

            SubFinalizer = new ScopableBindingFinalizer(
                BindInfo,
                (container, type) => new IFactoryProvider<TContract>(container, factoryId));

            var binder = new ScopeConcreteIdArgConditionCopyNonLazyBinder(BindInfo);
            // Needed for example if the user uses MoveIntoDirectSubContainers
            binder.AddSecondaryCopyBindInfo(subBinder.BindInfo);
            return binder;
        }

#if !NOT_UNITY3D

        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromComponentsOn(GameObject gameObject)
        {
            BindingUtil.AssertIsValidGameObject(gameObject);
            BindingUtil.AssertIsComponent(ConcreteTypes);
            BindingUtil.AssertTypesAreNotAbstract(ConcreteTypes);

            BindInfo.RequireExplicitScope = true;
            SubFinalizer = new ScopableBindingFinalizer(
                BindInfo,
                (container, type) => new GetFromGameObjectComponentProvider(
                    type, gameObject, false));

            return new ScopeConcreteIdArgConditionCopyNonLazyBinder(BindInfo);
        }

        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromComponentOn(GameObject gameObject)
        {
            BindingUtil.AssertIsValidGameObject(gameObject);
            BindingUtil.AssertIsComponent(ConcreteTypes);
            BindingUtil.AssertTypesAreNotAbstract(ConcreteTypes);

            BindInfo.RequireExplicitScope = true;
            SubFinalizer = new ScopableBindingFinalizer(
                BindInfo,
                (container, type) => new GetFromGameObjectComponentProvider(
                    type, gameObject, true));

            return new ScopeConcreteIdArgConditionCopyNonLazyBinder(BindInfo);
        }

        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromComponentsOn(Func<InjectContext, GameObject> gameObjectGetter)
        {
            BindingUtil.AssertIsComponent(ConcreteTypes);
            BindingUtil.AssertTypesAreNotAbstract(ConcreteTypes);

            BindInfo.RequireExplicitScope = false;
            SubFinalizer = new ScopableBindingFinalizer(
                BindInfo,
                (container, type) => new GetFromGameObjectGetterComponentProvider(
                    type, gameObjectGetter, false));

            return new ScopeConcreteIdArgConditionCopyNonLazyBinder(BindInfo);
        }

        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromComponentOn(Func<InjectContext, GameObject> gameObjectGetter)
        {
            BindingUtil.AssertIsComponent(ConcreteTypes);
            BindingUtil.AssertTypesAreNotAbstract(ConcreteTypes);

            BindInfo.RequireExplicitScope = false;
            SubFinalizer = new ScopableBindingFinalizer(
                BindInfo,
                (container, type) => new GetFromGameObjectGetterComponentProvider(
                    type, gameObjectGetter, true));

            return new ScopeConcreteIdArgConditionCopyNonLazyBinder(BindInfo);
        }

        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromComponentsOnRoot()
        {
            return FromComponentsOn(
                ctx => BindContainer.Resolve<Context>().gameObject);
        }

        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromComponentOnRoot()
        {
            return FromComponentOn(
                ctx => BindContainer.Resolve<Context>().gameObject);
        }

        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromNewComponentOn(GameObject gameObject)
        {
            BindingUtil.AssertIsValidGameObject(gameObject);
            BindingUtil.AssertIsComponent(ConcreteTypes);
            BindingUtil.AssertTypesAreNotAbstract(ConcreteTypes);

            BindInfo.RequireExplicitScope = true;
            SubFinalizer = new ScopableBindingFinalizer(
                BindInfo,
                (container, type) => new AddToExistingGameObjectComponentProvider(
                    gameObject, container, type, BindInfo.Arguments, BindInfo.ConcreteIdentifier));

            return new ScopeConcreteIdArgConditionCopyNonLazyBinder(BindInfo);
        }

        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromNewComponentOn(Func<InjectContext, GameObject> gameObjectGetter)
        {
            BindingUtil.AssertIsComponent(ConcreteTypes);
            BindingUtil.AssertTypesAreNotAbstract(ConcreteTypes);

            BindInfo.RequireExplicitScope = true;
            SubFinalizer = new ScopableBindingFinalizer(
                BindInfo,
                (container, type) => new AddToExistingGameObjectComponentProviderGetter(
                    gameObjectGetter, container, type, BindInfo.Arguments, BindInfo.ConcreteIdentifier));

            return new ScopeConcreteIdArgConditionCopyNonLazyBinder(BindInfo);
        }

        public ArgConditionCopyNonLazyBinder FromNewComponentSibling()
        {
            BindingUtil.AssertIsComponent(ConcreteTypes);
            BindingUtil.AssertTypesAreNotAbstract(ConcreteTypes);

            BindInfo.RequireExplicitScope = true;
            SubFinalizer = new SingleProviderBindingFinalizer(
                BindInfo, (container, type) => new AddToCurrentGameObjectComponentProvider(
                    container, type, BindInfo.Arguments, BindInfo.ConcreteIdentifier));

            return new ArgConditionCopyNonLazyBinder(BindInfo);
        }

        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromNewComponentOnRoot()
        {
            return FromNewComponentOn(
                ctx => BindContainer.Resolve<Context>().gameObject);
        }

        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder FromNewComponentOnNewGameObject()
        {
            return FromNewComponentOnNewGameObject(new GameObjectCreationParameters());
        }

        internal NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder FromNewComponentOnNewGameObject(
            GameObjectCreationParameters gameObjectInfo)
        {
            BindingUtil.AssertIsComponent(ConcreteTypes);
            BindingUtil.AssertTypesAreNotAbstract(ConcreteTypes);

            BindInfo.RequireExplicitScope = true;
            SubFinalizer = new ScopableBindingFinalizer(
                BindInfo,
                (container, type) => new AddToNewGameObjectComponentProvider(
                    container,
                    type,
                    BindInfo.Arguments,
                    gameObjectInfo, BindInfo.ConcreteIdentifier));

            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(BindInfo, gameObjectInfo);
        }

        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder FromNewComponentOnNewPrefabResource(string resourcePath)
        {
            return FromNewComponentOnNewPrefabResource(resourcePath, new GameObjectCreationParameters());
        }

        internal NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder FromNewComponentOnNewPrefabResource(
            string resourcePath, GameObjectCreationParameters gameObjectInfo)
        {
            BindingUtil.AssertIsValidResourcePath(resourcePath);
            BindingUtil.AssertIsComponent(ConcreteTypes);
            BindingUtil.AssertTypesAreNotAbstract(ConcreteTypes);

            BindInfo.RequireExplicitScope = true;
            SubFinalizer = new PrefabResourceBindingFinalizer(
                BindInfo, gameObjectInfo, resourcePath,
                (contractType, instantiator) => new InstantiateOnPrefabComponentProvider(contractType, instantiator));

            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(BindInfo, gameObjectInfo);
        }

        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder FromNewComponentOnNewPrefab(UnityEngine.Object prefab)
        {
            return FromNewComponentOnNewPrefab(prefab, new GameObjectCreationParameters());
        }

        internal NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder FromNewComponentOnNewPrefab(
            UnityEngine.Object prefab, GameObjectCreationParameters gameObjectInfo)
        {
            BindingUtil.AssertIsValidPrefab(prefab);
            BindingUtil.AssertIsComponent(ConcreteTypes);
            BindingUtil.AssertTypesAreNotAbstract(ConcreteTypes);

            BindInfo.RequireExplicitScope = true;
            SubFinalizer = new PrefabBindingFinalizer(
                BindInfo, gameObjectInfo, prefab,
                (contractType, instantiator) => new InstantiateOnPrefabComponentProvider(contractType, instantiator));

            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(BindInfo, gameObjectInfo);
        }

        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder FromComponentInNewPrefab(UnityEngine.Object prefab)
        {
            return FromComponentInNewPrefab(
                prefab, new GameObjectCreationParameters());
        }

        internal NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder FromComponentInNewPrefab(
            UnityEngine.Object prefab, GameObjectCreationParameters gameObjectInfo)
        {
            BindingUtil.AssertIsValidPrefab(prefab);
            BindingUtil.AssertIsInterfaceOrComponent(AllParentTypes);

            BindInfo.RequireExplicitScope = true;
            SubFinalizer = new PrefabBindingFinalizer(
                BindInfo, gameObjectInfo, prefab,
                (contractType, instantiator) => new GetFromPrefabComponentProvider(contractType, instantiator, true));

            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(BindInfo, gameObjectInfo);
        }

        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder FromComponentsInNewPrefab(UnityEngine.Object prefab)
        {
            return FromComponentsInNewPrefab(
                prefab, new GameObjectCreationParameters());
        }

        internal NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder FromComponentsInNewPrefab(
            UnityEngine.Object prefab, GameObjectCreationParameters gameObjectInfo)
        {
            BindingUtil.AssertIsValidPrefab(prefab);
            BindingUtil.AssertIsInterfaceOrComponent(AllParentTypes);

            BindInfo.RequireExplicitScope = true;
            SubFinalizer = new PrefabBindingFinalizer(
                BindInfo, gameObjectInfo, prefab,
                (contractType, instantiator) => new GetFromPrefabComponentProvider(contractType, instantiator, false));

            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(BindInfo, gameObjectInfo);
        }

        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder FromComponentInNewPrefabResource(string resourcePath)
        {
            return FromComponentInNewPrefabResource(resourcePath, new GameObjectCreationParameters());
        }

        internal NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder FromComponentInNewPrefabResource(
            string resourcePath, GameObjectCreationParameters gameObjectInfo)
        {
            BindingUtil.AssertIsValidResourcePath(resourcePath);
            BindingUtil.AssertIsInterfaceOrComponent(AllParentTypes);

            BindInfo.RequireExplicitScope = true;
            SubFinalizer = new PrefabResourceBindingFinalizer(
                BindInfo, gameObjectInfo, resourcePath,
                (contractType, instantiator) => new GetFromPrefabComponentProvider(contractType, instantiator, true));

            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(BindInfo, gameObjectInfo);
        }

        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder FromComponentsInNewPrefabResource(string resourcePath)
        {
            return FromComponentsInNewPrefabResource(resourcePath, new GameObjectCreationParameters());
        }

        internal NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder FromComponentsInNewPrefabResource(
            string resourcePath, GameObjectCreationParameters gameObjectInfo)
        {
            BindingUtil.AssertIsValidResourcePath(resourcePath);
            BindingUtil.AssertIsInterfaceOrComponent(AllParentTypes);

            BindInfo.RequireExplicitScope = true;
            SubFinalizer = new PrefabResourceBindingFinalizer(
                BindInfo, gameObjectInfo, resourcePath,
                (contractType, instantiator) => new GetFromPrefabComponentProvider(contractType, instantiator, false));

            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(BindInfo, gameObjectInfo);
        }

        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromNewScriptableObjectResource(string resourcePath)
        {
            return FromScriptableObjectResourceInternal(resourcePath, true);
        }

        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromScriptableObjectResource(string resourcePath)
        {
            return FromScriptableObjectResourceInternal(resourcePath, false);
        }

        ScopeConcreteIdArgConditionCopyNonLazyBinder FromScriptableObjectResourceInternal(
            string resourcePath, bool createNew)
        {
            BindingUtil.AssertIsValidResourcePath(resourcePath);
            BindingUtil.AssertIsInterfaceOrScriptableObject(AllParentTypes);

            BindInfo.RequireExplicitScope = true;
            SubFinalizer = new ScopableBindingFinalizer(
                BindInfo,
                (container, type) => new ScriptableObjectResourceProvider(
                    resourcePath, type, container, BindInfo.Arguments, createNew, BindInfo.ConcreteIdentifier));

            return new ScopeConcreteIdArgConditionCopyNonLazyBinder(BindInfo);
        }

        public ScopeConditionCopyNonLazyBinder FromResource(string resourcePath)
        {
            BindingUtil.AssertDerivesFromUnityObject(ConcreteTypes);

            BindInfo.RequireExplicitScope = false;
            SubFinalizer = new ScopableBindingFinalizer(
                BindInfo,
                (_, type) => new ResourceProvider(resourcePath, type, true));

            return new ScopeConditionCopyNonLazyBinder(BindInfo);
        }

        public ScopeConditionCopyNonLazyBinder FromResources(string resourcePath)
        {
            BindingUtil.AssertDerivesFromUnityObject(ConcreteTypes);

            BindInfo.RequireExplicitScope = false;
            SubFinalizer = new ScopableBindingFinalizer(
                BindInfo,
                (_, type) => new ResourceProvider(resourcePath, type, false));

            return new ScopeConditionCopyNonLazyBinder(BindInfo);
        }

#endif

        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromMethodUntyped(Func<InjectContext, object> method)
        {
            BindInfo.RequireExplicitScope = false;
            // Don't know how it's created so can't assume here that it violates AsSingle
            BindInfo.MarkAsCreationBinding = false;
            SubFinalizer = new ScopableBindingFinalizer(
                BindInfo,
                (container, type) => new MethodProviderUntyped(method, container));

            return this;
        }

        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromMethodMultipleUntyped(Func<InjectContext, IEnumerable<object>> method)
        {
            BindInfo.RequireExplicitScope = false;
            // Don't know how it's created so can't assume here that it violates AsSingle
            BindInfo.MarkAsCreationBinding = false;
            SubFinalizer = new ScopableBindingFinalizer(
                BindInfo,
                (container, type) => new MethodMultipleProviderUntyped(method, container));

            return this;
        }

        protected ScopeConcreteIdArgConditionCopyNonLazyBinder FromMethodBase<TConcrete>(Func<InjectContext, TConcrete> method)
        {
            BindingUtil.AssertIsDerivedFromTypes(typeof(TConcrete), AllParentTypes);

            BindInfo.RequireExplicitScope = false;
            // Don't know how it's created so can't assume here that it violates AsSingle
            BindInfo.MarkAsCreationBinding = false;
            SubFinalizer = new ScopableBindingFinalizer(
                BindInfo,
                (container, type) => new MethodProvider<TConcrete>(method, container));

            return this;
        }

        protected ScopeConcreteIdArgConditionCopyNonLazyBinder FromMethodMultipleBase<TConcrete>(Func<InjectContext, IEnumerable<TConcrete>> method)
        {
            BindingUtil.AssertIsDerivedFromTypes(typeof(TConcrete), AllParentTypes);

            BindInfo.RequireExplicitScope = false;
            // Don't know how it's created so can't assume here that it violates AsSingle
            BindInfo.MarkAsCreationBinding = false;
            SubFinalizer = new ScopableBindingFinalizer(
                BindInfo,
                (container, type) => new MethodProviderMultiple<TConcrete>(method, container));

            return this;
        }

        protected ScopeConditionCopyNonLazyBinder FromResolveGetterBase<TObj, TResult>(
            object identifier, Func<TObj, TResult> method, InjectSources source, bool matchMultiple)
        {
            BindingUtil.AssertIsDerivedFromTypes(typeof(TResult), AllParentTypes);

            BindInfo.RequireExplicitScope = false;
            // Don't know how it's created so can't assume here that it violates AsSingle
            BindInfo.MarkAsCreationBinding = false;
            SubFinalizer = new ScopableBindingFinalizer(
                BindInfo,
                (container, type) => new GetterProvider<TObj, TResult>(identifier, method, container, source, matchMultiple));

            return new ScopeConditionCopyNonLazyBinder(BindInfo);
        }

        protected ScopeConditionCopyNonLazyBinder FromInstanceBase(object instance)
        {
            BindingUtil.AssertInstanceDerivesFromOrEqual(instance, AllParentTypes);

            BindInfo.RequireExplicitScope = false;
            // Don't know how it's created so can't assume here that it violates AsSingle
            BindInfo.MarkAsCreationBinding = false;
            SubFinalizer = new ScopableBindingFinalizer(
                BindInfo,
                (container, type) => new InstanceProvider(type, instance, container));

            return new ScopeConditionCopyNonLazyBinder(BindInfo);
        }
    }
}
