using System;
using System.Collections.Generic;
using System.Linq;
using EcsRx.Components;
using EcsRx.Extensions;
using EcsRx.Unity.MonoBehaviours;
using EcsRx.UnityEditor.Data;
using EcsRx.UnityEditor.Editor.Helpers;
using EcsRx.UnityEditor.Editor.UIAspects;
using UnityEditor;
using UnityEngine;

namespace EcsRx.UnityEditor.Editor
{
    [CustomEditor(typeof(EntityView))]
    public class EntityViewInspector : global::UnityEditor.Editor
    {
        private EntityView _entityView;
        private EntityDataUIAspect _entityDataAspect;
        private EntityData _entityDataProxy;
        
        private void OnEnable()
        {
            _entityView = (EntityView)target;
            _entityDataProxy = new EntityData();
            _entityDataAspect = new EntityDataUIAspect(_entityDataProxy, this);

            _entityDataProxy.EntityId = _entityView.Entity.Id;
            _entityDataProxy.Components = new List<IComponent>(_entityView.Entity.Components);

            _entityDataAspect.ComponentAdded += (sender, args) => _entityView.Entity.AddComponents(args.Component);
            _entityDataAspect.ComponentRemoved += (sender, args) => _entityView.Entity.RemoveComponents(args.Component.GetType());
        }
        
        private void PoolSection()
        {
            EditorGUIHelper.WithVerticalLayout(() =>
            {
                if (GUILayout.Button("Destroy Entity", GUILayout.ExpandWidth(true)))
                {
                    _entityView.EntityCollection.RemoveEntity(_entityView.Entity.Id);
                    Destroy(_entityView.gameObject);
                }

                EditorGUIHelper.WithVerticalLayout(() =>
                {
                    var entityId = _entityView.Entity.Id.ToString();
                    EditorGUIHelper.WithLabelField("Entity Id", entityId);
                });

                EditorGUIHelper.WithVerticalLayout(() =>
                {
                    var collectionId = _entityView.EntityCollection.Id.ToString();
                    EditorGUIHelper.WithLabelField("Collection Id", collectionId);
                });
            });
        }
        
        private void SyncAnyExternalChanges()
        {
            var hasChanged = false;
            foreach (var component in _entityView.Entity.Components)
            {
                if (!_entityDataProxy.Components.Contains(component))
                {
                    _entityDataProxy.Components.Add(component);
                    hasChanged = true;
                }
            }
            
            for (var i = _entityDataProxy.Components.Count - 1; i >= 0; i--)
            {
                var previousComponent = _entityDataProxy.Components[i];
                if (!_entityView.Entity.Components.Contains(previousComponent))
                {
                    _entityDataProxy.Components.RemoveAt(i);
                    hasChanged = true;
                }
            }

            if(hasChanged)
            { Repaint(); }
        }

        public override void OnInspectorGUI()
        {
            _entityView = (EntityView)target;

            if (_entityView.Entity == null)
            {
                EditorGUILayout.LabelField("No Entity Assigned");
                return;
            }

            SyncAnyExternalChanges();

            PoolSection();

            _entityDataAspect.DisplayUI();
        }
    }
}