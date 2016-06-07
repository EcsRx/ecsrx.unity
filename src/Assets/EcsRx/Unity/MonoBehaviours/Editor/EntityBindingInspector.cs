using System;
using System.Collections.Generic;
using System.Linq;
using EcsRx.Components;
using EcsRx.Unity.Helpers.Extensions;
using EcsRx.Unity.MonoBehaviours;
using UnityEditor;
using UnityEngine;

namespace EcsRx.Unity.Helpers
{
    [CustomEditor(typeof(EntityBinding))]
    public class EntityBindingInspector : Editor
    {
        private EntityBinding entityBinding;

        public bool showComponents;

        private IEnumerable<Type> GetAvailableComponents()
        {
            var type = typeof(IComponent);
            return AppDomain.CurrentDomain.GetAssemblies()
                                .SelectMany(s => s.GetTypes())
                                .Where(p => type.IsAssignableFrom(p));
        }

        public override void OnInspectorGUI()
        {
            entityBinding = (EntityBinding)target;

            if (entityBinding.Entity == null)
            {
                EditorGUILayout.LabelField("No Entity Assigned");
                return;
            }
            
            if (GUILayout.Button("Destroy Entity"))
            {
                entityBinding.Pool.RemoveEntity(entityBinding.Entity);
                Destroy(entityBinding.gameObject);
            }
            
            EditorGUILayout.BeginVertical();

            this.UseVerticalBoxLayout(() =>
            {
                var id = entityBinding.Entity.Id.ToString();
                this.WithLabelField("Entity Id: ", id);
            });

            this.UseVerticalBoxLayout(() =>
            {
                this.WithLabelField("Pool: ", entityBinding.Pool.Name);
            });
            
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
            
            this.UseVerticalBoxLayout(() =>
            {
                var components = GetAvailableComponents().ToArray();
                var types = components.Select(_ => _.ToString()).ToArray();
                var index = -1;
                index = EditorGUILayout.Popup("Add Component", index, types);
                if (index >= 0)
                {
                    var component = (IComponent)Activator.CreateInstance(components[index]);
                    entityBinding.Entity.AddComponent(component);
                }
            });
            
            this.UseHorizontalBoxLayout(() =>
            {
                this.WithLabel("Components (" + entityBinding.Entity.Components.Count() + ")");
                if (this.WithIconButton("▸"))
                { showComponents = false; }
                if (this.WithIconButton("▾"))
                { showComponents = true; }
            });

            if (showComponents)
            {
                for (var i = 0; i < entityBinding.Entity.Components.Count(); i++)
                {
                    this.UseVerticalBoxLayout(() =>
                    {
                        this.WithLabel(entityBinding.Entity.Components.ElementAt(i).GetType().Name);
                        if (this.WithIconButton("-"))
                        { entityBinding.Entity.RemoveComponent(entityBinding.Entity.Components.ElementAt(i)); }
                    });
                }
            }
        }
    }
}