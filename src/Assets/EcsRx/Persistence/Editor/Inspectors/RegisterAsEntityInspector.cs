using System;
using EcsRx.Infrastructure.Extensions;
using EcsRx.Persistence.Editor.EditorHelper;
using EcsRx.Persistence.Editor.Infrastructure;
using EcsRx.Persistence.Editor.UIAspects;
using EcsRx.Persistence.MonoBehaviours;
using LazyData.Binary;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EcsRx.Persistence.Editor.Inspectors
{
    [Serializable]
    [CustomEditor(typeof(RegisterAsEntity))]
    public partial class RegisterAsEntityInspector : UnityEditor.Editor
    {
        private RegisterAsEntity _registerAsEntity;
        private EntityDataUIAspect _entityDataAspect;

        private void PoolSection()
        {
            EditorGUIHelper.WithVerticalLayout(() =>
            {
                _registerAsEntity.CollectionName = EditorGUIHelper.WithTextField("Collection Name", _registerAsEntity.CollectionName);
            });
        }

        private void PersistChanges()
        {
            _registerAsEntity.EntityId = _registerAsEntity.EntityData.EntityId;
            if (GUI.changed)
            { _registerAsEntity.SerializeState(); }
        }

        private void OnEnable()
        {
            _registerAsEntity = (RegisterAsEntity)target;
            _entityDataAspect = new EntityDataUIAspect(_registerAsEntity.EntityData, this);

            InjectIntoTarget();
            _registerAsEntity.DeserializeState();

            if (_registerAsEntity.EntityData.EntityId != 0)
            { _registerAsEntity.EntityId = _registerAsEntity.EntityData.EntityId; }
            else
            { _registerAsEntity.EntityData.EntityId = _registerAsEntity.EntityId; }
        }

        private void InjectIntoTarget()
        {
            var serializer = EditorContext.Container.Resolve<IBinarySerializer>();
            var deserializer = EditorContext.Container.Resolve<IBinaryDeserializer>();
            _registerAsEntity.Serializer = serializer;
            _registerAsEntity.Deserializer = deserializer;
        }

        public override void OnInspectorGUI()
        {
            PoolSection();

            _entityDataAspect.DisplayUI();
            
            if(SceneManager.GetActiveScene().isDirty)
            { PersistChanges(); }
        }        
    }
}