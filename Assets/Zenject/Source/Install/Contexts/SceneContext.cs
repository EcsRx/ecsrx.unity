#if !NOT_UNITY3D

using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using ModestTree.Util;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject.Internal;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Zenject
{
    public class SceneContext : Context
    {
        public static readonly List<Scene> DecoratedScenes = new List<Scene>();

        public static Action<DiContainer> BeforeInstallHooks;
        public static Action<DiContainer> AfterInstallHooks;

        public static DiContainer ParentContainer;

        [FormerlySerializedAs("ParentNewObjectsUnderRoot")]
        [Tooltip("When true, objects that are created at runtime will be parented to the SceneContext")]
        [SerializeField]
        bool _parentNewObjectsUnderRoot = false;

        DiContainer _container;
        readonly List<object> _dependencyRoots = new List<object>();

        bool _hasInitialized;

#if UNITY_EDITOR
        bool _validateShutDownAfterwards = true;
#endif

        static bool _autoRun = true;

        public override DiContainer Container
        {
            get
            {
                return _container;
            }
        }

#if UNITY_EDITOR
        public bool IsValidating
        {
            get;
            set;
        }

        public bool ValidateShutDownAfterwards
        {
            get
            {
                return _validateShutDownAfterwards;
            }
            set
            {
                _validateShutDownAfterwards = value;
            }
        }
#else
        public bool IsValidating
        {
            get
            {
                return false;
            }
        }
#endif

        public bool ParentNewObjectsUnderRoot
        {
            get
            {
                return _parentNewObjectsUnderRoot;
            }
            set
            {
                _parentNewObjectsUnderRoot = value;
            }
        }

        public void Awake()
        {
            // We always want to initialize ProjectContext as early as possible
            ProjectContext.Instance.EnsureIsInitialized();

#if UNITY_EDITOR
            IsValidating = ProjectContext.Instance.Container.IsValidating;
#endif

            if (_autoRun)
            {
                Run();
            }
            else
            {
                // True should always be default
                _autoRun = true;
            }
        }

#if UNITY_EDITOR
        public void Run()
        {
            if (IsValidating)
            {
                try
                {
                    RunInternal();

                    Assert.That(_container.IsValidating);

                    _container.ValidateIValidatables();

                    Log.Info("Scene '{0}' Validated Successfully", this.gameObject.scene.name);
                }
                catch (Exception e)
                {
                    Log.ErrorException("Scene '{0}' Failed Validation!".Fmt(this.gameObject.scene.name), e);
                }
            }
            else
            {
                RunInternal();
            }
        }
#else
        public void Run()
        {
            RunInternal();
        }
#endif

        public void RunInternal()
        {
            Assert.That(!_hasInitialized);
            _hasInitialized = true;

            Assert.IsNull(_container);

            var parentContainer = ParentContainer ?? ProjectContext.Instance.Container;

            // ParentContainer is optionally set temporarily before calling ZenUtil.LoadScene
            ParentContainer = null;

            _container = parentContainer.CreateSubContainer(IsValidating);

#if !UNITY_EDITOR
            Assert.That(!IsValidating);
#endif

            // This can happen if you run a decorated scene with immediately running a normal scene afterwards
            foreach (var decoratedScene in DecoratedScenes)
            {
                Assert.That(decoratedScene.isLoaded,
                    "Unexpected state in SceneContext - found unloaded decorated scene");
            }

            // Record all the injectable components in the scene BEFORE installing the installers
            // This is nice for cases where the user calls InstantiatePrefab<>, etc. in their installer
            // so that it doesn't inject on the game object twice
            // InitialComponentsInjecter will also guarantee that any component that is injected into
            // another component has itself been injected
            var componentInjecter = new InitialComponentsInjecter(
                _container, GetInjectableComponents().ToList());

            Log.Debug("SceneContext: Running installers...");

            _container.IsInstalling = true;

            try
            {
                InstallBindings(componentInjecter);
            }
            finally
            {
                _container.IsInstalling = false;
            }

            Log.Debug("SceneContext: Injecting components in the scene...");

            componentInjecter.LazyInjectComponents();

            Log.Debug("SceneContext: Resolving dependency roots...");

            Assert.That(_dependencyRoots.IsEmpty());
            _dependencyRoots.AddRange(_container.ResolveDependencyRoots());

            DecoratedScenes.Clear();

            Log.Debug("SceneContext: Initialized successfully");
        }

        void InstallBindings(InitialComponentsInjecter componentInjecter)
        {
            if (_parentNewObjectsUnderRoot)
            {
                _container.DefaultParent = this.transform;
            }
            else
            {
                // This is necessary otherwise we inherit the project root DefaultParent
                _container.DefaultParent = null;
            }

            _container.Bind<Context>().FromInstance(this);
            _container.Bind<SceneContext>().FromInstance(this);

            InstallSceneBindings(componentInjecter);

            if (BeforeInstallHooks != null)
            {
                BeforeInstallHooks(_container);
                // Reset extra bindings for next time we change scenes
                BeforeInstallHooks = null;
            }

            _container.Bind<SceneKernel>().FromComponent(this.gameObject).AsSingle().NonLazy();

            _container.Bind<ZenjectSceneLoader>().AsSingle();

            InstallInstallers();

            if (AfterInstallHooks != null)
            {
                AfterInstallHooks(_container);
                // Reset extra bindings for next time we change scenes
                AfterInstallHooks = null;
            }
        }

        protected override IEnumerable<Component> GetInjectableComponents()
        {
            foreach (var gameObject in GetRootGameObjects())
            {
                foreach (var component in GetInjectableComponents(gameObject))
                {
                    yield return component;
                }
            }

            yield break;
        }

        public IEnumerable<GameObject> GetRootGameObjects()
        {
            var scene = this.gameObject.scene;

            // Note: We can't use activeScene.GetRootObjects() here because that apparently fails with an exception
            // about the scene not being loaded yet when executed in Awake
            // We also can't use GameObject.FindObjectsOfType<Transform>() because that does not include inactive game objects
            // So we use Resources.FindObjectsOfTypeAll, even though that may include prefabs.  However, our assumption here
            // is that prefabs do not have their "scene" property set correctly so this should work
            //
            // It's important here that we only inject into root objects that are part of our scene, to properly support
            // multi-scene editing features of Unity 5.x
            //
            // Also, even with older Unity versions, if there is an object that is marked with DontDestroyOnLoad, then it will
            // be injected multiple times when another scene is loaded
            //
            // We also make sure not to inject into the project root objects which are injected by ProjectContext.
            return Resources.FindObjectsOfTypeAll<GameObject>()
                .Where(x => x.transform.parent == null
                    && x.GetComponent<ProjectContext>() == null
                    && (x.scene == scene || DecoratedScenes.Contains(x.scene)));
        }

        // These methods can be used for cases where you need to create the SceneContext entirely in code
        // Note that if you use these methods that you have to call Run() yourself
        // This is useful because it allows you to create a SceneContext and configure it how you want
        // and add what installers you want before kicking off the Install/Resolve
        public static SceneContext Create()
        {
            return CreateComponent(
                new GameObject("SceneContext"));
        }

        public static SceneContext CreateComponent(GameObject gameObject)
        {
            _autoRun = false;
            var result = gameObject.AddComponent<SceneContext>();
            Assert.That(_autoRun); // Should be reset
            return result;
        }
    }
}

#endif
