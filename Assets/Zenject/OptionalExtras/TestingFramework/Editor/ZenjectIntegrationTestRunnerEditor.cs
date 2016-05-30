using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ModestTree;
using UnityEditor;
using UnityEngine;
using Assert = ModestTree.Assert;

namespace Zenject.TestFramework
{
    [CustomEditor(typeof(ZenjectIntegrationTestRunner))]
    public class ZenjectIntegrationTestRunnerEditor : Editor
    {
        readonly List<string> _testMethods = new List<string>();

        public void OnEnable()
        {
            Assert.That(_testMethods.IsEmpty());

            ZenjectIntegrationTestRunner obj = target as ZenjectIntegrationTestRunner;

            foreach (var fixture in obj.GetComponentsInChildren<ZenjectIntegrationTestFixture>())
            {
                var fixtureType = fixture.GetType();

                foreach (var method in fixtureType
                    .GetMethods(
                        BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                    .Where(x => x.DeclaringType == fixtureType)
                    .Where(x => x.HasAttribute<TestAttribute>())
                    .Where(x => x.GetParameters().Length == 0))
                {
                    _testMethods.Add("{0}/{1}".Fmt(fixtureType.Name, method.Name));
                }
            }
        }

        public override void OnInspectorGUI()
        {
            ZenjectIntegrationTestRunner obj = target as ZenjectIntegrationTestRunner;

            if (obj == null)
            {
                return;
            }

            GUILayout.Space(15);

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Single Test: ", GUILayout.MaxWidth(80));

                var selectedIndex = _testMethods
                    .Select((v, i) => new { Name = v, Index = i })
                    .Where(x => x.Name == obj.TestMethod).Select(x => x.Index).Cast<int?>().FirstOrDefault();

                var newIndex = EditorGUILayout.Popup(selectedIndex.HasValue ? selectedIndex.Value : 0, _testMethods.ToArray());

                if (_testMethods.IsEmpty())
                {
                    obj.TestMethod = "";
                }
                else
                {
                    obj.TestMethod = _testMethods[newIndex];
                }
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(5);

            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Validate Single Test"))
                {
                    EditorApplication.delayCall += () =>
                    {
                        EditorPrefs.SetBool(ZenjectIntegrationTestRunner.EditorPrefsSingleRunKey, true);
                        ProjectContext.PersistentIsValidating = true;
                        EditorApplication.isPlaying = true;
                    };
                }

                if (GUILayout.Button("Run Single Test"))
                {
                    EditorApplication.delayCall += () =>
                    {
                        EditorPrefs.SetBool(ZenjectIntegrationTestRunner.EditorPrefsSingleRunKey, true);
                        EditorApplication.isPlaying = true;
                    };
                }
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(5);

            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Run All Tests"))
                {
                    EditorApplication.delayCall += () =>
                    {
                        EditorApplication.isPlaying = true;
                    };
                }
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
