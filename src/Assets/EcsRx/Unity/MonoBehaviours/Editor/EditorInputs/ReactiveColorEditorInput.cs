using UniRx;
using UnityEditor;

namespace EcsRx.Unity.Helpers.EditorInputs
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