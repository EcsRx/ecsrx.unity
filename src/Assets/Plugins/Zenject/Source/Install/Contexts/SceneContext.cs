#if !NOT_UNITY3D

using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using ModestTree.Util;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;
using Zenject.Internal;

namespace Zenject
{
    public class SceneContext : Context
    {
        public static Action<DiContainer> ExtraBindingsInstallMethod;

        public static DiContainer ParentContainer;

        [FormerlySerializedAs("ParentNewObjectsUnderRoot")]
        [Tooltip("When true, objects that are created at runtime will be parented to the SceneContext")]
        [SerializeField]
        bool _parentNewObjectsUnderRoot = false;

        [Tooltip("Optional contract names for this SceneContext, allowing contexts in subsequently loaded scenes to depend on it and be parented to it, and also for previously loaded decorators to be included")]
        [SerializeField]
        List<string> _contractNames = new List<string>();

        [Tooltip("Optional contract name of a SceneContext in a previously loaded scene that this context depends on and to which it must be parented")]
        [SerializeField]
        string _parentContractName;

        [Tooltip("When false, wait until run method is explicitly called. Otherwise run on awake")]
        [SerializeField]
        bool _autoRun = true;

        DiContainer _container;
        readonly List<object> _dependencyRoots = new List<object>();

        readonly List<SceneDecoratorContext> _decoratorContexts = new List<SceneDecoratorContext>();

        bool _hasInstalled;
        bool _hasResolved;

        static bool _staticAutoRun = true;

        public override DiContainer Container
        {
            get { return _container; }
        }

        public bool IsValidating
        {
            get
            {
#if UNITY_EDITOR
                return ProjectContext.Instance.Container.IsValidating;
#else
                return false;
#endif
            }
        }

        public IEnumerable<string> ContractNames
        {
            get { return _contractNames; }
            set
            {
                _contractNames.Clear();
                _contractNames.AddRange(value);
            }
        }

        public string ParentContractName
        {
            get { return _parentContractName; }
            set
            {
                _parentContractName = value;
            }
        }

        public bool ParentNewObjectsUnderRoot
        {
            get { return _parentNewObjectsUnderRoot; }
            set { _parentNewObjectsUnderRoot = value; }
        }

        public void Awake()
        {
            // We always want to initialize ProjectContext as early as possible
            ProjectContext.Instance.EnsureIsInitialized();

            if (_staticAutoRun && _autoRun)
            {
                Run();
            }
            else
            {
                // True should always be default
                _staticAutoRun = true;
            }
        }

#if UNITY_EDITOR
        public void Validate()
        {
            Assert.That(IsValidating);

            Install();
            Resolve();

            _container.ValidateValidatables();
        }
#endif

        public void Run()
        {
            Assert.That(!IsValidating);
            Install();
            Resolve();
        }

        public override IEnumerable<GameObject> GetRootGameObjects()
        {
            return ZenUtilInternal.GetRootGameObjects(gameObject.scene);
        }

        DiContainer GetParentContainer()
        {
            if (string.IsNullOrEmpty(_parentContractName))
            {
                if (ParentContainer != null)
                {
                    var tempParentContainer = ParentContainer;

                    // Always reset after using it - it is only used to pass the reference
                    // between scenes via ZenjectSceneLoader
                    ParentContainer = null;

                    return tempParentContainer;
                }

                return ProjectContext.Instance.Container;
            }

            Assert.IsNull(ParentContainer,
                "Scene cannot have both a parent scene context name set and also an explicit parent container given");

            var sceneContexts = UnityUtil.AllLoadedScenes
                .Except(gameObject.scene)
                .SelectMany(scene => scene.GetRootGameObjects())
                .SelectMany(root => root.GetComponentsInChildren<SceneContext>())
                .Where(sceneContext => sceneContext.ContractNames.Contains(_parentContractName))
                .ToList();

            Assert.That(sceneContexts.Any(), () => string.Format(
                "SceneContext on object {0} of scene {1} requires contract {2}, but none of the loaded SceneContexts implements that contract.",
                gameObject.name,
                gameObject.scene.name,
                _parentContractName));

            Assert.That(sceneContexts.Count == 1, () => string.Format(
                "SceneContext on object {0} of scene {1} requires a single implementation of contract {2}, but multiple were found.",
                gameObject.name,
                gameObject.scene.name,
                _parentContractName));

            return sceneContexts.Single().Container;
        }

