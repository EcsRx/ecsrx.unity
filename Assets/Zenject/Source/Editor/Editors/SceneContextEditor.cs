using System.Collections.Generic;
using System.Linq;
using Zenject;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;
using ModestTree;

namespace Zenject
{
    [CustomEditor(typeof(SceneContext))]
    public class SceneContextEditor : ContextEditor
    {
        SerializedProperty _parentNewObjectsUnderRootProperty;

        public override void OnEnable()
        {
            base.OnEnable();

            _parentNewObjectsUnderRootProperty = serializedObject.FindProperty("_parentNewObjectsUnderRoot");
        }

        protected override void OnGui()
        {
            base.OnGui();

            EditorGUILayout.PropertyField(_parentNewObjectsUnderRootProperty);
        }
    }
}



