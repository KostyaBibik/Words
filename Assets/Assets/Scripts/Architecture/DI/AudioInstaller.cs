using Core.Services.Audio;
using UnityEngine;
using Zenject;
using AudioSettings = Gameplay.Data.Audio.AudioSettings;

namespace Architecture.DI
{
    public class AudioInstaller : MonoInstaller
    {
        [SerializeField] private AudioSettings _settings;
        
        public override void InstallBindings()
        {
            BindConfigs();
            BindServices();
        }

        private void BindConfigs()
        {
            Container.BindInstance(_settings);
        }

        private void BindServices()
        {
            Container.BindInterfacesTo<AudioService>().AsSingle();
        }
    }
}