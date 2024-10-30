using EcsRx.UnityEditor.Editor.EditorInputs.Unity;
using UnityEngine;

namespace EcsRx.UnityEditor.Editor.EditorInputs.Reactive
{
    public class ReactivePropertyColorEditorInput : ReactivePropertyEditorInput<Color>
    {
        protected override Color CreateTypeUI(string label, Color value)
        { return ColorEditorInput.TypeUI(label, value); }
    }
}