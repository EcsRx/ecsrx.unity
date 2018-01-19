using UnityEngine;

namespace Zenject.SpaceFighter
{
    public interface IAudioPlayer
    {
        void Play(AudioClip clip, float volume);
        void Play(AudioClip clip);
    }

    public class AudioPlayer : IAudioPlayer
    {
        readonly Camera _camera;

        public AudioPlayer(Camera camera)
        {
            _camera = camera;
        }

        public void Play(AudioClip clip)
        {
            Play(clip, 1);
        }

        public void Play(AudioClip clip, float volume)
        {
            _camera.GetComponent<AudioSource>().PlayOneShot(clip, volume);
        }
    }
}
