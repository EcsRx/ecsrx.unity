#if !NOT_UNITY3D

using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using ModestTree.Util;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject.Internal;

#pragma warning disable 649

namespace Zenject
{
    public class GameObjectContext : Context
    {
        readonly List<object> _dependencyRoots = new List<object>();

        [SerializeField]
        [Tooltip("Note that this field is optional and can be ignored in most cases.  This is really only needed if you want to control the 'Script Execution Order' of your subcontainer.  In this case, define a new class that derives from MonoKernel, add it to this game object, then drag it into this field.  Then you can set a value for 'Script Execution Order' for this new class and this will control when all ITickable/IInitializable classes bound within this subcontainer get called.")]
        [FormerlySerializedAs("_facade")]
        MonoKernel _kernel;

        DiContainer _container;

        public override DiContainer Container
        {
            get
            {
                return _container;
            }
        }

        [Inject]
        public void Construct(
            DiContainer parentContainer,
            [InjectOptional]
            InstallerExtraArgs installerExtraArgs)
        {
            Assert.IsNull(_container);

            _container = parentContainer.CreateSubContainer();

            var componentInjecter = new InitialComponentsInjecter(
                _container, GetInjectableComponents().ToList());

            foreach (var component in componentInjecter.Components)
            {
                if (component is MonoKernel)
                {
                    Assert.That(component == _kernel,
                        "Found MonoKernel derived class that is not hooked up to GameObjectContext.  If you use MonoKernel, you must indicate this to GameObjectContext by dragging and dropping it to the Kernel field in the inspector");
                }
            }

            _container.IsInstalling = true;

            try
            {
                InstallBindings(installerExtraArgs, componentInjecter);
            }
            finally
            {
                _container.IsInstalling = false;
            }

            Log.Debug("GameObjectContext: Injecting into child components...");

            componentInjecter.LazyInjectComponents();

            Assert.That(_dependencyRoots.IsEmpty());
            _dependencyRoots.AddRange(_container.ResolveDependencyRoots());

            Log.Debug("GameObjectContext: Initialized successfully");
        }

        protected override IEnumerable<Component> GetInjectableComponents()
        {
            // We inject on all components on the root except ourself
            foreach (var component in GetComponents<Component>())
            {
                if (component == null)
                {
                    Log.Warn("Zenject: Found null component on game object '{0}'.  Possible missing script.", gameObject.name);
                    continue;
                }

                if (component.GetType().DerivesFrom<MonoInstaller>())
                {
                    // Do not inject on installers since these are always injected before they are installed
                    continue;
                }

                if (component == this)
                {
                    continue;
                }

                yield return component;
            }

            foreach (var gameObject in UnityUtil.GetDirectChildren(this.gameObject))
            {
                foreach (var component in GetInjectableComponents(gameObject))
                {
                    yield return component;
                }
            }
        }

        void InstallBindings(
            InstallerExtraArgs installerExtraArgs, InitialComponentsInjecter componentInjecter)
        {
            _container.DefaultParent = this.transform;

            _container.Bind<Context>().FromInstance(this);

            if (_kernel == null)
            {
                _container.Bind<MonoKernel>()
                    .To<DefaultGameObjectKernel>().FromComponent(this.gameObject).AsSingle().NonLazy();
            }
            else
            {
                _container.Bind<MonoKernel>().FromInstance(_kernel).AsSingle().NonLazy();
            }

            InstallSceneBindings(componentInjecter);

            var extraArgsMap = new Dictionary<Type, List<TypeValuePair>>();

            if (installerExtraArgs != null)
            {
                extraArgsMap.Add(
                    installerExtraArgs.InstallerType, installerExtraArgs.ExtraArgs);
            }

            InstallInstallers(extraArgsMap);
        }

        public class InstallerExtraArgs
        {
            public Type InstallerType;
            public List<TypeValuePair> ExtraArgs;
        }
    }
}

#endif
