using System;

namespace EcsRx.Persistence.Editor.EditorInputs
{
    public interface IEditorInput
    {
        bool HandlesType(Type type);
        UIStateChange CreateUI(string label, object value);
    }
}