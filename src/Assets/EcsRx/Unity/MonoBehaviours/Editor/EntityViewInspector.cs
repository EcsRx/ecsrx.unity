namespace EcsRx.Unity.Helpers
{
    using UnityEngine;
    using UnityEditor;
    using System.Collections.Generic;
    using EcsRx.Components;
    using System;
    using System.Linq;

    [CustomEditor(typeof(EntityView))]
    [System.Serializable]
    public class EntityViewInspector : Editor
    {
        EntityView view;

        List<Type> components = new List<Type>();
        string[] types;
        bool showComponents;

        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Destroy Entity"))
            {
                view.Pool.RemoveEntity(view.Entity);
                Destroy(view.gameObject);
            }

            view = (EntityView)target;

            if (Application.isPlaying)
            {
                if (view.Entity == null)
                {
                    EditorGUILayout.LabelField("No Entity Assigned");
                    return;
                }

                EditorGUILayout.BeginVertical();

                EditorGUILayout.BeginHorizontal();
                var id = view.Entity.Id.ToString();
                EditorGUILayout.LabelField("Entity Id: ", id);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                var pool = string.IsNullOrEmpty(view.PoolName) ? "Default" : view.PoolName;
                EditorGUILayout.LabelField("Pool: ", pool);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();

								EditorGUILayout.BeginHorizontal(GUI.skin.box);
								var type = typeof(IComponent);
                components = AppDomain.CurrentDomain.GetAssemblies()
																		.SelectMany(s => s.GetTypes())
                                    .Where(p => type.IsAssignableFrom(p)).ToList();
                types = components.Select(_ => _.ToString()).ToArray();
                var index = -1;
                index = EditorGUILayout.Popup("Add Component", index, types);
                if (index >= 0)
                {
                    var component = (IComponent)Activator.CreateInstance(components[index]);
                    view.Entity.AddComponent(component);
                }
								EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal(GUI.skin.box);
                EditorGUILayout.LabelField("Components (" + view.Entity.Components.Count() + ")", EditorStyles.boldLabel);
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
                    for (var i = 0; i < view.Entity.Components.Count(); i++)
                    {
                        EditorGUILayout.BeginHorizontal(GUI.skin.box);
                        EditorGUILayout.LabelField(view.Entity.Components.ElementAt(i).GetType().Name);
                        if (GUILayout.Button("-", GUILayout.Width(20), GUILayout.Height(15)))
                        {
                            view.Entity.RemoveComponent(view.Entity.Components.ElementAt(i));
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }
            }
            else
            {
								EditorGUILayout.BeginHorizontal(GUI.skin.box);
								var type = typeof(IComponent);
                components = AppDomain.CurrentDomain.GetAssemblies()
																		.SelectMany(s => s.GetTypes())
                                    .Where(p => type.IsAssignableFrom(p)).ToList();
                types = components.Select(_ => _.ToString()).ToArray();
                var index = -1;
                index = EditorGUILayout.Popup("Add Component", index, types);
                if (index >= 0)
                {
                    view.StagedComponents.Add(types[index]);
                }
								EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal(GUI.skin.box);
                EditorGUILayout.LabelField("Components (" + view.StagedComponents.Count() + ")", EditorStyles.boldLabel);
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
                    var componentsToRemove = new List<int>();
                    for (var i = 0; i < view.StagedComponents.Count(); i++)
                    {
                        EditorGUILayout.BeginHorizontal(GUI.skin.box);
                        EditorGUILayout.LabelField(view.StagedComponents[i]);
                        if (GUILayout.Button("-", GUILayout.Width(20), GUILayout.Height(15)))
                        {
                            componentsToRemove.Add(i);
                        }
                        EditorGUILayout.EndHorizontal();
                    }

                    for (var i = 0; i < componentsToRemove.Count(); i++)
                    {
                        view.StagedComponents.RemoveAt(componentsToRemove[i]);
                    }
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
