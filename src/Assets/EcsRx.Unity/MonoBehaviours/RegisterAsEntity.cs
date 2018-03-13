using System;
using System.Collections.Generic;
using System.Linq;
using EcsRx.Collections;
using EcsRx.Components;
using EcsRx.Entities;
using EcsRx.Unity.Extensions;
using EcsRx.Views.Components;
using UnityEngine;
using Zenject;

namespace EcsRx.Unity.MonoBehaviours
{
    public class RegisterAsEntity : MonoBehaviour
    {
        [Inject]
        public IEntityCollectionManager CollectionManager { get; private set; }

        [SerializeField]
        public string PoolName;

        [SerializeField]
        public List<string> Components = new List<string>();

        [SerializeField]
        public List<string> ComponentEditorState = new List<string>();

        [Inject]
        public void RegisterEntity()
        {
            if (!gameObject.activeInHierarchy || !gameObject.activeSelf) { return; }

            IEntityCollection collectionToUse;

            if (string.IsNullOrEmpty(PoolName))
            { collectionToUse = CollectionManager.GetCollection(); }
            else if (CollectionManager.Pools.All(x => x.Name != PoolName))
            { collectionToUse = CollectionManager.CreateCollection(PoolName); }
            else
            { collectionToUse = CollectionManager.GetCollection(PoolName); }

            var createdEntity = collectionToUse.CreateEntity();
            createdEntity.AddComponent(new ViewComponent { View = gameObject });
            SetupEntityBinding(createdEntity, collectionToUse);
            SetupEntityComponents(createdEntity);

            Destroy(this);
        }

        private void SetupEntityBinding(IEntity entity, IEntityCollection entityCollection)
        {
            var entityBinding = gameObject.AddComponent<EntityView>();
            entityBinding.Entity = entity;
            entityBinding.EntityCollection = entityCollection;
        }

        private void SetupEntityComponents(IEntity entity)
        {
            for (var i = 0; i < Components.Count(); i++)
            {
                var typeName = Components[i];
                var type = Type.GetType(typeName);
                if (type == null) { throw new Exception("Cannot resolve type for [" + typeName + "]"); }

                var component = (IComponent)Activator.CreateInstance(type);
                var componentProperties = JSON.Parse(ComponentEditorState[i]);
                component.DeserializeComponent(componentProperties);

                entity.AddComponent(component);
            }
        }
        
        public IEntityCollection GetPool()
        {
            return CollectionManager.GetCollection(PoolName);
        }
    }
}