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
                EditorGUILayout.LabelField("Pool Manager Inactive");
                return;
            }
            
            EditorGUILayout.TextField("Active Pools");
            EditorGUILayout.Space();

            foreach (var pool in poolManager.Pools)
            {
                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField("Pool: " + pool.Name);
                EditorGUILayout.LabelField("Entities: " + pool.Entities.Count());
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
            }
        }
    }
}