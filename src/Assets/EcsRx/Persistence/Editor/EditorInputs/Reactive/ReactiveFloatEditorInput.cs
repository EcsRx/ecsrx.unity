using EcsRx.Persistence.Editor.EditorInputs.Basic;

namespace EcsRx.Persistence.Editor.EditorInputs.Reactive
{
    public class ReactivePropertyFloatEditorInput : ReactivePropertyEditorInput<float>
    {
        protected override float CreateTypeUI(string label, float value)
        { return FloatEditorInput.TypeUI(label, value); }
    }
}