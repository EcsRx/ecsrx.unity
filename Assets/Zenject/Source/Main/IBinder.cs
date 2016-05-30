using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using ModestTree.Util;

#if !NOT_UNITY3D
using UnityEngine;
#endif

namespace Zenject
{
    public interface IBinder
    {
        // _____ Bind<> _____
        // Map the given type to a way of obtaining it
        // See ITypeBinderOld.cs for the full list of methods to call on the return value
        // Note that this can include open generic types as well such as List<>
        ConcreteIdBinderGeneric<TContract> Bind<TContract>();

        // _____ Bind _____
        // Non-generic version of Bind<> for cases where you only have the runtime type
        // Note that this can include open generic types as well such as List<>
        ConcreteIdBinderNonGeneric Bind(params Type[] contractTypes);

        // _____ BindFactory<> _____
        // TBD
        FactoryToChoiceBinder<TContract> BindFactory<TContract, TFactory>()
            where TFactory : Factory<TContract>;

        FactoryToChoiceBinder<TParam1, TContract> BindFactory<TParam1, TContract, TFactory>()
            where TFactory : Factory<TParam1, TContract>;

        FactoryToChoiceBinder<TParam1, TParam2, TContract> BindFactory<TParam1, TParam2, TContract, TFactory>()
            where TFactory : Factory<TParam1, TParam2, TContract>;

        FactoryToChoiceBinder<TParam1, TParam2, TParam3, TContract> BindFactory<TParam1, TParam2, TParam3, TContract, TFactory>()
            where TFactory : Factory<TParam1, TParam2, TParam3, TContract>;

        FactoryToChoiceBinder<TParam1, TParam2, TParam3, TParam4, TContract> BindFactory<TParam1, TParam2, TParam3, TParam4, TContract, TFactory>()
            where TFactory : Factory<TParam1, TParam2, TParam3, TParam4, TContract>;

        FactoryToChoiceBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TContract> BindFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TContract, TFactory>()
            where TFactory : Factory<TParam1, TParam2, TParam3, TParam4, TParam5, TContract>;

        // _____ BindAllInterfaces _____
        // Bind all the interfaces for the given type to the same thing.
        //
        // Example:
        //
        //    public class Foo : ITickable, IInitializable
        //    {
        //    }
        //
        //    Container.BindAllInterfacesToSingle<Foo>();
        //
        //  This line above is equivalent to the following:
        //
        //    Container.Bind<ITickable>().ToSingle<Foo>();
        //    Container.Bind<IInitializable>().ToSingle<Foo>();
        //
        // Note here that we do not bind Foo to itself.  For that, use BindAllInterfacesAndSelf
        ConcreteIdBinderNonGeneric BindAllInterfaces<T>();
        ConcreteIdBinderNonGeneric BindAllInterfaces(Type type);

        // _____ BindAllInterfacesAndSelf _____
        // Same as BindAllInterfaces except also binds to self
        ConcreteIdBinderNonGeneric BindAllInterfacesAndSelf<T>();
        ConcreteIdBinderNonGeneric BindAllInterfacesAndSelf(Type type);

        // _____ BindInstance _____
        //  This is simply a shortcut to using the FromInstance method.
        //
        //  Example:
        //      Container.BindInstance(new Foo());
        //
        //  This line above is equivalent to the following:
        //
        //      Container.Bind<Foo>().FromInstance(new Foo());
        //
        IdScopeBinder BindInstance<TContract>(TContract obj);

        // _____ HasBinding _____
        // Returns true if the given type is bound to something in the container
        bool HasBinding(InjectContext context);
        bool HasBinding<TContract>();
        bool HasBinding<TContract>(object identifier);

        // _____ Install _____
        // This will cause the container to Inject all dependencies on the given installers,
        // and then call InstallBindings() on them to add more bindings to the container.
        void Install(IEnumerable<IInstaller> installers);
        void Install(IInstaller installer);
        void Install(IInstaller installer, IEnumerable<object> extraArgs);

        // _____ Install _____
        // This will create a new instance of the given type, inject all dependencies into it,
        // and then call InstallBindings() on them to add more bindings to the container.
        void Install<T>()
            where T : Installer;

        void Install<T>(IEnumerable<object> extraArgs)
            where T : Installer;

        void Install(Type installerType);
        void Install(Type installerType, IEnumerable<object> extraArgs);

        // This is only necessary if you have to pass in null values as parameters to the installer
        void InstallExplicit(Type installerType, List<TypeValuePair> extraArgs);

#if !NOT_UNITY3D
        // _____ InstallPrefab _____
        void InstallPrefab<T>()
            where T : MonoInstaller;

        void InstallPrefab<T>(IEnumerable<object> extraArgs)
            where T : MonoInstaller;

        void InstallPrefab(Type installerType);
        void InstallPrefab(Type installerType, IEnumerable<object> extraArgs);

        // This is only necessary if you have to pass in null values as parameters to the installer
        void InstallPrefabExplicit(Type installerType, List<TypeValuePair> extraArgs);

        // _____ InstallScriptableObject _____

        void InstallScriptableObject<T>()
            where T : ScriptableObjectInstaller;

        void InstallScriptableObject<T>(IEnumerable<object> extraArgs)
            where T : ScriptableObjectInstaller;

        void InstallScriptableObject(Type installerType);
        void InstallScriptableObject(Type installerType, IEnumerable<object> extraArgs);
        void InstallScriptableObjectExplicit(Type installerType, List<TypeValuePair> extraArgs);
#endif

        // _____ InstallExplicit _____
        void InstallExplicit(IInstaller installer, List<TypeValuePair> extraArgs);

        // _____ Install _____
        // Returns true if the given installer type has already been installed on this container
        bool HasInstalled(Type installerType);

        bool HasInstalled<T>()
            where T : IInstaller;
    }
}
