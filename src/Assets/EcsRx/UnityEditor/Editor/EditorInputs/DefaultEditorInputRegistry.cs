using System;
using System.Collections.Generic;
using EcsRx.UnityEditor.Editor.EditorInputs.Basic;
using EcsRx.UnityEditor.Editor.EditorInputs.Reactive;
using EcsRx.UnityEditor.Editor.EditorInputs.Unity;

namespace EcsRx.UnityEditor.Editor.EditorInputs
{
    public static class DefaultEditorInputRegistry
    {
        private static readonly EditorInputRegistry _editorInputRegistry;

        static DefaultEditorInputRegistry()
        {
            _editorInputRegistry = new EditorInputRegistry(new List<IEditorInput>
            {
                new IntEditorInput(),
                new FloatEditorInput(),
                new StringEditorInput(),
                new BoolEditorInput(),
                new Vector2EditorInput(),
                new Vector3EditorInput(),
                new ColorEditorInput(),
                new BoundsEditorInput(),
                new RectEditorInput(),
                new EnumEditorInput(),
                new ReactivePropertyIntEditorInput(),
                new ReactivePropertyFloatEditorInput(),
                new ReactivePropertyStringEditorInput(),
                new ReactivePropertyBoolEditorInput(),
                new ReactivePropertyVector2EditorInput(),
                new ReactivePropertyVector3EditorInput(),
                new ReactivePropertyColorEditorInput(),
                new ReactivePropertyBoundsEditorInput(),
                new ReactivePropertyRectEditorInput(),
                new GameObjectEditorInput()
            });
        }

        public static IEditorInput GetHandlerFor(Type type)
        { return _editorInputRegistry.GetHandlerFor(type); }
    }
}