#if !ODIN_INSPECTOR

using UnityEditor;

namespace Zenject
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(SceneContext))]
    public class SceneContextEditor : RunnableContextEditor
    {
        SerializedProperty _contractNameProperty;
        SerializedProperty _parentNamesProperty;
        SerializedProperty _parentNewObjectsUnderRootProperty;

        public override void OnEnable()
        {
            base.OnEnable();

            _contractNameProperty = serializedObject.FindProperty("_contractNames");
            _parentNamesProperty = serializedObject.FindProperty("_parentContractNames");
            _parentNewObjectsUnderRootProperty = serializedObject.FindProperty("_parentNewObjectsUnderRoot");
        }

        protected override void OnGui()
        {
            base.OnGui();

            EditorGUILayout.PropertyField(_contractNameProperty, true);
            EditorGUILayout.PropertyField(_parentNamesProperty, true);
            EditorGUILayout.PropertyField(_parentNewObjectsUnderRootProperty);
        }
    }
}


#endif
