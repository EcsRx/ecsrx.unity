using UnityEditor;
using UnityEngine;

namespace EcsRx.Persistence.Editor.EditorInputs.Unity
{
    public class Vector3EditorInput : SimpleEditorInput<Vector3>
    {
        public static Vector3 TypeUI(string label, Vector3 value)
        { return EditorGUILayout.Vector3Field(label, value); }
        
        protected override Vector3 CreateTypeUI(string label, Vector3 value)
        { return TypeUI(label, value); }
    }
}