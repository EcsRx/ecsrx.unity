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
        
        private IEnumerable<Type> GetAvailableComponents()
        {
            var type = typeof(IComponent);
            return AppDomain.CurrentDomain.GetAssemblies()
                                .SelectMany(s => s.GetTypes())
                                .Where(p => type.IsAssignableFrom(p));
        }

        public override void OnInspectorGUI()
        {
            registerAsEntity = (RegisterAsEntity)target;
            
            var components = GetAvailableComponents();

            this.UseBoxLayout(() =>
            {
                var types = components.Select(_ => _.ToString()).ToArray();
                var index = -1;
                index = EditorGUILayout.Popup("Add Component", index, types);
                if (index >= 0)
                {
                    registerAsEntity.StagedComponents.Add(types[index]);
                }
            });

            var componentsToRemove = new List<int>();
            for (var i = 0; i < registerAsEntity.StagedComponents.Count(); i++)
            {
                this.UseBoxLayout(() =>
                {
                    EditorGUILayout.LabelField(registerAsEntity.StagedComponents[i]);
                    if (this.WithIconButton("-"))
                    { componentsToRemove.Add(i); }
                });
            }
            
            for (var i = 0; i < componentsToRemove.Count(); i++)
            {
                registerAsEntity.StagedComponents.RemoveAt(componentsToRemove[i]);
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
