using UniRx;
using UnityEditor;

namespace EcsRx.Unity.Helpers.EditorInputs
{
    public class ReactiveVector3EditorInput : SimpleEditorInput<Vector3ReactiveProperty>
    {
        protected override Vector3ReactiveProperty CreateTypeUI(string label, Vector3ReactiveProperty value)
        {
            value.Value = EditorGUILayout.Vector3Field(label, value.Value);
            return null;
        }
    }
}