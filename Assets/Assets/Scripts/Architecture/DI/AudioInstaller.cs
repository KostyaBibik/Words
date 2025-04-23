using Core.Services.Audio;
using Zenject;

namespace Architecture.DI
{
    public class AudioInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindServices();
        }

        private void BindServices()
        {
            Container.BindInterfacesTo<AudioService>().AsSingle();
        }
    }
}