using System;
#if !NOT_UNITY3D
using UnityEngine;
#endif
using ModestTree;

namespace Zenject
{
    public static class SubContainerCreatorUtil
    {
        public static void ApplyBindSettings(
            SubContainerCreatorBindInfo subContainerBindInfo, DiContainer subContainer)
        {
            if (subContainerBindInfo.DefaultParentName != null)
            {
#if !ZEN_TESTS_OUTSIDE_UNITY && !NOT_UNITY3D
                var defaultParent = new GameObject(
                    subContainerBindInfo.DefaultParentName);

                defaultParent.transform.SetParent(
                    subContainer.InheritedDefaultParent, false);

                subContainer.DefaultParent = defaultParent.transform;

                subContainer.Bind<IDisposable>()
                    .To<DefaultParentObjectDestroyer>().AsCached().WithArguments(defaultParent);

                // Always destroy the default parent last so that the non-monobehaviours get a chance
                // to clean it up if they want to first
                subContainer.BindDisposableExecutionOrder<DefaultParentObjectDestroyer>(int.MinValue);
#endif
            }

            if (subContainerBindInfo.CreateKernel)
            {
                var parentContainer = subContainer.ParentContainers.OnlyOrDefault();
                Assert.IsNotNull(parentContainer, "Could not find unique container when using WithKernel!");

                if (subContainerBindInfo.KernelType != null)
                {
                    parentContainer.Bind(typeof(Kernel).Interfaces()).To(subContainerBindInfo.KernelType)
                        .FromSubContainerResolve()
                        .ByInstance(subContainer).AsCached();
                    subContainer.Bind(subContainerBindInfo.KernelType).AsCached();
                }
                else
                {
                    parentContainer.BindInterfacesTo<Kernel>().FromSubContainerResolve()
                        .ByInstance(subContainer).AsCached();
                    subContainer.Bind<Kernel>().AsCached();
                }
            }
        }

#if !NOT_UNITY3D
        class DefaultParentObjectDestroyer : IDisposable
        {
            readonly GameObject _gameObject;

            public DefaultParentObjectDestroyer(GameObject gameObject)
            {
                _gameObject = gameObject;
            }

            public void Dispose()
            {
                GameObject.Destroy(_gameObject);
            }
        }
#endif
    }
}
