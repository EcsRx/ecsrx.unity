using UnityEditor;

namespace EcsRx.Unity.Helpers.EditorInputs
{
    public class BoolEditorInput : SimpleEditorInput<bool>
    {
        protected override bool CreateTypeUI(string label, bool value)
        { return EditorGUILayout.Toggle(label, value); }
    }
}