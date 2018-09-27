using UnityEditor;
using UnityEngine;

namespace EcsRx.Persistence.Editor.EditorInputs.Unity
{
    public class Vector4EditorInput : SimpleEditorInput<Vector4>
    {
        public static Vector4 TypeUI(string label, Vector4 value)
        { return EditorGUILayout.Vector3Field(label, value); }
        
        protected override Vector4 CreateTypeUI(string label, Vector4 value)
        { return TypeUI(label, value); }
    }
}