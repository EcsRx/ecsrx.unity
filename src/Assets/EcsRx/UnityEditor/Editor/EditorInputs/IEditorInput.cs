using System;

namespace EcsRx.UnityEditor.Editor.EditorInputs
{
    public interface IEditorInput
    {
        bool HandlesType(Type type);
        object CreateUI(string label, object value);
    }
}