using System;
using EcsRx.Events;
using EcsRx.Persistence.Database.Accessor;
using EcsRx.Unity.MonoBehaviours.Editor.Events;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EcsRx.Unity.MonoBehaviours.Editor.Infrastructure
{
    [InitializeOnLoad]
    public class SceneListener
    {
        private static readonly IEventSystem EventSystem;
        private static readonly IApplicationDatabaseAccessor DatabaseAccessor;
        private static string currentSceneName = "";

        static SceneListener()
        {
            Debug.Log("Scene Listener Active");

            EventSystem = EditorContext.Container.Resolve<IEventSystem>();
            DatabaseAccessor = EditorContext.Container.Resolve<IApplicationDatabaseAccessor>();
            
            EditorApplication.hierarchyWindowChanged += HierarchyWindowChanged;
        }

        private static void HierarchyWindowChanged()
        {
            var scene = SceneManager.GetActiveScene();
            if(currentSceneName == scene.name) { return; }

            if (string.IsNullOrEmpty(currentSceneName))
            { FirstScene(); }
            else
            { SceneChanged(); }

            currentSceneName = scene.name;
        }

        private static void SceneChanged()
        {
            DatabaseAccessor.PersistDatabase(() => {
                Debug.Log("Database Saved For Scene");
                DatabaseAccessor.ResetDatabase();
                DatabaseAccessor.ReloadDatabase(() =>
                {
                    Debug.Log("Scene DB Loaded For New Scene");
                    EventSystem.Publish(new ApplicationDatabaseLoadedEvent());
                });
            });
        }

        private static void FirstScene()
        {
            DatabaseAccessor.ReloadDatabase(() =>
            {
                Debug.Log("Scene DB Loaded");
                EventSystem.Publish(new ApplicationDatabaseLoadedEvent());
            });
        }
    }
}