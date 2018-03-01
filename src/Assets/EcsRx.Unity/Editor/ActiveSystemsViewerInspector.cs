using EcsRx.Executor;
using EcsRx.Unity.MonoBehaviours;
using UnityEditor;

namespace EcsRx.Unity
{
    [CustomEditor(typeof(ActiveSystemsViewer))]
    public class ActiveSystemsViewerInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            var activeSystemsViewer = (ActiveSystemsViewer)target;
            var executor = activeSystemsViewer.SystemExecutor;

            if (executor == null)
            {
                EditorGUILayout.LabelField("System Executor Inactive");
                return;
            }

            var isNormalExecutorType = executor is SystemExecutor;
            var typedExecutor = executor as SystemExecutor;

            EditorGUILayout.TextField("Setup Systems");
            EditorGUILayout.Space();
            foreach (var system in executor.Systems)
            {
                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField("System: " + system.GetType().Name);
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
            }
        }
    }
}