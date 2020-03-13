using UniRx;
using UnityEditor;

namespace EcsRx.UnityEditor.Editor.EditorInputs
{
    public class ReactiveColorEditorInput : SimpleEditorInput<ColorReactiveProperty>
    {
        protected override ColorReactiveProperty CreateTypeUI(string label, ColorReactiveProperty value)
        {
            value.Value = EditorGUILayout.ColorField(label, value.Value);
            return null;
        }
    }
}