using System.Linq;
using EcsRx.Extensions;
using EcsRx.Systems;
using EcsRx.Systems.Executor;
using UnityEditor;
using UnityEngine;

namespace EcsRx.Unity.Helpers
{
    [CustomEditor(typeof(ActiveSystemsViewer))]
    public class ActiveSystemsViewerInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            var activeSystemsViewer = (ActiveSystemsViewer)target;
            var executor = activeSystemsViewer.SystemExecutor;

            if (executor == null)
            {
                EditorGUILayout.LabelField("System Executor Inactive");
                return;
            }

            var isNormalExecutorType = executor is SystemExecutor;
            var typedExecutor = executor as SystemExecutor;

            EditorGUILayout.TextField("Setup Systems");
            EditorGUILayout.Space();
            foreach (var system in executor.Systems.OfType<ISetupSystem>())
            {
                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField("System: " + system.GetType().Name);
                if(isNormalExecutorType)
                {  EditorGUILayout.LabelField("Active Subscriptions: " + typedExecutor.GetSubscriptionCountForSystem(system)); }
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
                EditorGUILayout.Space();
            }

            EditorGUILayout.TextField("Group Systems");
            EditorGUILayout.Space();
            foreach (var system in executor.Systems.OfType<IReactToGroupSystem>())
            {
                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField("System: " + system.GetType().Name);
                if (isNormalExecutorType)
                { EditorGUILayout.LabelField("Active Subscriptions: " + typedExecutor.GetSubscriptionCountForSystem(system)); }
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
                EditorGUILayout.Space();
            }

            EditorGUILayout.TextField("Entity Systems");
            EditorGUILayout.Space();
            foreach (var system in executor.Systems.OfType<IReactToEntitySystem>())
            {
                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField("System: " + system.GetType().Name);
                if (isNormalExecutorType)
                { EditorGUILayout.LabelField("Active Subscriptions: " + typedExecutor.GetSubscriptionCountForSystem(system)); }
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
                EditorGUILayout.Space();
            }

            EditorGUILayout.TextField("Data Systems");
            EditorGUILayout.Space();
            foreach (var system in executor.Systems.Where(x => x.IsReactiveDataSystem()))
            {
                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField("System: " + system.GetType().Name);
                if (isNormalExecutorType)
                { EditorGUILayout.LabelField("Active Subscriptions: " + typedExecutor.GetSubscriptionCountForSystem(system)); }
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
                EditorGUILayout.Space();
            }

            EditorGUILayout.TextField("Total Subscriptions Across All Systems: " + typedExecutor.GetTotalSubscriptions());
        }
    }
}