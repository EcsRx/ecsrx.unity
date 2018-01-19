using System;

#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;
using ModestTree;
#endif

using Assert = ModestTree.Assert;

namespace Zenject
{
#if UNITY_EDITOR
    public abstract class ZenjectIntegrationTestFixture
    {
        SceneContext _sceneContext;

        List<GameObject> _unityTestRunnerObjects;

        bool _hasEndedInstall;
        bool _hasStartedInstall;
        bool _hasDestroyedAll;

        protected DiContainer Container
        {
            get
            {
                Assert.That(_hasStartedInstall,
                    "Must call PreInstall() before accessing ZenjectIntegrationTestFixture.Container!");
                return _sceneContext.Container;
            }
        }

        protected SceneContext SceneContext
        {
            get
            {
                Assert.That(_hasStartedInstall,
                    "Must call PreInstall() before accessing ZenjectIntegrationTestFixture.SceneContext!");
                return _sceneContext;
            }
        }

        [SetUp]
        public void Setup()
        {
            // Only need to record this once for each set of tests
            if (_unityTestRunnerObjects == null)
            {
                _unityTestRunnerObjects = SceneManager.GetActiveScene()
                    .GetRootGameObjects().ToList();
            }
        }

        protected void SkipInstall()
        {
            PreInstall();
            PostInstall();
        }

        protected void PreInstall()
        {
            Assert.That(!_hasStartedInstall, "Called PreInstall twice in test '{0}'!", TestContext.CurrentContext.Test.Name);
            _hasStartedInstall = true;

            Assert.That(!ProjectContext.HasInstance);

            var shouldValidate = CurrentTestHasAttribute<ValidateOnlyAttribute>();

            ProjectContext.ValidateOnNextRun = shouldValidate;

            Assert.IsNull(_sceneContext);

            _sceneContext = SceneContext.Create();
            _sceneContext.Install();

            Assert.That(ProjectContext.HasInstance);
            Assert.IsEqual(shouldValidate, ProjectContext.Instance.Container.IsValidating);
            Assert.IsEqual(shouldValidate, _sceneContext.Container.IsValidating);
        }

        bool CurrentTestHasAttribute<T>()
            where T : Attribute
        {
            // tests with double parameters need to have their () removed first
            var name = TestContext.CurrentContext.Test.FullName;

            // Remove all characters after the first open bracket if there is one
            int openBracketIndex = name.IndexOf("(");

            if (openBracketIndex != -1)
            {
                name = name.Substring(0, openBracketIndex);
            }

            // Now we can get the substring starting at the last '.'
            name = name.Substring(name.LastIndexOf(".") + 1);

            return this.GetType().GetMethod(name).GetCustomAttributes(true)
                .Cast<Attribute>().OfType<T>().Any();
        }

        protected void PostInstall()
        {
            Assert.That(_hasStartedInstall,
                "Called PostInstall but did not call PreInstall in test '{0}'!", TestContext.CurrentContext.Test.Name);

            Assert.That(!_hasEndedInstall, "Called PostInstall twice in test '{0}'!", TestContext.CurrentContext.Test.Name);

            _hasEndedInstall = true;
            _sceneContext.Resolve();

            Container.Inject(this);

            if (Container.IsValidating)
            {
                Container.ValidateValidatables();
            }
            else
            {
                // This is necessary because otherwise MonoKernel is not started until later
                // and therefore IInitializable objects are not initialized
                Container.Resolve<MonoKernel>().Initialize();
            }
        }

        protected void DestroyAll()
        {
            Assert.That(!_hasDestroyedAll, "Called DestroyAll twice in test '{0}'!", TestContext.CurrentContext.Test.Name);
            _hasDestroyedAll = true;

            Assert.That(_hasStartedInstall,
                "Called DestroyAll but did not call PreInstall (or SkipInstall) in test '{0}'!", TestContext.CurrentContext.Test.Name);

            // We need to use DestroyImmediate so that all the IDisposable's etc get processed immediately before
            // next test runs
            GameObject.DestroyImmediate(_sceneContext.gameObject);
            _sceneContext = null;

            var allRootObjects = new List<GameObject>();

            // We want to clear all objects across all scenes to ensure the next test is not affected
            // at all by previous tests
            // TODO: How does this handle cases where the test loads other scenes?
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                allRootObjects.AddRange(
                    SceneManager.GetSceneAt(i).GetRootGameObjects());
            }

            // We also want to destroy any objects marked DontDestroyOnLoad, especially ProjectContext
            allRootObjects.AddRange(ProjectContext.Instance.gameObject.scene.GetRootGameObjects());

            foreach (var rootObj in allRootObjects)
            {
                // Make sure not to destroy the unity test runner objects that it adds
                if (!_unityTestRunnerObjects.Contains(rootObj))
                {
                    // Use DestroyImmediate for other objects too just to ensure we are fully
                    // cleaned up before the next test starts
                    GameObject.DestroyImmediate(rootObj);
                }
            }
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome == ResultState.Success)
            {
                Assert.That(_hasStartedInstall,
                    "PreInstall (or SkipInstall) was not called in test '{0}'!", TestContext.CurrentContext.Test.Name);

                Assert.That(_hasEndedInstall,
                    "PostInstall was not called in test '{0}'!", TestContext.CurrentContext.Test.Name);
            }

            if (!_hasDestroyedAll)
            {
                DestroyAll();
            }

            _hasStartedInstall = false;
            _hasEndedInstall = false;
            _hasDestroyedAll = false;
        }
    }
#else
    public abstract class ZenjectIntegrationTestFixture
    {
        protected DiContainer Container
        {
            get
            {
                throw CreateException();
            }
        }

        Exception CreateException()
        {
            return Assert.CreateException(
                "ZenjectIntegrationTestFixture currently only supports running within unity editor");
        }

        protected void SkipInstall()
        {
            throw CreateException();
        }

        protected void PreInstall()
        {
            throw CreateException();
        }

        protected void PostInstall()
        {
            throw CreateException();
        }

        protected void DestroyAll()
        {
            throw CreateException();
        }
    }
#endif
}
