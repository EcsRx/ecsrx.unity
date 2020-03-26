using EcsRx.UnityEditor.MonoBehaviours;
using UnityEditor;

namespace EcsRx.UnityEditor.Editor
{
    [CustomEditor(typeof(ActiveSystemsViewer))]
    public class ActiveSystemsViewerInspector : global::UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var activeSystemsViewer = (ActiveSystemsViewer)target;
            if(activeSystemsViewer == null) {  return; }
            var executor = activeSystemsViewer.SystemExecutor;

            if (executor == null)
            {
                EditorGUILayout.LabelField("System Executor Inactive");
                return;
            }
            
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