using EcsRx.Events;
using EcsRx.Unity.MonoBehaviours.Editor.Infrastructure;
using UnityEditor;

namespace EcsRx.Unity.Editors
{
    public abstract class InjectedEditor : Editor
    {
        protected bool WasInPlaymode;
        protected IEventSystem EventSystem { get; private set; }

        protected virtual void OnComponentRemoved() {}
        protected abstract void OnDependenciesRegistered();

        protected virtual void SetupDependencies()
        {
            EventSystem = EditorContext.Container.Resolve<IEventSystem>();
        }

        private void OnEnable()
        {
            SetupDependencies();
            OnDependenciesRegistered();
            ListenForPlaystateChanges();
        }

        private void ListenForPlaystateChanges()
        {
            EditorApplication.playmodeStateChanged += () =>
            {
                if (!EditorApplication.isPlaying)
                { WasInPlaymode = true; }

                if (EditorApplication.isPlaying)
                { WasInPlaymode = false; }
            };
        }

    }
}