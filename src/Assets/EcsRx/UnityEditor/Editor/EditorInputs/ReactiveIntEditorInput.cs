using UniRx;
using UnityEditor;

namespace EcsRx.UnityEditor.Editor.EditorInputs
{
    public class ReactiveIntEditorInput : SimpleEditorInput<IntReactiveProperty>
    {
        protected override IntReactiveProperty CreateTypeUI(string label, IntReactiveProperty value)
        {
            value.Value = EditorGUILayout.IntField(label, value.Value);
            return null;
        }
    }
}