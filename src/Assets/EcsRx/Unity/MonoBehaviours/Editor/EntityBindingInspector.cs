using System.Linq;
using EcsRx.Unity.MonoBehaviours;
using UnityEditor;
using UnityEngine;

namespace EcsRx.Unity.Helpers
{
    [CustomEditor(typeof(EntityBindingInspector))]
    public class EntityBindingInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            var entityBinding = (EntityBinding)target;
            var entity = entityBinding.Entity;

            if (entity == null)
            {
                EditorGUILayout.LabelField("No Entity Assigned");
                return;
            }

            var poolName = entityBinding.PoolName;

            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Entity Id");
            EditorGUILayout.LabelField(entity.Id.ToString());
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Pool Name");
            EditorGUILayout.LabelField(string.IsNullOrEmpty(poolName) ? "Default" : poolName);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();

            foreach (var component in entity.Components)
            {
                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField(component.GetType().Name);
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
            }
        }
    }
}