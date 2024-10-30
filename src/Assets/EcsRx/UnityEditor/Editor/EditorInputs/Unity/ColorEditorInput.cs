using UnityEditor;
using UnityEngine;

namespace EcsRx.UnityEditor.Editor.EditorInputs.Unity
{
    public class ColorEditorInput : SimpleEditorInput<Color>
    {
        public static Color TypeUI(string label, Color value)
        { return EditorGUILayout.ColorField(label, value); }
        
        protected override Color CreateTypeUI(string label, Color value)
        { return TypeUI(label, value); }
    }
}