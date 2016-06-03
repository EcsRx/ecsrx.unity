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
    [System.Serializable]
    public class EntityViewInspector : Editor
    {
        EntityView view;
        IEntity entity;

        List<Type> components = new List<Type>();
        string[] types;
        bool showComponents;

        void Awake()
        {
            // components.Add(typeof(CubeComponent));
            // components.Add(typeof(SphereComponent));
            // types = components.Select(_ => _.ToString()).ToArray();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();

            if (GUILayout.Button("Create Entity"))
            {
                var id = UnityEngine.Random.Range(9999, 999999999);
                view.Entity = new Entity(id, null);
            }

            if (GUILayout.Button("Destroy Entity"))
            {
                view.Pool.RemoveEntity(entity);
                Destroy(view.gameObject);
            }

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

            components.Add(typeof(CubeComponent));
            components.Add(typeof(SphereComponent));
            types = components.Select(_ => _.ToString()).ToArray();
            var index = -1;
            index = EditorGUILayout.Popup("Add Component", index, types);
            if (index >= 0)
            {
                var type = components[index];
                var component = (IComponent)Activator.CreateInstance(type);

                if (Application.isPlaying)
                {
                    entity.AddComponent(component);
                }
                else
                {
                    Debug.Log(component.ToString());
                    view.StagedComponents.Add(component);
                }
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
                for (var i = 0; i < entity.Components.Count(); i++)
                {
                    EditorGUILayout.BeginHorizontal(GUI.skin.box);
                    EditorGUILayout.LabelField(entity.Components.ElementAt(i).GetType().Name);
                    if (GUILayout.Button("-", GUILayout.Width(20), GUILayout.Height(15)))
                    {
                        entity.RemoveComponent(entity.Components.ElementAt(i));
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();
            }
        }
    }
}
