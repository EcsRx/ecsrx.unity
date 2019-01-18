using EcsRx.Unity.MonoBehaviours;
using UnityEditor;

namespace EcsRx.Unity
{
    [CustomEditor(typeof(EntityCollectionManagerViewer))]
    public class PoolManagerViewerInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            var poolManagerViewer = (EntityCollectionManagerViewer)target;
            var poolManager = poolManagerViewer.CollectionManager;

            if (poolManager == null)
            {
                EditorGUILayout.LabelField("EntityCollection Manager Inactive");
                return;
            }
            
            EditorGUILayout.TextField("Active Pools");
            EditorGUILayout.Space();

            foreach (var pool in poolManager.Collections)
            {
                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField("EntityCollection: " + pool.Id);
                EditorGUILayout.LabelField("Entities: " + pool.Count);
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
            }
        }
    }
}