using EcsRx.UnityEditor.Editor.EditorInputs.Unity;
using UnityEngine;

namespace EcsRx.UnityEditor.Editor.EditorInputs.Reactive
{
    public class ReactivePropertyVector3EditorInput : ReactivePropertyEditorInput<Vector3>
    {
        protected override Vector3 CreateTypeUI(string label, Vector3 value)
        { return Vector3EditorInput.TypeUI(label, value); }
    }
}