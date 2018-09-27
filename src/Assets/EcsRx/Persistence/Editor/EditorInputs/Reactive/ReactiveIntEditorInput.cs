using EcsRx.Persistence.Editor.EditorInputs.Basic;

namespace EcsRx.Persistence.Editor.EditorInputs.Reactive
{
    public class ReactivePropertyIntEditorInput : ReactivePropertyEditorInput<int>
    {
        protected override int CreateTypeUI(string label, int value)
        { return IntEditorInput.TypeUI(label, value); }
    }
}