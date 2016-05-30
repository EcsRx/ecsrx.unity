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
    public class InjectArgs
    {
        public List<TypeValuePair> ExtraArgs;
        public bool UseAllArgs;
        public ZenjectTypeInfo TypeInfo;
        public InjectContext Context;
        public string ConcreteIdentifier;
    }

    public interface IResolver
    {
        // When you call any of these Inject methods
        //    Any fields marked [Inject] will be set using the bindings on the container
        //    Any methods marked with a [Inject] will be called
        //    Any constructor parameters will be filled in with values from the container
        void Inject(object injectable);
        void Inject(object injectable, IEnumerable<object> extraArgs);

        // InjectExplicit is only necessary when you want to inject null values into your object
        // otherwise you can just use Inject()
        // Note: Any arguments that are used will be removed from extraArgMap
        void InjectExplicit(
            object injectable, List<TypeValuePair> extraArgs);

        // NOTE: Any arguments taken from extraArgs will be removed from the given list
        void InjectExplicit(
            object injectable, InjectArgs args);

        // Resolve<> - Lookup a value in the container.
        //
        // Note that this may result in a new object being created (for transient bindings) or it
        // may return an already created object (for FromInstance or ToSingle, etc. bindings)
        //
        // If a single unique value for the given type cannot be found, an exception is thrown.
        //
        TContract Resolve<TContract>();
        TContract Resolve<TContract>(object identifier);
        // InjectContext can be used to add more constraints to the object that you want to retrieve
        TContract Resolve<TContract>(InjectContext context);

        // Non-generic versions
        object Resolve(Type contractType);
        object Resolve(Type contractType, object identifier);
        object Resolve(InjectContext context);

        // Same as Resolve<> except it will return null if a value for the given type cannot
        // be found.
        TContract TryResolve<TContract>()
            where TContract : class;
        TContract TryResolve<TContract>(object identifier)
            where TContract : class;

        object TryResolve(Type contractType);
        object TryResolve(Type contractType, object identifier);

        // Same as Resolve<> except it will return all bindings that are associated with the given type
        List<TContract> ResolveAll<TContract>();
        List<TContract> ResolveAll<TContract>(bool optional);
        List<TContract> ResolveAll<TContract>(object identifier);
        List<TContract> ResolveAll<TContract>(object identifier, bool optional);
        List<TContract> ResolveAll<TContract>(InjectContext context);

        // Untyped versions
        IList ResolveAll(InjectContext context);

        IList ResolveAll(Type contractType);
        IList ResolveAll(Type contractType, object identifier);
        IList ResolveAll(Type contractType, bool optional);
        IList ResolveAll(Type contractType, object identifier, bool optional);

        // Returns all the types that would be returned if ResolveAll was called with the given values
        List<Type> ResolveTypeAll(InjectContext context);
        List<Type> ResolveTypeAll(Type type);

#if !NOT_UNITY3D
        // Inject dependencies into any and all child components on the given game object
        void InjectGameObject(
            GameObject gameObject);

        void InjectGameObject(
            GameObject gameObject, bool recursive);

        void InjectGameObject(
            GameObject gameObject, bool recursive, 
            IEnumerable<object> extraArgs);

        void InjectGameObjectExplicit(
            GameObject gameObject, bool recursive, 
            List<TypeValuePair> extraArgs);

        void InjectGameObjectExplicit(
            GameObject gameObject, bool recursive, 
            List<TypeValuePair> extraArgs, bool useAllArgs);
#endif
    }
}
