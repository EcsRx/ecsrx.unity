using EcsRx.Systems;
using EcsRx.UnityEditor.Editor.Extensions;
using EcsRx.UnityEditor.Editor.Helpers;
using EcsRx.UnityEditor.MonoBehaviours;
using UnityEditor;
using UnityEngine;

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
            
            EditorGUIHelper.WithLabel("Running Systems");
            EditorGUILayout.Space();
            foreach (var system in executor.Systems)
            {
                EditorGUIHelper.WithVerticalBoxLayout(() =>
                {
                    GUI.backgroundColor = system.GetHashCode().ToMutedColor();
                    EditorGUILayout.LabelField(system.GetType().Name);
                });
            }
        }
    }
}