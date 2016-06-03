namespace EcsRx.Unity.Helpers
{
    using UnityEngine;
    using System.Collections;
    using UnityEditor;
    using Entities;
    using System.Collections.Generic;
    using EcsRx.Components;
    using Assets.Examples.ViewBinding.Components;
    using System;
    using System.Linq;

    [CustomEditor(typeof(EntityView))]
    public class EntityViewInspector : Editor
    {
        EntityView view;
        IEntity entity;

        List<Type> components = new List<Type>();
        string[] types;
        bool showComponents;

        void Awake()
        {
            components.Add(typeof(CubeComponent));
            components.Add(typeof(SphereComponent));
            types = components.Select(_ => _.ToString()).ToArray();
        }

        public override void OnInspectorGUI()
        {
            view = (EntityView)target;
            entity = view.Entity;

            if (entity == null)
            {
                EditorGUILayout.LabelField("No Entity Assigned");
                return;
            }

            var poolName = view.PoolName;

            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Entity Id");
            EditorGUILayout.LabelField(entity.Id.ToString());
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Pool Name");
            EditorGUILayout.LabelField(string.IsNullOrEmpty(poolName) ? "Default" : poolName);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();


            if (GUILayout.Button("Destroy Entity"))
            {
								view.Pool.RemoveEntity(entity);
                Destroy(view.gameObject);
            }

            EditorGUILayout.Space();
            var index = -1;
            index = EditorGUILayout.Popup("Add Component", index, types);
            if (index >= 0)
            {
                var type = components[index];
                var component = (IComponent)Activator.CreateInstance(type);
                entity.AddComponent(component);
            }

            EditorGUILayout.EndVertical();

						EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Components (" + entity.Components.Count() + ")", EditorStyles.boldLabel);
            if (GUILayout.Button("▸", GUILayout.Width(20), GUILayout.Height(15)))
            {
                showComponents = false;
            }
            if (GUILayout.Button("▾", GUILayout.Width(20), GUILayout.Height(15)))
            {
                showComponents = true;
            }
            EditorGUILayout.EndHorizontal();

            if (showComponents)
						{
							foreach (var component in entity.Components)
							{
									EditorGUILayout.BeginVertical(GUI.skin.box);
									EditorGUILayout.LabelField(component.GetType().Name);
									EditorGUILayout.EndVertical();
							}
						}
        }
    }
}
