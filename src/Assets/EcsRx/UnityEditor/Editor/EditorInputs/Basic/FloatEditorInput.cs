using UnityEditor;

namespace EcsRx.UnityEditor.Editor.EditorInputs.Basic
{
    public class FloatEditorInput : SimpleEditorInput<float>
    {
        public static float TypeUI(string label, float value)
        { return EditorGUILayout.FloatField(label, value); }

        protected override float CreateTypeUI(string label, float value)
        { return TypeUI(label, value); }
    }
}