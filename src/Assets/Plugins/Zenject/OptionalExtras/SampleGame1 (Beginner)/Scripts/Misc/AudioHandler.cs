using System;
using UnityEngine;
using Zenject;

namespace Zenject.Asteroids
{
    public class AudioHandler : IInitializable, IDisposable
    {
        readonly Settings _settings;
        readonly AudioSource _audioSource;

        ShipCrashedSignal _shipCrashedSignal;

        public AudioHandler(
            AudioSource audioSource,
            Settings settings,
            ShipCrashedSignal shipCrashedSignal)
        {
            _shipCrashedSignal = shipCrashedSignal;
            _settings = settings;
            _audioSource = audioSource;
        }

        public void Initialize()
        {
            _shipCrashedSignal += OnShipCrashed;
        }

        public void Dispose()
        {
            _shipCrashedSignal -= OnShipCrashed;
        }

        void OnShipCrashed()
        {
            _audioSource.PlayOneShot(_settings.CrashSound);
        }

        [Serializable]
        public class Settings
        {
            public AudioClip CrashSound;
        }
    }
}
