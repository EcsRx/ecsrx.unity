using EcsRx.UnityEditor.Editor.EditorInputs.Unity;
using UnityEngine;

namespace EcsRx.UnityEditor.Editor.EditorInputs.Reactive
{
    public class ReactivePropertyQuaternionEditorInput : ReactivePropertyEditorInput<Quaternion>
    {
        protected override Quaternion CreateTypeUI(string label, Quaternion value)
        { return QuaternionEditorInput.TypeUI(label, value); }
    }
}