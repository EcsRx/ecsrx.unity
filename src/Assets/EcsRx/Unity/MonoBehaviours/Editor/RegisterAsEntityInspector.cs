using EcsRx.Unity.MonoBehaviours;
using UnityEngine;
using UnityEditor;
using System;
using EcsRx.Unity.MonoBehaviours.Editor.EditorHelper;
using EcsRx.Unity.MonoBehaviours.Editor.Infrastructure;
using EcsRx.Unity.MonoBehaviours.Editor.UIAspects;
using Persistity.Serialization.Binary;

namespace EcsRx.Unity.Helpers
{
    [Serializable]
    [CustomEditor(typeof(RegisterAsEntity))]
    public partial class RegisterAsEntityInspector : Editor
    {
        private RegisterAsEntity _registerAsEntity;
        private EntityDataUIAspect _entityDataAspect;

        private void PoolSection()
        {
            EditorGUIHelper.WithVerticalLayout(() =>
            {
                _registerAsEntity.PoolName = EditorGUIHelper.WithTextField("Pool", _registerAsEntity.PoolName);
            });
        }

        private void PersistChanges()
        {
            _registerAsEntity.EntityId = _registerAsEntity.EntityData.EntityId;
            if (GUI.changed)
            {
                this._registerAsEntity.SerializeState();
                this.SaveActiveSceneChanges();
            }
        }

        private void OnEnable()
        {
            _registerAsEntity = (RegisterAsEntity)target;
            _entityDataAspect = new EntityDataUIAspect(_registerAsEntity.EntityData, this);

            InjectIntoTarget();
            _registerAsEntity.DeserializeState();

            if (_registerAsEntity.EntityData.EntityId != Guid.Empty)
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

            PersistChanges();
        }
    }
}