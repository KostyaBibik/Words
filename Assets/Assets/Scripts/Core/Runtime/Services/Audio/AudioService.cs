using Enums;
using UnityEngine;
using Zenject;
using AudioSettings = Gameplay.Data.Audio.AudioSettings;

namespace Core.Services.Audio
{
    public class AudioService : IAudioService, IInitializable
    {
        private AudioSource _audioSource;
        private bool _activityStatus = true;
        
        private readonly float _masterVolume = 1f;
        private AudioSettings _settings;
        
        private const string AUDIO_SOURCE_NAME = "AudioSource";
        
        public void Initialize()
        {
            CreateAudioSource();
        }

        public void SetSettings(AudioSettings settings) =>
            _settings = settings;

        public void PlaySound(ESoundType type, float volumeScale = 1f)
        {
            if(!_activityStatus)
                return;
            
            if(_settings == null)
                return;
            
            var clip = _settings.GetClip(type);
            if (clip != null)
            {
                _audioSource.PlayOneShot(clip, _masterVolume * volumeScale);
            }
        }

        public void SwapSoundsActiveStatus() =>
            _activityStatus = !_activityStatus;

        private void CreateAudioSource()
        {
            var sourceGO = new GameObject(AUDIO_SOURCE_NAME);
            _audioSource = sourceGO.AddComponent<AudioSource>();
            
            _audioSource.spatialBlend = 0;
            _audioSource.playOnAwake = false;
        }
    }
}