using EcsRx.Persistence.Editor.EditorInputs.Unity;
using UnityEngine;

namespace EcsRx.Persistence.Editor.EditorInputs.Reactive
{
    public class ReactivePropertyQuaternionEditorInput : ReactivePropertyEditorInput<Quaternion>
    {
        protected override Quaternion CreateTypeUI(string label, Quaternion value)
        { return QuaternionEditorInput.TypeUI(label, value); }
    }
}