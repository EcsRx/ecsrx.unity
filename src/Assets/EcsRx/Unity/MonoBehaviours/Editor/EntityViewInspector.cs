namespace EcsRx.Unity.Helpers
{
    using UnityEngine;
    using UnityEditor;
    using System.Collections.Generic;
    using EcsRx.Components;
    using System;
    using System.Linq;
    using Assets.Examples.ViewBinding.Components;
    using EcsRx.Json;
    using System.Reflection;

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
            serializedObject.Update();

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
                        EditorGUILayout.BeginVertical(GUI.skin.box);
                        EditorGUILayout.BeginHorizontal();
                        var label = view.Entity.Components.ElementAt(i).GetType().Name;
                        var dotIndex = label.LastIndexOf(".");
                        if (dotIndex != -1)
												{
                            label = label.Substring(dotIndex + 1);
                        }
                        EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
                        if (GUILayout.Button("-", GUILayout.Width(20), GUILayout.Height(15)))
                        {
                            view.Entity.RemoveComponent(view.Entity.Components.ElementAt(i));
                        }
                        EditorGUILayout.EndHorizontal();

                        var members = view.Entity.Components.ElementAt(i).GetType().GetMembers();
                        foreach (var property in view.Entity.Components.ElementAt(i).GetType().GetProperties())
                        {
                            bool isTypeSupported = true;
                            EditorGUILayout.BeginHorizontal();
                            var _type = property.PropertyType;
                            var _value = property.GetValue(view.Entity.Components.ElementAt(i), null);

                            if (_type == typeof(int))
                            {
                                _value = EditorGUILayout.IntField(property.Name, (int)_value);
                            }
                            else if (_type == typeof(float))
                            {
                                _value = EditorGUILayout.FloatField(property.Name, (float)_value);
                            }
                            else if (_type == typeof(bool))
                            {
                                _value = EditorGUILayout.Toggle(property.Name, (bool)_value);
                            }
                            else if (_type == typeof(string))
                            {
                                _value = EditorGUILayout.TextField(property.Name, (string)_value);
                            }
                            // else if (_type == typeof(Vector2))
                            // {
                            // 	_value = EditorGUILayout.Vector2Field(property.Name, (Vector2)_value);
                            // }
                            // else if (_type == typeof(Vector3))
                            // {
                            // 	_value = EditorGUILayout.Vector3Field(property.Name, (Vector3)_value);
                            // }
                            else if (_type == typeof(Color))
                            {
                                _value = EditorGUILayout.ColorField(property.Name, (Color)_value);
                            }
                            else if (_type == typeof(Bounds))
                            {
                                _value = EditorGUILayout.BoundsField(property.Name, (Bounds)_value);
                            }
                            else if (_type == typeof(Rect))
                            {
                                _value = EditorGUILayout.RectField(property.Name, (Rect)_value);
                            }
                            else if (_type == typeof(Enum))
                            {
                                _value = EditorGUILayout.EnumPopup(property.Name, (Enum)_value);
                            }
                            // else if (_type == typeof(Object))
                            // {
                            // 	_value = EditorGUILayout.ObjectField(property.Name, (Object)property.GetValue(), Object);
                            // }
                            else
                            {
                                Debug.Log("THIS TYPE IS NOT SUPPORTED!");
                                isTypeSupported = false;
                            }

                            if (isTypeSupported == true)
                            {
                                property.SetValue(view.Entity.Components.ElementAt(i), _value, null);
                            }

                            EditorGUILayout.EndHorizontal();
                        }
                        EditorGUILayout.EndVertical();
                    }
                }
            }
            else
            {
                EditorGUILayout.BeginHorizontal(GUI.skin.box);
                var interfaceType = typeof(IComponent);
                components = AppDomain.CurrentDomain.GetAssemblies()
                                    .SelectMany(s => s.GetTypes())
                                    .Where(p => interfaceType.IsAssignableFrom(p)).ToList();
                types = components.Select(_ => _.ToString()).ToArray();
                var index = -1;
                index = EditorGUILayout.Popup("Add Component", index, types);
                if (index >= 0)
                {
                    var component = Activator.CreateInstance(components[index]);
                    view.Components.Add(component.GetType().ToString());
                    var json = component.SerializeComponent();
                    view.Properties.Add(json.ToString());
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal(GUI.skin.box);
                EditorGUILayout.LabelField("Components (" + view.Components.Count() + ")", EditorStyles.boldLabel);
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
                    for (var i = 0; i < view.Components.Count(); i++)
                    {
                        EditorGUILayout.BeginVertical(GUI.skin.box);
                        EditorGUILayout.BeginHorizontal();

                        var label = view.Components[i];
                        var dotIndex = label.LastIndexOf(".");
												if (dotIndex != -1)
												{
                            label = label.Substring(dotIndex + 1);
												}
                        EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
                        if (GUILayout.Button("-", GUILayout.Width(20), GUILayout.Height(15)))
                        {
                            componentsToRemove.Add(i);
                        }
                        EditorGUILayout.EndHorizontal();

                        var type = GetTypeWithAssembly(view.Components[i]);
                        var component = Activator.CreateInstance(type);
                        var node = JSON.Parse(view.Properties[i]);
                        component.DeserializeComponent(node);

                        var members = component.GetType().GetMembers();
                        foreach (var property in component.GetType().GetProperties())
                        {
                            bool isTypeSupported = true;

                            EditorGUILayout.BeginHorizontal();
                            var _type = property.PropertyType;
                            var _value = property.GetValue(component, null);

                            if (_type == typeof(int))
                            {
                                _value = EditorGUILayout.IntField(property.Name, (int)_value);
                            }
                            else if (_type == typeof(float))
                            {
                                _value = EditorGUILayout.FloatField(property.Name, (float)_value);
                            }
                            else if (_type == typeof(bool))
                            {
                                _value = EditorGUILayout.Toggle(property.Name, (bool)_value);
                            }
                            else if (_type == typeof(string))
                            {
                                _value = EditorGUILayout.TextField(property.Name, (string)_value);
                            }
                            // else if (_type == typeof(Vector2))
                            // {
                            // 	_value = EditorGUILayout.Vector2Field(property.Name, (Vector2)_value);
                            // }
                            // else if (_type == typeof(Vector3))
                            // {
                            // 	_value = EditorGUILayout.Vector3Field(property.Name, (Vector3)_value);
                            // }
                            else if (_type == typeof(Color))
                            {
                                _value = EditorGUILayout.ColorField(property.Name, (Color)_value);
                            }
                            else if (_type == typeof(Bounds))
                            {
                                _value = EditorGUILayout.BoundsField(property.Name, (Bounds)_value);
                            }
                            else if (_type == typeof(Rect))
                            {
                                _value = EditorGUILayout.RectField(property.Name, (Rect)_value);
                            }
                            else if (_type == typeof(Enum))
                            {
                                _value = EditorGUILayout.EnumPopup(property.Name, (Enum)_value);
                            }
                            // else if (_type == typeof(Object))
                            // {
                            // 	_value = EditorGUILayout.ObjectField(property.Name, (Object)property.GetValue(), Object);
                            // }
                            else
                            {
                                Debug.Log("THIS TYPE IS NOT SUPPORTED!");
                                isTypeSupported = false;
                            }

                            if (isTypeSupported == true)
                            {
                                property.SetValue(component, _value, null);
                            }
                            view.Components[i] = component.GetType().ToString();
                            var json = component.SerializeComponent();
                            view.Properties[i] = json.ToString();
                            EditorGUILayout.EndHorizontal();
                        }
                        EditorGUILayout.EndVertical();
                    }

                    for (var i = 0; i < componentsToRemove.Count(); i++)
                    {
                        view.Components.RemoveAt(componentsToRemove[i]);
                        view.Properties.RemoveAt(componentsToRemove[i]);
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

        public static Type GetTypeWithAssembly(string typeName)
        {
            var type = Type.GetType(typeName);
            if (type != null) return type;
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = a.GetType(typeName);
                if (type != null)
                    return type;
            }
            return null;
        }
    }
}
