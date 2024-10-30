using UnityEditor;
using UnityEngine;

namespace EcsRx.UnityEditor.Editor.EditorInputs.Unity
{
    public class QuaternionEditorInput : SimpleEditorInput<Quaternion>
    {
        public static Quaternion TypeUI(string label, Quaternion value)
        {
            var v4 = new Vector4(value.x, value.y, value.z, value.w);
            var outputv4 = EditorGUILayout.Vector4Field(label, v4);
            return new Quaternion(outputv4.x, outputv4.y, outputv4.z, outputv4.w);
        }
        
        protected override Quaternion CreateTypeUI(string label, Quaternion value)
        { return TypeUI(label, value); }
    }
}