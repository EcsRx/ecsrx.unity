using UnityEditor;

namespace EcsRx.Persistence.Editor.EditorInputs
{
    public class IntEditorInput : SimpleEditorInput<int>
    {
        protected override int CreateTypeUI(string label, int value)
        { return EditorGUILayout.IntField(label, value); }
    }
}