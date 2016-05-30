#if UNITY_EDITOR

using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using ModestTree;
using UnityEditor;
using UnityEngine;

namespace Zenject.TestFramework
{
    public class ZenjectIntegrationTestMultiRunner : MonoBehaviour
    {
        public const string EditorPrefsKeyIgnoreError = "Zenject.TestFramework.IgnoreErrors";

        const float DefaultWaitTime = 0.5f;

        bool _hasFailed;
        bool _hitAnyError;

        public void Start()
        {
            EditorPrefs.SetBool(EditorPrefsKeyIgnoreError, false);

            ListenOnAllErrors();
            StartCoroutine(RunTests());
        }

        void ListenOnAllErrors()
        {
            Application.logMessageReceived += OnLogMessageReceived;
        }

        void OnLogMessageReceived(string condition, string stacktrace, LogType level)
        {
            if (level == LogType.Exception || level == LogType.Assert || level == LogType.Error)
            {
                _hitAnyError = true;
            }
        }

        IEnumerator RunTests()
        {
            foreach (var fixture in gameObject.GetComponentsInChildren<ZenjectIntegrationTestFixture>())
            {
                var testMethods = fixture.GetType().GetAllInstanceMethods()
                    .Where(x => x.HasAttribute<TestAttribute>());

                foreach (var methodInfo in testMethods)
                {
                    yield return StartCoroutine(RunTest(fixture, methodInfo, true));

                    if (_hasFailed)
                    {
                        break;
                    }

                    yield return StartCoroutine(RunTest(fixture, methodInfo, false));

                    if (_hasFailed)
                    {
                        break;
                    }
                }

                if (_hasFailed)
                {
                    break;
                }
            }

            if (_hasFailed)
            {
                Log.Error("Integration tests failed with errors");
            }
            else
            {
                Log.Info("Integration tests passed successfully");
            }

            EditorApplication.isPlaying = false;
        }

        IEnumerator RunTest(ZenjectIntegrationTestFixture fixture, MethodInfo methodInfo, bool validateOnly)
        {
            // This should be reset each time
            Assert.That(!EditorPrefs.GetBool(EditorPrefsKeyIgnoreError, false));

            bool isExpectingErrorDuringTest;

            if (validateOnly)
            {
                isExpectingErrorDuringTest = methodInfo.HasAttribute<ExpectedValidationExceptionAttribute>();
            }
            else
            {
                isExpectingErrorDuringTest = methodInfo.HasAttribute<ExpectedExceptionAttribute>();
            }

            var waitTimeAttribute = methodInfo.AllAttributes<WaitTimeSecondsAttribute>().OnlyOrDefault();
            var waitSeconds = waitTimeAttribute == null ? DefaultWaitTime : waitTimeAttribute.Seconds;

            var testName = "{0}.{1}".Fmt(fixture.GetType().Name(), methodInfo.Name);

            if (validateOnly)
            {
                Log.Info("Validating test '{0}'{1}", testName, isExpectingErrorDuringTest ? " (expecting error)" : "");
            }
            else
            {
                Log.Info("Running test '{0}' and waiting {1} seconds. {2}", testName, waitSeconds, isExpectingErrorDuringTest ? " (expecting error)" : "");
            }

            var context = SceneContext.Create();

            // Put under ourself otherwise it disables us during validation
            context.transform.parent = this.transform;

            context.IsValidating = validateOnly;
            context.ValidateShutDownAfterwards = false;

            context.NormalInstallers = new Installer[]
            {
                new ActionInstaller((container) =>
                    {
                        fixture.Container = container;
                        methodInfo.Invoke(fixture, new object[0]);
                        container.FlushBindings();
                    })
            };

            context.ParentNewObjectsUnderRoot = true;

            _hitAnyError = false;
            EditorPrefs.SetBool(EditorPrefsKeyIgnoreError, isExpectingErrorDuringTest);

            try
            {
                context.Run();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            if (!validateOnly)
            {
                yield return new WaitForSeconds(waitSeconds);
            }

            EditorPrefs.SetBool(EditorPrefsKeyIgnoreError, false);
            bool hitErrorDuringTest = _hitAnyError;

            GameObject.Destroy(context.gameObject);

            yield return null;

            if (isExpectingErrorDuringTest)
            {
                if (hitErrorDuringTest)
                {
                    Log.Info("Hit expected error during test '{0}'. Ignoring.", testName);
                }
                else
                {
                    Log.Error("Expected to hit error during test '{0}' but none was found!", testName);
                    _hasFailed = true;
                }
            }
            else
            {
                if (hitErrorDuringTest)
                {
                    _hasFailed = true;
                }
            }
        }

        class ActionInstaller : Installer
        {
            readonly Action<DiContainer> _installMethod;

            public ActionInstaller(Action<DiContainer> installMethod)
            {
                _installMethod = installMethod;
            }

            public override void InstallBindings()
            {
                _installMethod(Container);
            }
        }
    }
}

#endif
