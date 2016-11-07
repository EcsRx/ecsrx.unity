using UnityEditor;

namespace EcsRx.Unity.Helpers.EditorInputs
{
    public class FloatEditorInput : SimpleEditorInput<float>
    {
        protected override float CreateTypeUI(string label, float value)
        { return EditorGUILayout.FloatField(label, value); }
    }
}