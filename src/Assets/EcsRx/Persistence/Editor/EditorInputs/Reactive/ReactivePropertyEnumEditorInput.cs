using System;
using EcsRx.Persistence.Editor.EditorInputs.Basic;

namespace EcsRx.Persistence.Editor.EditorInputs.Reactive
{
    public class ReactivePropertyEnumEditorInput : ReactivePropertyEditorInput<Enum>
    {       
        protected override Enum CreateTypeUI(string label, Enum value)
        { return EnumEditorInput.TypeUI(label, value); }
    }
}