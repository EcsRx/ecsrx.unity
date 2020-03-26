using UniRx;
using UnityEditor;

namespace EcsRx.UnityEditor.Editor.EditorInputs
{
    public class ReactiveBoundsEditorInput : SimpleEditorInput<BoundsReactiveProperty>
    {
        protected override BoundsReactiveProperty CreateTypeUI(string label, BoundsReactiveProperty value)
        {
            value.Value = EditorGUILayout.BoundsField(label, value.Value);
            return null;
        }
    }
}