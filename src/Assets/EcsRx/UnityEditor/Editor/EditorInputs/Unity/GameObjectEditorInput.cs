using UnityEditor;
using UnityEngine;

namespace EcsRx.UnityEditor.Editor.EditorInputs.Unity
{
    public class GameObjectEditorInput : SimpleEditorInput<GameObject>
    {
        public static GameObject TypeUI(string label, GameObject value)
        { return (GameObject)EditorGUILayout.ObjectField(label, value, typeof(GameObject), true); }
        
        protected override GameObject CreateTypeUI(string label, GameObject value)
        { return TypeUI(label, value); }
    }
}
