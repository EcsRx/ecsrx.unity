using UnityEditor;

namespace EcsRx.UnityEditor.Editor.EditorInputs.Basic
{
    public class BoolEditorInput : SimpleEditorInput<bool>
    {
        public static bool TypeUI(string label, bool value)
        { return EditorGUILayout.Toggle(label, value); }
        
        protected override bool CreateTypeUI(string label, bool value)
        { return TypeUI(label, value); }
    }
}