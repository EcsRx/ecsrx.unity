using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
#if UNITY_5_6_OR_NEWER
using NUnit.Framework.Interfaces;
#endif
using UnityEditor.SceneManagement;
using UnityEngine;
using ModestTree;
using Assert = ModestTree.Assert;

namespace Zenject
{
    public abstract class ZenjectIntegrationTestFixture
    {
        SceneContext _sceneContext;

        bool _hasStarted;
        bool _isValidating;

        protected DiContainer Container
        {
            get { return _sceneContext.Container; }
        }

        protected SceneContext SceneContext
        {
            get { return _sceneContext; }
        }

        [SetUp]
        public virtual void SetUp()
        {
            // Don't clear the scene to allow tests to initialize the scene how they
            // want to set it up manually in their own [Setup] method (eg. TestFromComponentInChildren)
            // Also, I think unity might already clear the scene anyway?
            //ClearScene();

            _hasStarted = false;
            _isValidating = CurrentTestHasAttribute<ValidateOnlyAttribute>();

            ProjectContext.ValidateOnNextRun = _isValidating;

            _sceneContext = new GameObject("SceneContext").AddComponent<SceneContext>();
            _sceneContext.ParentNewObjectsUnderRoot = true;
            // This creates the container but does not resolve the roots yet
            _sceneContext.Install();
        }

        public void Initialize()
        {
            Assert.That(!_hasStarted);
            _hasStarted = true;

            _sceneContext.Resolve();

            // This allows them to make very common bindings fields for use in any of the tests
            Container.Inject(this);

            if (_isValidating)
            {
                Container.ValidateValidatables();
            }
            else
            {
                _sceneContext.gameObject.GetComponent<SceneKernel>().Start();
            }
        }

        [TearDown]
        public void TearDown()
        {
#if UNITY_5_6_OR_NEWER
            if (TestContext.CurrentContext.Result.Outcome == ResultState.Success)
            {
                Assert.That(_hasStarted, "ZenjectIntegrationTestFixture.Initialize was not called by current test");
            }
#else
            if (TestContext.CurrentContext.Result.Status == TestStatus.Passed)
            {
                // If we expected an exception then initialize would normally not be called
                // Unless the initialize method itself is what caused the exception
                if (!CurrentTestHasAttribute<ExpectedExceptionAttribute>())
                {
                    Assert.That(_hasStarted, "ZenjectIntegrationTestFixture.Initialize was not called by current test");
                }
            }
#endif

            ClearScene();
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

        protected void ClearScene()
        {
            var scene = EditorSceneManager.GetActiveScene();

            // This is the temp scene that unity creates for EditorTestRunner
            Assert.IsEqual(scene.name, "");

            // This will include ProjectContext which is what we want, to ensure no test
            // affects any other test
            foreach (var gameObject in scene.GetRootGameObjects())
            {
                GameObject.DestroyImmediate(gameObject);
            }
        }
    }
}

