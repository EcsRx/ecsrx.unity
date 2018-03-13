using UnityEngine;
using ModestTree;

namespace Zenject
{
    public class ZenAutoInjecter : MonoBehaviour
    {
        [SerializeField]
        ContainerSources _containerSource;

        bool _hasStarted;

        // Make sure they don't cause injection to happen twice
        [Inject]
        public void Construct()
        {
            if (!_hasStarted)
            {
                throw Assert.CreateException(
                    "ZenAutoInjecter was injected!  Do not use ZenAutoInjecter for objects that are instantiated through zenject or which exist in the initial scene hierarchy");
            }
        }

        public void Start()
        {
            _hasStarted = true;
            LookupContainer().InjectGameObject(this.gameObject);
        }

        DiContainer LookupContainer()
        {
            if (_containerSource == ContainerSources.ProjectContext)
            {
                return ProjectContext.Instance.Container;
            }

            Assert.IsEqual(_containerSource, ContainerSources.SceneContext);

            return ProjectContext.Instance.Container.Resolve<SceneContextRegistry>()
                .GetSceneContextForScene(this.gameObject.scene).Container;
        }

        public enum ContainerSources
        {
            SceneContext,
            ProjectContext
        }
    }
}
