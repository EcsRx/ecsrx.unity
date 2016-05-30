#if UNITY_EDITOR

using System;
using UnityEngine;
using UnityEditor;
using ModestTree.Util;
using System.Linq;

namespace Zenject
{
    public class ValidationSceneDisabler : MonoBehaviour
    {
        static ValidationSceneDisabler _instance;

        public static ValidationSceneDisabler Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameObject("ValidationSceneDisabler")
                        .AddComponent<ValidationSceneDisabler>();
                }

                return _instance;
            }
        }

        public bool StopPlayingAsap
        {
            get;
            set;
        }

        public void LazyInit()
        {
            // Do Nothing
        }

        public void Awake()
        {
            StopPlayingAsap = true;
        }

        public void Update()
        {
            DisableEverything();

            if (StopPlayingAsap)
            {
                EditorApplication.isPlaying = false;
            }
        }

        public void DisableEverything()
        {
            // Disable everything so that nothing executes Start() or Update() etc.
            foreach (var gameObj in UnityEngine.Object.FindObjectsOfType<GameObject>()
                .Where(x => x.transform.parent == null))
            {
                if (gameObj != this.gameObject)
                {
                    gameObj.SetActive(false);
                }
            }
        }
    }
}

#endif
