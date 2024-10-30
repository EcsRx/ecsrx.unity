using EcsRx.UnityEditor.Editor.Extensions;
using EcsRx.UnityEditor.Editor.Helpers;
using EcsRx.UnityEditor.MonoBehaviours;
using UnityEditor;
using UnityEngine;

namespace EcsRx.UnityEditor.Editor
{
    [CustomEditor(typeof(EntityDatabaseViewer))]
    public class EntityDatabaseViewerInspector : global::UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var entityDatabaseViewer = (EntityDatabaseViewer)target;
            var entityDatabase = entityDatabaseViewer.EntityDatabase;

            if (entityDatabase == null)
            {
                EditorGUILayout.LabelField("Entity Database Inactive");
                return;
            }
            
            EditorGUIHelper.WithLabel("Collections");

            foreach (var entityCollection in entityDatabase.Collections)
            {
                EditorGUIHelper.WithVerticalBoxLayout(() =>
                {
                    GUI.backgroundColor = entityCollection.GetHashCode().ToMutedColor();
                    EditorGUIHelper.WithHorizontalLayout(() =>
                    {
                        EditorGUILayout.LabelField($"Collection {entityCollection.Id}");
                        EditorGUILayout.LabelField($"Entities {entityCollection.Count}");
                    });
                });
            }
        }
    }
}