using System;

namespace EcsRx.Unity.Helpers.EditorInputs
{
    public interface IEditorInput
    {
        bool HandlesType(Type type);
        object CreateUI(string label, object value);
    }
}