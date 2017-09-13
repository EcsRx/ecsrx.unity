#if !NOT_UNITY3D

using System;
using UnityEngine;
using ModestTree;

namespace Zenject
{
    // We'd prefer to make this abstract but Unity 5.3.5 has a bug where references
    // can get lost during compile errors for classes that are abstract
    public class MonoInstaller : MonoInstallerBase
    {
    }

    //
    // Derive from this class instead to install like this:
    //     FooInstaller.InstallFromResource(Container);
    // Or
    //     FooInstaller.InstallFromResource("My/Path/ToPrefab", Container);
    //
    // (Instead of needing to add the MonoInstaller via inspector)
    //
    // This approach is needed if you want to pass in strongly parameters to it from
    // another installer
    public class MonoInstaller<TDerived> : MonoInstaller
        where TDerived : MonoInstaller<TDerived>
    {
        public static void InstallFromResource(DiContainer container)
        {
            InstallFromResource(MonoInstallerUtil.GetDefaultResourcePath<TDerived>(), container);
        }

        public static void InstallFromResource(string resourcePath, DiContainer container)
        {
            InstallFromResource(resourcePath, container, new object[0]);
        }

        public static void InstallFromResource(DiContainer container, object[] extraArgs)
        {
            InstallFromResource(MonoInstallerUtil.GetDefaultResourcePath<TDerived>(), container, extraArgs);
        }

        public static void InstallFromResource(string resourcePath, DiContainer container, object[] extraArgs)
        {
            var installer = MonoInstallerUtil.CreateInstaller<TDerived>(resourcePath, container);
            container.Inject(installer, extraArgs);
            installer.InstallBindings();
        }
    }

    public class MonoInstaller<TParam1, TDerived> : MonoInstallerBase
        where TDerived : MonoInstaller<TParam1, TDerived>
    {
        public static void InstallFromResource(DiContainer container, TParam1 p1)
        {
            InstallFromResource(MonoInstallerUtil.GetDefaultResourcePath<TDerived>(), container, p1);
        }

        public static void InstallFromResource(string resourcePath, DiContainer container, TParam1 p1)
        {
            var installer = MonoInstallerUtil.CreateInstaller<TDerived>(resourcePath, container);
            container.InjectExplicit(installer, InjectUtil.CreateArgListExplicit(p1));
            installer.InstallBindings();
        }
    }

    public class MonoInstaller<TParam1, TParam2, TDerived> : MonoInstallerBase
        where TDerived : MonoInstaller<TParam1, TParam2, TDerived>
    {
        public static void InstallFromResource(DiContainer container, TParam1 p1, TParam2 p2)
        {
            InstallFromResource(MonoInstallerUtil.GetDefaultResourcePath<TDerived>(), container, p1, p2);
        }

        public static void InstallFromResource(string resourcePath, DiContainer container, TParam1 p1, TParam2 p2)
        {
            var installer = MonoInstallerUtil.CreateInstaller<TDerived>(resourcePath, container);
            container.InjectExplicit(installer, InjectUtil.CreateArgListExplicit(p1, p2));
            installer.InstallBindings();
        }
    }

    public class MonoInstaller<TParam1, TParam2, TParam3, TDerived> : MonoInstallerBase
        where TDerived : MonoInstaller<TParam1, TParam2, TParam3, TDerived>
    {
        public static void InstallFromResource(DiContainer container, TParam1 p1, TParam2 p2, TParam3 p3)
        {
            InstallFromResource(MonoInstallerUtil.GetDefaultResourcePath<TDerived>(), container, p1, p2, p3);
        }

        public static void InstallFromResource(string resourcePath, DiContainer container, TParam1 p1, TParam2 p2, TParam3 p3)
        {
            var installer = MonoInstallerUtil.CreateInstaller<TDerived>(resourcePath, container);
            container.InjectExplicit(installer, InjectUtil.CreateArgListExplicit(p1, p2, p3));
            installer.InstallBindings();
        }
    }

    public class MonoInstaller<TParam1, TParam2, TParam3, TParam4, TDerived> : MonoInstallerBase
        where TDerived : MonoInstaller<TParam1, TParam2, TParam3, TParam4, TDerived>
    {
        public static void InstallFromResource(DiContainer container, TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4)
        {
            InstallFromResource(MonoInstallerUtil.GetDefaultResourcePath<TDerived>(), container, p1, p2, p3, p4);
        }

        public static void InstallFromResource(string resourcePath, DiContainer container, TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4)
        {
            var installer = MonoInstallerUtil.CreateInstaller<TDerived>(resourcePath, container);
            container.InjectExplicit(installer, InjectUtil.CreateArgListExplicit(p1, p2, p3, p4));
            installer.InstallBindings();
        }
    }

    public class MonoInstaller<TParam1, TParam2, TParam3, TParam4, TParam5, TDerived> : MonoInstallerBase
        where TDerived : MonoInstaller<TParam1, TParam2, TParam3, TParam4, TParam5, TDerived>
    {
        public static void InstallFromResource(DiContainer container, TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5)
        {
            InstallFromResource(MonoInstallerUtil.GetDefaultResourcePath<TDerived>(), container, p1, p2, p3, p4, p5);
        }

        public static void InstallFromResource(string resourcePath, DiContainer container, TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5)
        {
            var installer = MonoInstallerUtil.CreateInstaller<TDerived>(resourcePath, container);
            container.InjectExplicit(installer, InjectUtil.CreateArgListExplicit(p1, p2, p3, p4, p5));
            installer.InstallBindings();
        }
    }

    public static class MonoInstallerUtil
    {
        public static string GetDefaultResourcePath<TInstaller>()
            where TInstaller : MonoInstallerBase
        {
            return "Installers/" + typeof(TInstaller).Name();
        }

        public static TInstaller CreateInstaller<TInstaller>(
            string resourcePath, DiContainer container)
            where TInstaller : MonoInstallerBase
        {
            bool shouldMakeActive;
            var gameObj = container.CreateAndParentPrefabResource(
                resourcePath, GameObjectCreationParameters.Default, null, out shouldMakeActive);

            if (shouldMakeActive)
            {
                gameObj.SetActive(true);
            }

            var installers = gameObj.GetComponentsInChildren<TInstaller>();

            Assert.That(installers.Length == 1,
                "Could not find unique MonoInstaller with type '{0}' on prefab '{1}'", typeof(TInstaller), gameObj.name);

            return installers[0];
        }
    }
}

#endif