        List<SceneDecoratorContext> LookupDecoratorContexts()
        {
            if (_contractNames.IsEmpty())
            {
                return new List<SceneDecoratorContext>();
            }

            return UnityUtil.AllLoadedScenes
                .Except(gameObject.scene)
                .SelectMany(scene => scene.GetRootGameObjects())
                .SelectMany(root => root.GetComponentsInChildren<SceneDecoratorContext>())
                .Where(decoratorContext => _contractNames.Contains(decoratorContext.DecoratedContractName))
                .ToList();
        }

        public void Install()
        {
#if !UNITY_EDITOR
            Assert.That(!IsValidating);
#endif

            Assert.That(!_hasInstalled);
            _hasInstalled = true;

            Assert.IsNull(_container);
            _container = GetParentContainer().CreateSubContainer();

            Assert.That(_decoratorContexts.IsEmpty());
            _decoratorContexts.AddRange(LookupDecoratorContexts());

            Log.Debug("SceneContext: Running installers...");

            if (_parentNewObjectsUnderRoot)
            {
                _container.DefaultParent = this.transform;
            }
            else
            {
                // This is necessary otherwise we inherit the project root DefaultParent
                _container.DefaultParent = null;
            }

            // Record all the injectable components in the scene BEFORE installing the installers
            // This is nice for cases where the user calls InstantiatePrefab<>, etc. in their installer
            // so that it doesn't inject on the game object twice
            // InitialComponentsInjecter will also guarantee that any component that is injected into
            // another component has itself been injected
            foreach (var instance in GetInjectableMonoBehaviours().Cast<object>())
            {
                _container.QueueForInject(instance);
            }

            foreach (var decoratorContext in _decoratorContexts)
            {
                decoratorContext.Initialize(_container);
            }

            _container.IsInstalling = true;

            try
            {
                InstallBindings();
            }
            finally
            {
                _container.IsInstalling = false;
            }
        }

        public void Resolve()
        {
            Log.Debug("SceneContext: Injecting components in the scene...");

            Assert.That(_hasInstalled);
            Assert.That(!_hasResolved);
            _hasResolved = true;

            Log.Debug("SceneContext: Resolving all...");

            Assert.That(_dependencyRoots.IsEmpty());
            _dependencyRoots.AddRange(_container.ResolveDependencyRoots());

            _container.FlushInjectQueue();

            Log.Debug("SceneContext: Initialized successfully");
        }

        void InstallBindings()
        {
            _container.Bind(typeof(Context), typeof(SceneContext)).To<SceneContext>().FromInstance(this);

            foreach (var decoratorContext in _decoratorContexts)
            {
                decoratorContext.InstallDecoratorSceneBindings();
            }

            InstallSceneBindings();

            _container.Bind<SceneKernel>().FromNewComponentOn(this.gameObject).AsSingle().NonLazy();

            _container.Bind<ZenjectSceneLoader>().AsSingle();

            if (ExtraBindingsInstallMethod != null)
            {
                ExtraBindingsInstallMethod(_container);
                // Reset extra bindings for next time we change scenes
                ExtraBindingsInstallMethod = null;
            }

            // Always install the installers last so they can be injected with
            // everything above
            foreach (var decoratorContext in _decoratorContexts)
            {
                decoratorContext.InstallDecoratorInstallers();
            }

            InstallInstallers();

            foreach (var decoratorContext in _decoratorContexts)
            {
                decoratorContext.InstallLateDecoratorInstallers();
            }
        }

        protected override IEnumerable<MonoBehaviour> GetInjectableMonoBehaviours()
        {
            return ZenUtilInternal.GetInjectableMonoBehaviours(this.gameObject.scene);
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
            _staticAutoRun = false;
            var result = gameObject.AddComponent<SceneContext>();
            Assert.That(_staticAutoRun); // Should be reset
            return result;
        }
    }
}

#endif
