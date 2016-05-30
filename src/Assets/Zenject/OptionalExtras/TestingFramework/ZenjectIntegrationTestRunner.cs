#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using UnityEditor;
using UnityEngine;

namespace Zenject.TestFramework
{
    public class ZenjectIntegrationTestRunner : MonoBehaviour
    {
        public const string EditorPrefsSingleRunKey = "ZenjectIntegrationTestRunner.SingleRun";

        public string TestMethod;

        public void Awake()
        {
            Assert.That(Application.isEditor);

            bool isSingleRun = EditorPrefs.GetBool(EditorPrefsSingleRunKey, false);

            if (isSingleRun)
            {
                EditorPrefs.SetBool(EditorPrefsSingleRunKey, false);
                RunSingleTest();
            }
            else
            {
                RunAllTests();
            }
        }

        void RunAllTests()
        {
            Log.Info("Running all test fixtures underneath '{0}'", this.name);
            gameObject.AddComponent<ZenjectIntegrationTestMultiRunner>();
        }

        void RunSingleTest()
        {
            if (string.IsNullOrEmpty(TestMethod))
            {
                Log.Error("No test method given!");
                EditorApplication.isPlaying = false;
                return;
            }

            Log.Info("Running single test '{0}'", TestMethod);

            var seperatorIndex = TestMethod.IndexOf("/");
            var fixtureTypeName = TestMethod.Substring(0, seperatorIndex);
            var fixtureMethodName = TestMethod.Substring(seperatorIndex + 1);

            var installer = gameObject.AddComponent<ZenjectIntegrationTestSingleInstaller>();

            installer.Fixture = GetComponentsInChildren<ZenjectIntegrationTestFixture>()
                .Where(x => x.GetType().Name == fixtureTypeName).Single();

            installer.TestMethod = installer.Fixture.GetType()
                .GetMethods().Where(x => x.Name == fixtureMethodName).Single();

            var context = SceneContext.CreateComponent(gameObject);

            context.ParentNewObjectsUnderRoot = true;

            context.Installers = new List<MonoInstaller>()
            {
                installer
            };

            context.Run();
        }
    }
}

#endif
