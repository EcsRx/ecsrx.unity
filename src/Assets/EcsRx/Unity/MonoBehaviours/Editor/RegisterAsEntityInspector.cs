using EcsRx.Unity.MonoBehaviours;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using EcsRx.Persistence.Data;
using EcsRx.Persistence.Database.Accessor;
using EcsRx.Persistence.Events;
using EcsRx.Unity.MonoBehaviours.Editor.EditorHelper;
using EcsRx.Unity.MonoBehaviours.Editor.Infrastructure;
using EcsRx.Unity.MonoBehaviours.Editor.UIAspects;
using UniRx;

namespace EcsRx.Unity.Editors
{
    [Serializable]
    [CustomEditor(typeof(RegisterAsEntity))]
    public partial class RegisterAsEntityInspector : InjectedEditor
    {
        private IApplicationDatabaseAccessor _databaseAccessor;

        private RegisterAsEntity _registerAsEntity;
        private EntityDataUIAspect _entityDataAspect;

        private EntityData _entityData;
        private PoolData _currentPool;
        
        private void PoolSection()
        {
            EditorGUIHelper.WithVerticalLayout(() =>
            {
                _registerAsEntity.PoolName = EditorGUIHelper.WithTextField("Pool", _registerAsEntity.PoolName);
            });
        }

        private void EntityIdSection()
        {
            EditorGUIHelper.WithVerticalLayout(() =>
            {
                EditorGUIHelper.WithHorizontalLayout(() =>
                {
                    EditorGUILayout.LabelField("Entity Id", GUILayout.MaxWidth(100.0f));
                    var entityId = EditorGUILayout.TextField(_registerAsEntity.EntityId.ToString());
                    Guid entityGuid;

                    try
                    { entityGuid = new Guid(entityId); }
                    catch (Exception)
                    { return; }

                    if (EditorGUIHelper.WithIconButton("↻", "Generate new GUID"))
                    { entityGuid = Guid.NewGuid(); }

                    if (entityGuid != _registerAsEntity.EntityId)
                    {
                        _entityData.EntityId = entityGuid;
                        _registerAsEntity.EntityId = entityGuid;
                    }
                });
            });
        }
        
        private void OnDestroy()
        {
            if (WasInPlaymode)
            {
                if (EditorUtility.DisplayDialog("Removing Entity", "Do you want to remove this entity from the database too?", "Yes", "No"))
                {
                    _currentPool.Entities.Remove(_entityData);
                    if (_currentPool.Entities.Count == 0)
                    { _databaseAccessor.Database.Pools.Remove(_currentPool); }
                }
            }
        }

        private void PersistChanges()
        {
            _registerAsEntity.EntityId = _entityData.EntityId;
            if (GUI.changed)
            {
                this.SaveActiveSceneChanges();
                _databaseAccessor.PersistDatabase(() => { Debug.Log("Saved Database"); });
            }
        }

        private void OnEnable()
        {
            _registerAsEntity = (RegisterAsEntity)target;
        }
        
        private void OnDatabaseLoaded()
        {
            Debug.Log("Database Loaded");
            var poolToUse = string.IsNullOrEmpty(_registerAsEntity.PoolName) ? "default" : _registerAsEntity.PoolName.ToLower();

            var _currentPool = _databaseAccessor.Database.Pools.SingleOrDefault(x => x.PoolName.ToLower() == poolToUse);
            if (_currentPool == null)
            {
                _currentPool = new PoolData {PoolName = _registerAsEntity.PoolName};
                _databaseAccessor.Database.Pools.Add(_currentPool);
            }

            _entityData = _currentPool.Entities.SingleOrDefault(x => x.EntityId == _registerAsEntity.EntityId);
            if (_entityData == null)
            {
                _entityData = new EntityData { EntityId = _registerAsEntity.EntityId };
                _currentPool.Entities.Add(_entityData);
            }

            _entityDataAspect = new EntityDataUIAspect(_entityData, this);
        }

        public override void OnInspectorGUI()
        {
            if (_entityDataAspect == null) { return; }

            PoolSection();
            EntityIdSection();
            _entityDataAspect.DisplayUI();
            PersistChanges();
        }

        protected override void SetupDependencies()
        {
            base.SetupDependencies();

            _databaseAccessor = EditorContext.Container.Resolve<IApplicationDatabaseAccessor>();
        }

        protected override void OnDependenciesRegistered()
        {
            if (_databaseAccessor.HasInitialized)
            {
                OnDatabaseLoaded();
                return;
            }

            EventSystem.Receive<ApplicationDatabaseLoadedEvent>()
                .First()
                .Subscribe(evt => OnDatabaseLoaded());
        }
    }
}
