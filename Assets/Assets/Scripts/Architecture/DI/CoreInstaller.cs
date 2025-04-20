using Core.GameState;
using Core.GameState.States;
using Core.Services.DataRepository;
using Infrastructure;
using Infrastructure.RemoteConfig;
using Zenject;

namespace Architecture.DI
{
    public class CoreInstaller : MonoInstaller<CoreInstaller>
    {
        public override void InstallBindings() 
        {
            Container.BindInterfacesTo<GameDataRepository>().AsSingle();

            BindInitSystems();
            
            BindGameStates();
            
            BindEntryPoint();
        }

        private void BindInitSystems()
        {
            Container.BindInterfacesTo<RemoteLevelLoader>().AsSingle();
            Container.BindInterfacesTo<LevelProcessor>().AsSingle();
        }

        private void BindGameStates()
        {
            Container.BindInterfacesTo<GameStateMachine>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<MainMenuState>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelGenerationState>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameplayState>().AsSingle();
            Container.BindInterfacesAndSelfTo<VictoryState>().AsSingle();
        }

        private void BindEntryPoint()
        {
            Container.BindInterfacesAndSelfTo<AppStartup>().AsSingle().NonLazy();
        }
    }
}