using UnityEditor;
using UnityEngine;

namespace EcsRx.UnityEditor.Editor.EditorInputs.Unity
{
    public class BoundsEditorInput : SimpleEditorInput<Bounds>
    {
        public static Bounds TypeUI(string label, Bounds value)
        { return EditorGUILayout.BoundsField(label, value); }
        
        protected override Bounds CreateTypeUI(string label, Bounds value)
        { return TypeUI(label, value); }
    }
}