using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EcsRx.Unity.Helpers
{
    [CustomEditor(typeof(PoolManagerViewer))]
    public class PoolManagerViewerInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            var poolManagerViewer = (PoolManagerViewer)target;
            var poolManager = poolManagerViewer.PoolManager;

            if (poolManager == null)
            {
                EditorGUILayout.TextArea("Pool Manager Inactive");
                return;
            }

            EditorGUILayout.TextArea("Active Pools");
            EditorGUILayout.Space();

            foreach (var pool in poolManager.Pools)
            {
                EditorGUILayout.BeginVertical();
                EditorGUILayout.TextArea("Pool: " + pool.Name);
                EditorGUILayout.TextArea("Entities: " + pool.Entities.Count());
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
            }
        }
    }
}