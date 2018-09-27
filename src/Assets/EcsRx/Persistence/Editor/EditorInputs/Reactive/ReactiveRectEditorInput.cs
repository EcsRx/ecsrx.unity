using EcsRx.Persistence.Editor.EditorInputs.Unity;
using UnityEngine;

namespace EcsRx.Persistence.Editor.EditorInputs.Reactive
{
    public class ReactivePropertyRectEditorInput : ReactivePropertyEditorInput<Rect>
    {
        protected override Rect CreateTypeUI(string label, Rect value)
        { return RectEditorInput.TypeUI(label, value); }
    }
}