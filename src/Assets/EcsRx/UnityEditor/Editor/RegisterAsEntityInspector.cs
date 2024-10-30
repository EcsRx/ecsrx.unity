using System;
using System.Collections.Generic;
using System.Linq;
using EcsRx.Components;
using EcsRx.Plugins.Views.Components;
using EcsRx.UnityEditor.Editor.Extensions;
using EcsRx.UnityEditor.Editor.UIAspects;
using EcsRx.UnityEditor.Extensions;
using EcsRx.UnityEditor.MonoBehaviours;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UEditor = UnityEditor.Editor;

namespace EcsRx.UnityEditor.Editor
{
    [Serializable]
    [CustomEditor(typeof(RegisterAsEntity))]
    public partial class RegisterAsEntityInspector : UEditor
    {
        private RegisterAsEntity _registerAsEntity;
        private EntityDataUIAspect _entityDataAspect;

        private void PoolSection()
        {
            this.UseVerticalBoxLayout(() =>
            {
                _registerAsEntity.CollectionId = this.WithNumberField("Entity Collection Id:", _registerAsEntity.CollectionId);
            });
        }

        private void OnEnable()
        {
            _registerAsEntity = (RegisterAsEntity)target;
            _entityDataAspect = new EntityDataUIAspect(_registerAsEntity.EntityData, this);
            _registerAsEntity.DeserializeState();

            if (_registerAsEntity.EntityData.EntityId != 0)
            { _registerAsEntity.EntityId = _registerAsEntity.EntityData.EntityId; }
            else
            { _registerAsEntity.EntityData.EntityId = _registerAsEntity.EntityId; }
        }

        private void PersistChanges()
        {
            if (GUI.changed)
            { this.SaveActiveSceneChanges(); }
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
