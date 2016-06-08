using System;
using System.Collections.Generic;
using System.Linq;
using Assets.EcsRx.Unity.Extensions;
using EcsRx.Components;
using EcsRx.Json;
using EcsRx.Unity.Helpers.Extensions;
using EcsRx.Unity.MonoBehaviours;
using UnityEditor;
using UnityEngine;

namespace EcsRx.Unity.Helpers
{
    [CustomEditor(typeof(EntityView))]
    public class EntityBindingInspector : Editor
    {
        private EntityView _entityView;

        public bool showComponents;

        private readonly IEnumerable<Type> allComponentTypes = AppDomain.CurrentDomain.GetAssemblies()
                                .SelectMany(s => s.GetTypes())
                                .Where(p => typeof(IComponent).IsAssignableFrom(p) && p.IsClass);


        private void PoolSection()
        {
            this.UseVerticalBoxLayout(() =>
            {
                if (GUILayout.Button("Destroy Entity"))
                {
                    _entityView.Pool.RemoveEntity(_entityView.Entity);
                    Destroy(_entityView.gameObject);
                }

                this.UseVerticalBoxLayout(() =>
                {
                    var id = _entityView.Entity.Id.ToString();
                    this.WithLabelField("Entity Id: ", id);
                });

                this.UseVerticalBoxLayout(() =>
                {
                    this.WithLabelField("Pool: ", _entityView.Pool.Name);
                });
            });
        }

        private void ComponentListings()
        {
            EditorGUILayout.BeginVertical(EditorExtensions.DefaultBoxStyle);
            this.WithHorizontalLayout(() =>
            {
                this.WithLabel("Components (" + _entityView.Entity.Components.Count() + ")");
                if (this.WithIconButton("▸")) { showComponents = false; }
                if (this.WithIconButton("▾")) { showComponents = true; }
            });

            var componentsToRemove = new List<int>();
            if (showComponents)
            {
                for (var i = 0; i < _entityView.Entity.Components.Count(); i++)
                {
                    this.UseVerticalBoxLayout(() =>
                    {
                        var componentType = _entityView.Entity.Components.ElementAt(i).GetType();
                        var typeName = componentType.Name;
                        var typeNamespace = componentType.Namespace;

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
                var component = _entityView.Entity.Components.ElementAt(i);
                _entityView.Entity.RemoveComponent(component);
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
            var component = _entityView.Entity.Components.ElementAt(index);

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
                    Debug.LogWarning("This type is not supported: " + _type.Name + " - In component: " + component.GetType().Name);
                    isTypeSupported = false;
                }

                if (isTypeSupported == true)
                {
                    property.SetValue(component, _value, null);
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        private void ComponentSelectionSection()
        {
            this.UseVerticalBoxLayout(() =>
            {
                var availableTypes = allComponentTypes
                    .Where(x => !_entityView.Entity.Components.Select(y => y.GetType()).Contains(x))
                    .ToArray();

                var types = availableTypes.Select(x => string.Format("{0} [{1}]", x.Name, x.Namespace)).ToArray();
                var index = -1;
                index = EditorGUILayout.Popup("Add Component", index, types);
                if (index >= 0)
                {
                    var component = (IComponent)Activator.CreateInstance(availableTypes[index]);
                    _entityView.Entity.AddComponent(component);
                }
            });
        }

        public override void OnInspectorGUI()
        {
            _entityView = (EntityView)target;

            if (_entityView.Entity == null)
            {
                EditorGUILayout.LabelField("No Entity Assigned");
                return;
            }

            PoolSection();
            EditorGUILayout.Space();
            ComponentSelectionSection();
            ComponentListings();
        }
    }
}