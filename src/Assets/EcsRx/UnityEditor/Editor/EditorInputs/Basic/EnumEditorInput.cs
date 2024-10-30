using System;
using UnityEditor;

namespace EcsRx.UnityEditor.Editor.EditorInputs.Basic
{
    public class EnumEditorInput : SimpleEditorInput<Enum>
    {
        public override bool HandlesType(Type type)
        { return type.IsEnum; }
        
        public static Enum TypeUI(string label, Enum value)
        { return EditorGUILayout.EnumPopup(label, value); }
        
        protected override Enum CreateTypeUI(string label, Enum value)
        { return TypeUI(label, value); }
    }
}
