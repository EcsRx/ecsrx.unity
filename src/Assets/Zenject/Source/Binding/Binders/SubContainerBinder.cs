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
    public class SubContainerBinder
    {
        readonly BindInfo _bindInfo;
        readonly BindFinalizerWrapper _finalizerWrapper;
        readonly object _subIdentifier;

        public SubContainerBinder(
            BindInfo bindInfo,
            BindFinalizerWrapper finalizerWrapper,
            object subIdentifier)
        {
            _bindInfo = bindInfo;
            _finalizerWrapper = finalizerWrapper;
            _subIdentifier = subIdentifier;

            // Reset in case the user ends the binding here
            finalizerWrapper.SubFinalizer = null;
        }

        protected IBindingFinalizer SubFinalizer
        {
            set
            {
                _finalizerWrapper.SubFinalizer = value;
            }
        }

        public ScopeBinder ByInstaller<TInstaller>()
            where TInstaller : Installer
        {
            return ByInstaller(typeof(TInstaller));
        }

        public ScopeBinder ByInstaller(Type installerType)
        {
            BindingUtil.AssertIsInstallerType(installerType);

            SubFinalizer = new SubContainerInstallerBindingFinalizer(
                _bindInfo, installerType, _subIdentifier);

            return new ScopeBinder(_bindInfo);
        }

        public ScopeBinder ByMethod(Action<DiContainer> installerMethod)
        {
            SubFinalizer = new SubContainerMethodBindingFinalizer(
                _bindInfo, installerMethod, _subIdentifier);

            return new ScopeBinder(_bindInfo);
        }

#if !NOT_UNITY3D

        public GameObjectNameGroupNameScopeBinder ByPrefab(GameObject prefab)
        {
            BindingUtil.AssertIsValidPrefab(prefab);

            var gameObjectInfo = new GameObjectBindInfo();

            SubFinalizer = new SubContainerPrefabBindingFinalizer(
                _bindInfo, gameObjectInfo, prefab, _subIdentifier);

            return new GameObjectNameGroupNameScopeBinder(_bindInfo, gameObjectInfo);
        }

        public GameObjectNameGroupNameScopeBinder ByPrefabResource(string resourcePath)
        {
            BindingUtil.AssertIsValidResourcePath(resourcePath);

            var gameObjectInfo = new GameObjectBindInfo();

            SubFinalizer = new SubContainerPrefabResourceBindingFinalizer(
                _bindInfo, gameObjectInfo, resourcePath, _subIdentifier);

            return new GameObjectNameGroupNameScopeBinder(_bindInfo, gameObjectInfo);
        }
#endif
    }
}
