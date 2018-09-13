using System.Collections.Generic;
using System.Linq;
using EcsRx.Components;
using EcsRx.Persistence.Data;
using EcsRx.Persistence.Editor.EditorHelper;
using EcsRx.Persistence.Editor.UIAspects;
using EcsRx.Persistence.MonoBehaviours;
using UnityEditor;
using UnityEngine;

namespace EcsRx.Unity.Helpers
{
    [CustomEditor(typeof(EntityView))]
    public class EntityViewInspector : Editor
    {
        private EntityView _entityView;
        private EntityData _entityDataProxy;
        private EntityDataUIAspect _entityDataAspect;

        private void PoolSection()
        {
            EditorGUIHelper.WithVerticalLayout(() =>
            {
                if (GUILayout.Button("Destroy Entity"))
                {
                    _entityView.EntityCollection.RemoveEntity(_entityView.Entity.Id);
                    Destroy(_entityView.gameObject);
                }

                EditorGUIHelper.WithVerticalLayout(() =>
                {
                    var id = _entityView.Entity.Id.ToString();
                    EditorGUIHelper.WithLabelField("Entity Id", id);
                });

                EditorGUIHelper.WithVerticalLayout(() =>
                {
                    EditorGUIHelper.WithLabelField("Pool", _entityView.EntityCollection.Name);
                });
            });
        }

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