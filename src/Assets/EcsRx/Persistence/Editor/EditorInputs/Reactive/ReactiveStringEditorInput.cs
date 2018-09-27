using EcsRx.Persistence.Editor.EditorInputs.Basic;

namespace EcsRx.Persistence.Editor.EditorInputs.Reactive
{
    public class ReactivePropertyStringEditorInput : ReactivePropertyEditorInput<string>
    {
        protected override string CreateTypeUI(string label, string value)
        { return StringEditorInput.TypeUI(label, value); }
    }
}