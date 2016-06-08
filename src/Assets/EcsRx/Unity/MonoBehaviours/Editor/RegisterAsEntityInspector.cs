using Assets.EcsRx.Unity.Extensions;
using EcsRx.Json;
using EcsRx.Unity.Components;
using EcsRx.Unity.Helpers.Extensions;
using EcsRx.Unity.MonoBehaviours;

namespace EcsRx.Unity.Helpers
{
    using UnityEngine;
    using UnityEditor;
    using System.Collections.Generic;
    using EcsRx.Components;
    using System;
    using System.Linq;

    [CustomEditor(typeof(SetupView))]
    [Serializable]
    public class RegisterAsEntityInspector : Editor
    {
        private SetupView _setupView;

        private readonly IEnumerable<Type> allComponentTypes = AppDomain.CurrentDomain.GetAssemblies()
                                .SelectMany(s => s.GetTypes())
                                .Where(p => typeof(IComponent).IsAssignableFrom(p) && p.IsClass && !typeof(ViewComponent).IsAssignableFrom(p));

        private bool showComponents;

        private void PoolSection()
        {
            this.UseVerticalBoxLayout(() =>
            {
                _setupView.PoolName = this.WithTextField("Pool: ", _setupView.PoolName);
            });
        }

        private void ComponentListings()
        {
            EditorGUILayout.BeginVertical(EditorExtensions.DefaultBoxStyle);
            this.WithHorizontalLayout(() =>
            {
                this.WithLabel("Components (" + _setupView.Components.Count() + ")");
                if (this.WithIconButton("▸")) { showComponents = false; }
                if (this.WithIconButton("▾")) { showComponents = true; }
            });

            var componentsToRemove = new List<int>();
            if (showComponents)
            {
                for (var i = 0; i < _setupView.Components.Count(); i++)
                {
                    this.UseVerticalBoxLayout(() =>
                    {
                        var componentType = _setupView.Components[i];
                        var namePortions = componentType.Split(',')[0].Split('.');
                        var typeName = namePortions.Last();
                        var typeNamespace = string.Join(".", namePortions.Take(namePortions.Length - 1).ToArray());

                        this.WithVerticalLayout(() =>
                        {
                            this.WithHorizontalLayout(() =>
                            {
                                if (this.WithIconButton("-"))
                                {
                                    componentsToRemove.Add(i);
                                }

                                this.WithLabel(typeName);
                            });

                            EditorGUILayout.LabelField(typeNamespace);
                            EditorGUILayout.Space();
                        });

                        ShowComponentProperties(i);
                    });
                }
            }

            EditorGUILayout.EndVertical();

            for (var i = 0; i < componentsToRemove.Count(); i++)
            {
                _setupView.Components.RemoveAt(componentsToRemove[i]);
                _setupView.Properties.RemoveAt(componentsToRemove[i]);
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

        private void ShowComponentProperties(int index)
        {
            var type = GetTypeWithAssembly(_setupView.Components[index]);
            var component = Activator.CreateInstance(type);
            var node = JSON.Parse(_setupView.Properties[index]);
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
                    Debug.LogWarning("This type is not supported: " + _type.Name + " - In component: " + type.Name);
                    isTypeSupported = false;
                }

                if (isTypeSupported == true)
                {
                    property.SetValue(component, _value, null);
                }
                _setupView.Components[index] = component.GetType().ToString();
                var json = component.SerializeComponent();
                _setupView.Properties[index] = json.ToString();
                EditorGUILayout.EndHorizontal();
            }
        }

        private void ComponentSelectionSection()
        {
            this.UseVerticalBoxLayout(() =>
            {
                var availableTypes = allComponentTypes
                    .Where(x => !_setupView.Components.Contains(x.ToString()))
                    .ToArray();

                var types = availableTypes.Select(x => string.Format("{0} [{1}]", x.Name, x.Namespace)).ToArray();
                var index = -1;
                index = EditorGUILayout.Popup("Add Component", index, types);
                if (index >= 0)
                {
                    var component = availableTypes.ElementAt(index);
                    var componentName = component.ToString();
                    _setupView.Components.Add(componentName);
                    var json = component.SerializeComponent();
                    _setupView.Properties.Add(json.ToString());
                }
            });
        }

        private void PersistChanges()
        {
            if (GUI.changed)
            { this.SaveActiveSceneChanges(); }
        }

        public override void OnInspectorGUI()
        {
            _setupView = (SetupView)target;
            
            PoolSection();
            EditorGUILayout.Space();
            ComponentSelectionSection();
            ComponentListings();
            PersistChanges();
        }
    }
}
