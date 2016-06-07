using EcsRx.Pools;
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

    [CustomEditor(typeof(RegisterAsEntity))]
    [Serializable]
    public class RegisterAsEntityInspector : Editor
    {
        private RegisterAsEntity registerAsEntity;

        private readonly IEnumerable<Type> allComponentTypes = AppDomain.CurrentDomain.GetAssemblies()
                                .SelectMany(s => s.GetTypes())
                                .Where(p => typeof(IComponent).IsAssignableFrom(p));

        private bool showComponents;

        private void PoolSection()
        {
            this.UseVerticalBoxLayout(() =>
            {
                registerAsEntity.PoolName = this.WithTextField("Pool: ", registerAsEntity.PoolName);
            });
        }

        private void RemoveComponentSection()
        {
            EditorGUILayout.BeginVertical(EditorExtensions.DefaultBoxStyle);
            this.WithHorizontalLayout(() =>
            {
                this.WithLabel("Components (" + registerAsEntity.StagedComponents.Count() + ")");
                if (this.WithIconButton("▸")) { showComponents = false; }
                if (this.WithIconButton("▾")) { showComponents = true; }
            });

            var componentsToRemove = new List<int>();
            if (showComponents)
            {
                for (var i = 0; i < registerAsEntity.StagedComponents.Count(); i++)
                {
                    this.UseVerticalBoxLayout(() =>
                    {
                        var componentType = registerAsEntity.StagedComponents[i];
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
                        });
                    });
                }
            }
            EditorGUILayout.EndVertical();

            for (var i = 0; i < componentsToRemove.Count(); i++)
            { registerAsEntity.StagedComponents.RemoveAt(componentsToRemove[i]); }
        }

        private void ComponentSelectionSection()
        {
            this.UseVerticalBoxLayout(() =>
            {
                var availableTypes = allComponentTypes
                    .Where(x => !registerAsEntity.StagedComponents.Contains(x.ToString()))
                    .ToArray();

                var types = availableTypes.Select(x => string.Format("{0} [{1}]", x.Name, x.Namespace)).ToArray();
                var index = -1;
                index = EditorGUILayout.Popup("Add Component", index, types);
                if (index >= 0)
                {
                    registerAsEntity.StagedComponents.Add(availableTypes.ElementAt(index).ToString());
                }
            });
        }

        private void PersistChanges()
        {
            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();
            }
        }

        public override void OnInspectorGUI()
        {
            registerAsEntity = (RegisterAsEntity)target;
            
            PoolSection();
            ComponentSelectionSection();
            RemoveComponentSection();
            PersistChanges();
        }
    }
}
