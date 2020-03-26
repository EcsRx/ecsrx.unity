using System;
using System.Collections.Generic;
using System.Linq;
using EcsRx.Collections;
using EcsRx.Collections.Database;
using EcsRx.Collections.Entity;
using EcsRx.Components;
using EcsRx.Entities;
using EcsRx.Extensions;
using EcsRx.Plugins.Views.Components;
using EcsRx.Unity.MonoBehaviours;
using EcsRx.UnityEditor.Extensions;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace EcsRx.UnityEditor.MonoBehaviours
{
    public class RegisterAsEntity : MonoBehaviour
    {
        [Inject]
        public IEntityDatabase EntityDatabase { get; private set; }

        [FormerlySerializedAs("CollectionName")] 
        [SerializeField]
        public int CollectionId;

        [SerializeField]
        public List<string> Components = new List<string>();

        [SerializeField]
        public List<string> ComponentEditorState = new List<string>();

        [Inject]
        public void RegisterEntity()
        {
            if (!gameObject.activeInHierarchy || !gameObject.activeSelf) { return; }

            IEntityCollection collectionToUse;

            if (CollectionId == 0)
            { collectionToUse = EntityDatabase.GetCollection(); }
            else if (EntityDatabase.Collections.All(x => x.Id != CollectionId))
            { collectionToUse = EntityDatabase.CreateCollection(CollectionId); }
            else
            { collectionToUse = EntityDatabase.GetCollection(CollectionId); }

            var createdEntity = collectionToUse.CreateEntity();
            createdEntity.AddComponents(new ViewComponent { View = gameObject });
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
            var componentsToRegister = new IComponent[Components.Count];
            for (var i = 0; i < Components.Count; i++)
            {
                var typeName = Components[i];
                var type = Type.GetType(typeName);
                if (type == null) { throw new Exception("Cannot resolve type for [" + typeName + "]"); }

                var component = (IComponent)Activator.CreateInstance(type);
                var componentProperties = JSON.Parse(ComponentEditorState[i]);
                component.DeserializeComponent(componentProperties);
                componentsToRegister[i] = component;
            }
            entity.AddComponents(componentsToRegister);
        }
        
        public IEntityCollection GetCollection()
        {
            return EntityDatabase.GetCollection(CollectionId);
        }
    }
}