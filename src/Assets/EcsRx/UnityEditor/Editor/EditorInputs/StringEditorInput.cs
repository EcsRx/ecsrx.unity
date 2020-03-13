using UnityEditor;

namespace EcsRx.UnityEditor.Editor.EditorInputs
{
    public class StringEditorInput : SimpleEditorInput<string>
    {
        protected override string CreateTypeUI(string label, string value)
        { return EditorGUILayout.TextField(label, value); }
    }
}