using System.Collections.Generic;
using UnityEngine.SceneManagement;
using ModestTree;

namespace Zenject
{
    public class SceneContextRegistry
    {
        readonly Dictionary<Scene, SceneContext> _map = new Dictionary<Scene, SceneContext>();

        public void Add(SceneContext context)
        {
            Assert.That(!_map.ContainsKey(context.gameObject.scene));
            _map.Add(context.gameObject.scene, context);
        }

        public SceneContext GetSceneContextForScene(Scene scene)
        {
            return _map[scene];
        }

        public void Remove(SceneContext context)
        {
            _map.RemoveWithConfirm(context.gameObject.scene);
        }
    }

}
