using UnityEditor;
using UnityEngine;

namespace EcsRx.UnityEditor.Editor.EditorInputs
{
    public class RectEditorInput : SimpleEditorInput<Rect>
    {
        protected override Rect CreateTypeUI(string label, Rect value)
        { return EditorGUILayout.RectField(label, value); }
    }
}