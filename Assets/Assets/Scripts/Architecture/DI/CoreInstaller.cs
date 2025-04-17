using Core.Services.DataRepository;
using Infrastructure;
using Infrastructure.RemoteConfig;
using Zenject;

namespace Assets.Scripts.Architecture.DI
{
    public class CoreInstaller : MonoInstaller<CoreInstaller>
    {
        public override void InstallBindings() 
        {
            Container.BindInterfacesTo<GameDataRepository>().AsSingle();
            Container.BindInterfacesTo<RemoteLevelLoader>().AsSingle();
            Container.BindInterfacesTo<LevelProcessor>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<AppStartup>().AsSingle().NonLazy();
        }
    }
}