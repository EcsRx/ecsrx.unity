using EcsRx.UnityEditor.MonoBehaviours;
using UnityEditor;

namespace EcsRx.UnityEditor.Editor
{
    [CustomEditor(typeof(EntityCollectionManagerViewer))]
    public class PoolManagerViewerInspector : global::UnityEditor.Editor
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