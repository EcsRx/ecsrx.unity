using UnityEditor;
using UnityEngine;

namespace EcsRx.Persistence.Editor.EditorInputs.Unity
{
    public class Vector2EditorInput : SimpleEditorInput<Vector2>
    {
        public static Vector2 TypeUI(string label, Vector2 value)
        { return EditorGUILayout.Vector2Field(label, value); }
        
        protected override Vector2 CreateTypeUI(string label, Vector2 value)
        { return TypeUI(label, value); }
    }
}