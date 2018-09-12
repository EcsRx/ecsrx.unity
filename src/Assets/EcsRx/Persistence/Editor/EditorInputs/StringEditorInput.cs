using UnityEditor;

namespace EcsRx.Persistence.Editor.EditorInputs
{
    public class StringEditorInput : SimpleEditorInput<string>
    {
        protected override string CreateTypeUI(string label, string value)
        { return EditorGUILayout.TextField(label, value); }
    }
}