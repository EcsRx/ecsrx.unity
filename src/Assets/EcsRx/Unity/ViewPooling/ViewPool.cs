using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Assets.EcsRx.Unity.ViewPooling
{
    public class ViewPool : IViewPool
    {
        private readonly IList<ViewObjectContainer> _pooledObjects = new List<ViewObjectContainer>();
        private readonly IInstantiator _instantiator;

        public GameObject Prefab { get; private set; }
        public int IncrementSize { get; private set; }

        public ViewPool(IInstantiator instantiator, GameObject prefab, int incrementSize = 5)
        {
            Prefab = prefab;
            IncrementSize = incrementSize;
            _instantiator = instantiator;
        }

        public void PreAllocate(int allocationCount)
        {
            for (var i = 0; i < allocationCount; i++)
            {
                var newInstance = _instantiator.InstantiatePrefab(Prefab);
                var objectContainer = new ViewObjectContainer(newInstance);
                _pooledObjects.Add(objectContainer);
            }
        }

        public GameObject AllocateInstance()
        {
            var availableViewObject = _pooledObjects.First(x => !x.IsInUse);
            if (availableViewObject == null)
            {
                PreAllocate(IncrementSize);
                availableViewObject = _pooledObjects.First(x => !x.IsInUse);
            }

            availableViewObject.IsInUse = true;
            return availableViewObject.ViewObject;
        }
        
        public void ReleaseInstance(GameObject instance)
        {
            var container = _pooledObjects.First(x => x.ViewObject == instance);
            if(container == null) { return; }

            container.IsInUse = false;
            var gameObject = container.ViewObject;
            gameObject.SetActive(false);
        }

        public void EmptyPool()
        { _pooledObjects.Clear(); }
    }
}