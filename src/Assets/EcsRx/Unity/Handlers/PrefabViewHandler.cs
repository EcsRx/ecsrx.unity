using EcsRx.Views.ViewHandlers;
using UnityEngine;
using Zenject;

namespace EcsRx.Unity.Handlers
{
    public class PrefabViewHandler : IViewHandler
    {
        public IInstantiator Instantiator { get; }
        protected GameObject PrefabTemplate { get; }
        
        public PrefabViewHandler(IInstantiator instantiator, GameObject prefabTemplate)
        {
            Instantiator = instantiator;
            PrefabTemplate = prefabTemplate;
        }
        
        public void DestroyView(object view)
        { Object.Destroy(view as GameObject); }

        public void SetActiveState(object view, bool isActive)
        { (view as GameObject).SetActive(isActive); }

        public object CreateView()
        {
            var createdPrefab = Instantiator.InstantiatePrefab(PrefabTemplate);
            createdPrefab.transform.position = Vector3.zero;
            createdPrefab.transform.rotation = Quaternion.identity;
            return createdPrefab;
        }
    }
}