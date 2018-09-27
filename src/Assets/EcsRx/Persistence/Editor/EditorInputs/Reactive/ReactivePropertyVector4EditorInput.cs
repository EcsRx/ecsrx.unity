using EcsRx.Persistence.Editor.EditorInputs.Unity;
using UnityEngine;

namespace EcsRx.Persistence.Editor.EditorInputs.Reactive
{
    public class ReactivePropertyVector4EditorInput : ReactivePropertyEditorInput<Vector4>
    {
        protected override Vector4 CreateTypeUI(string label, Vector4 value)
        { return Vector4EditorInput.TypeUI(label, value); }
    }
}