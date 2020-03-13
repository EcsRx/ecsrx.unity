using UnityEditor;

namespace EcsRx.UnityEditor.Editor.EditorInputs
{
    public class BoolEditorInput : SimpleEditorInput<bool>
    {
        protected override bool CreateTypeUI(string label, bool value)
        { return EditorGUILayout.Toggle(label, value); }
    }
}