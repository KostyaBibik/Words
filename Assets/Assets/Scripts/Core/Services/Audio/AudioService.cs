using Enums;
using UnityEngine;
using Zenject;
using AudioSettings = Gameplay.Data.Audio.AudioSettings;

namespace Core.Services.Audio
{
    public class AudioService : IAudioService, IInitializable
    {
        private AudioSource _audioSource;
        
        private readonly float _masterVolume = 1f;
        private readonly AudioSettings _settings;

        private const string AudioSourceName = "AudioSource";
        
        public AudioService(AudioSettings settings)
        {
            _settings = settings;
        }
        
        public void Initialize()
        {
            CreateAudioSource();
        }

        public void PlaySound(ESoundType type, float volumeScale = 1f)
        {
            var clip = _settings.GetClip(type);
            if (clip != null)
            {
                _audioSource.PlayOneShot(clip, _masterVolume * volumeScale);
            }
        }

        private void CreateAudioSource()
        {
            var sourceGO = new GameObject(AudioSourceName);
            _audioSource = sourceGO.AddComponent<AudioSource>();
            
            _audioSource.spatialBlend = 0;
            _audioSource.playOnAwake = false;
        }
    }
}