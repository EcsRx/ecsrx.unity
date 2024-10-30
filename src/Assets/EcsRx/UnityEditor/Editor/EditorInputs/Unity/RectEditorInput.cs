using UnityEditor;
using UnityEngine;

namespace EcsRx.UnityEditor.Editor.EditorInputs.Unity
{
    public class RectEditorInput : SimpleEditorInput<Rect>
    {
        public static Rect TypeUI(string label, Rect value)
        { return EditorGUILayout.RectField(label, value); }
        
        protected override Rect CreateTypeUI(string label, Rect value)
        { return TypeUI(label, value); }
    }
}