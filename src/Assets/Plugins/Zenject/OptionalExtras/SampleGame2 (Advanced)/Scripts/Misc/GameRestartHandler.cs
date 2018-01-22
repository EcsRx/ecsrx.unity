using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Zenject.SpaceFighter
{
    public class GameRestartHandler : IInitializable, IDisposable, ITickable
    {
        readonly Settings _settings;

        PlayerDiedSignal _playerDiedSignal;
        bool _isDelaying;
        float _delayStartTime;

        public GameRestartHandler(
            Settings settings,
            PlayerDiedSignal playerDiedSignal)
        {
            _playerDiedSignal = playerDiedSignal;
            _settings = settings;
        }

        public void Initialize()
        {
            _playerDiedSignal += OnPlayerDied;
        }

        public void Dispose()
        {
            _playerDiedSignal -= OnPlayerDied;
        }

        public void Tick()
        {
            if (_isDelaying)
            {
                if (Time.realtimeSinceStartup - _delayStartTime > _settings.RestartDelay)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }
        }

        void OnPlayerDied()
        {
            // Wait a bit before restarting the scene
            _delayStartTime = Time.realtimeSinceStartup;
            _isDelaying = true;
        }

        [Serializable]
        public class Settings
        {
            public float RestartDelay;
        }
    }
}
