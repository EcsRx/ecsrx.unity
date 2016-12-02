using UnityEditor;
using UnityEngine;

namespace EcsRx.Unity.Helpers.EditorInputs
{
    public class BoundsEditorInput : SimpleEditorInput<Bounds>
    {
        protected override Bounds CreateTypeUI(string label, Bounds value)
        { return EditorGUILayout.BoundsField(label, value); }
    }
}