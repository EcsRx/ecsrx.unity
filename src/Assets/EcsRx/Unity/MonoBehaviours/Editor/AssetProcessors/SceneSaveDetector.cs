using EcsRx.Events;
using EcsRx.Unity.MonoBehaviours.Editor.Events;
using EcsRx.Unity.MonoBehaviours.Editor.Infrastructure;

namespace EcsRx.Tests.AssetProcessors
{
    class SceneSaveDetector : UnityEditor.AssetModificationProcessor
    {
        private void OnWillSaveAssets(string[] paths)
        {
            var eventSystem = EditorContext.Container.Resolve<IEventSystem>();
            eventSystem.Publish(new SceneSavedEvent());
        }
    }
}