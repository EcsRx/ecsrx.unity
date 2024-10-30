using EcsRx.UnityEditor.Editor.EditorInputs.Basic;

namespace EcsRx.UnityEditor.Editor.EditorInputs.Reactive
{
    public class ReactivePropertyBoolEditorInput : ReactivePropertyEditorInput<bool>
    {
        protected override bool CreateTypeUI(string label, bool value)
        { return BoolEditorInput.TypeUI(label, value); }
    }
}