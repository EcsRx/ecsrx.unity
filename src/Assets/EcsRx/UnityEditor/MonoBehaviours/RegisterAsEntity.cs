using System;
using System.Collections.Generic;
using System.Linq;
using EcsRx.Collections.Database;
using EcsRx.Collections.Entity;
using EcsRx.Components;
using EcsRx.Entities;
using EcsRx.Extensions;
using EcsRx.Plugins.Views.Components;
using EcsRx.Unity.MonoBehaviours;
using EcsRx.UnityEditor.Data;
using EcsRx.UnityEditor.Extensions;
using UnityEngine;
using Zenject;

namespace EcsRx.UnityEditor.MonoBehaviours
{
    public class RegisterAsEntity : MonoBehaviour
    {
        [Inject]
        public IEntityDatabase EntityDatabase { get; private set; }

        [SerializeField]
        public int CollectionId;
        
        [SerializeField]
        public int EntityId;

        [SerializeField]
        public List<string> Components = new List<string>();

        [SerializeField]
        public List<string> ComponentEditorState = new List<string>();
        
        public bool HasDeserialized = false;
        public EntityData EntityData = new EntityData();
        
        public void SerializeState()
        {
            EntityData.EntityId = EntityId;

            foreach (var component in EntityData.Components)
            {
                var componentName = component.ToString();
                Components.Add(componentName);
                var json = component.SerializeComponent();
                ComponentEditorState.Add(json.ToString());
            }
        }

        public void DeserializeState()
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
            
            EntityData.Components = componentsToRegister;
            EntityData.EntityId = EntityId;
            HasDeserialized = true;
        }
        
        private IEntityCollection GetCollectionManager()
        {
            if (CollectionId == 0)
            { return EntityDatabase.GetCollection(); }

            if (EntityDatabase.Collections.All(x => x.Id != CollectionId))
            { return EntityDatabase.CreateCollection(CollectionId); }

            return EntityDatabase.GetCollection(CollectionId);
        }

        public IEntity CreateEntity(IEntityCollection collectionToUse)
        {
            if(EntityId > 0)
            { return collectionToUse.CreateEntity(null, EntityId); }

            return collectionToUse.CreateEntity();
        }
        
        [Inject]
        public void RegisterEntity()
        {
            if (!gameObject.activeInHierarchy || !gameObject.activeSelf) { return; }

            var collectionToUse = GetCollectionManager();
            var createdEntity = CreateEntity(collectionToUse);
            createdEntity.AddComponents(EntityData.Components.ToArray());
            createdEntity.AddComponents(new ViewComponent { View = gameObject });
            SetupEntityBinding(createdEntity, collectionToUse);

            Destroy(this);
        }

        private void SetupEntityBinding(IEntity entity, IEntityCollection entityCollection)
        {
            var entityBinding = gameObject.AddComponent<EntityView>();
            entityBinding.Entity = entity;
            entityBinding.EntityCollection = entityCollection;
        }
    }
}