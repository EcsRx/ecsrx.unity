using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EcsRx.Entities;
using EcsRx.Pools;
using EcsRx.Unity.Components;
using EcsRx.Extensions;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Zenject;

namespace EcsRx.Unity.Managers
{
    public class SceneManager : IInitializable, IDisposable
    {
        public class SceneEventArg
        {
            public Scene Scene;
            public LoadSceneMode SceneMode;
        }

        private IPoolManager poolManager;
        private CompositeDisposable disposables = new CompositeDisposable();
        private IList<string> scenes;
        private IDictionary<string, IEntity> sceneEntities;

        public SceneManager(IPoolManager poolManager)
        {
            this.poolManager = poolManager;
            sceneEntities = new Dictionary<string, IEntity>();
            scenes = new List<string>();

            IPool defaultPool = poolManager.GetPool();
            SceneLoadedAsObservable().Subscribe(arg =>
            {
                scenes.Add(arg.Scene.name);
                var entity = defaultPool.CreateEntity();
                var sceneComponent = new SceneComponent { Scene = arg.Scene };
                entity.AddComponent(sceneComponent);
                sceneEntities.Add(new KeyValuePair<string, IEntity>(arg.Scene.name, entity));
            }).AddTo(disposables);

            SceneUnloadedAsObservable().Subscribe(scene =>
            {
                if (sceneEntities.ContainsKey(scene.name))
                {
                    scenes.Remove(scene.name);
                    defaultPool.RemoveEntity(sceneEntities[scene.name]);
                    sceneEntities.Remove(scene.name);
                }
            }).AddTo(disposables);
        }

        public void Initialize()
        {
           
        }

        public void Dispose()
        {
            disposables.Dispose();
            sceneEntities.Clear();
        }

        public IObservable<SceneEventArg> SceneLoadedAsObservable()
        {
            return Observable.FromEvent<UnityAction<Scene, LoadSceneMode>, SceneEventArg>(h => (scene, sceneMode) => h(new SceneEventArg { Scene = scene, SceneMode = sceneMode }),
               h => UnityEngine.SceneManagement.SceneManager.sceneLoaded += h,
               h => UnityEngine.SceneManagement.SceneManager.sceneLoaded -= h);
        }

        public IObservable<Scene> SceneUnloadedAsObservable()
        {
            return Observable.FromEvent<UnityAction<Scene>, Scene>(h => scene => h(scene),
                h => UnityEngine.SceneManagement.SceneManager.sceneUnloaded += h,
                h => UnityEngine.SceneManagement.SceneManager.sceneUnloaded -= h);
        }

        public void AddScene(string sceneName)
        {
            LoadScene(sceneName);
        }

        public Scene PopScene()
        {
            string sceneName = scenes.Last();
            Scene scene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName);
            UnloadScene(sceneName);
            scenes.RemoveAt(scenes.Count-1);
            return scene;
        }

        public Scene RemoveScene(string sceneName)
        {
            Scene scene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName);
            UnloadScene(sceneName);
            
            return scene;
        }

        private void LoadScene(string sceneName)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }

        private void UnloadScene(string sceneName)
        {
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneName);
        }

      
    }

}

