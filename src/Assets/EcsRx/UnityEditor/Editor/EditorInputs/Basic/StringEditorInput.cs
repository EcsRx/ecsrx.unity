using UnityEditor;
using UnityEngine;

namespace EcsRx.UnityEditor.Editor.EditorInputs.Basic
{
    public class StringEditorInput : SimpleEditorInput<string>
    {
        public static string TypeUI(string label, string value)
        { return EditorGUILayout.TextField(label, value); }
        
        protected override string CreateTypeUI(string label, string value)
        { return TypeUI(label, value); }
    }
    
    public class ReadOnlyStringEditorInput : SimpleEditorInput<string>
    {
        public static string TypeUI(string label, string value)
        {
            EditorGUILayout.LabelField(label, value);
            return string.Empty;
        }
        
        protected override string CreateTypeUI(string label, string value)
        { return TypeUI(label, value); }
    }
}