using UniRx;
using UnityEditor;

namespace EcsRx.UnityEditor.Editor.EditorInputs
{
    public class ReactiveVector2EditorInput : SimpleEditorInput<Vector2ReactiveProperty>
    {
        protected override Vector2ReactiveProperty CreateTypeUI(string label, Vector2ReactiveProperty value)
        {
            value.Value = EditorGUILayout.Vector2Field(label, value.Value);
            return null;
        }
    }
}