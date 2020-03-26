using UnityEditor;
using UnityEngine;

namespace EcsRx.UnityEditor.Editor.EditorInputs
{
    public class ColorEditorInput : SimpleEditorInput<Color>
    {
        protected override Color CreateTypeUI(string label, Color value)
        { return EditorGUILayout.ColorField(label, value); }
    }
}