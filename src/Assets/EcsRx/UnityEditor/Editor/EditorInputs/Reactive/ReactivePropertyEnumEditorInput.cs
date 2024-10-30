using System;
using EcsRx.UnityEditor.Editor.EditorInputs.Basic;

namespace EcsRx.UnityEditor.Editor.EditorInputs.Reactive
{
    public class ReactivePropertyEnumEditorInput : ReactivePropertyEditorInput<Enum>
    {       
        protected override Enum CreateTypeUI(string label, Enum value)
        { return EnumEditorInput.TypeUI(label, value); }
    }
}